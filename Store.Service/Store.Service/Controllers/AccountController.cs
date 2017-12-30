using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Store.DAL.EF;
using Store.DAL.Repos.Interfaces;
using Store.Models.Entities;
using Store.Service.Model;

namespace Store.Service.Controllers
{
    [Route("api/[controller]/[action]")]
    public class AccountController : Controller
    {
        private readonly SignInManager<UserEntity> _signInManager;
        private readonly UserManager<UserEntity> _userManager;
        private readonly IConfiguration _configuration;
        private readonly ICustomerRepo _customerRepo;

        public AccountController(
            UserManager<UserEntity> userManager,
            SignInManager<UserEntity> signInManager,
            IConfiguration configuration,
            ICustomerRepo customerRepo)

        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _customerRepo = customerRepo;
        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<object> Login([FromBody] LoginDto model)
        {
            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);



            if (result.Succeeded)
            {
                var appUser = _userManager.Users.SingleOrDefault(r => r.Email == model.Email);

                var customer = _customerRepo.FindByUserId(appUser.Id);
                appUser.Customer = customer;

                var clamis = await _userManager.GetClaimsAsync(appUser);

                return GenerateJwtToken(clamis.ToArray());
            }

            return NotFound(result);

        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = new UserEntity
            {
                UserName = model.EmailAddress,
                Email = model.EmailAddress,
                Customer = new Customer
                {
                    FullName = model.FullName,

                }
            };
            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                user = _userManager.Users.SingleOrDefault(r => r.Email == model.EmailAddress);
                if(user == null)
                {
                    throw new ArgumentNullException("cant find user");
                
                }
                await AddClaimsToUser(user);

                return Created("", result);
            }
            else
            {
                throw new ValidationException(result.ToString());
            }
        }

        private object GenerateJwtToken(Claim[] claims)
        {

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(Convert.ToDouble(_configuration["JwtExpireDays"]));

            var token = new JwtSecurityToken(
                _configuration["JwtIssuer"],
                _configuration["JwtIssuer"],
                claims,
                expires: expires,
                signingCredentials: creds
            );
            var Securitytoken = new JwtSecurityTokenHandler().WriteToken(token);



            return new CookieViewModel
            {
                Token = Securitytoken,
                Expires = expires,

            };
        }
        private async Task AddClaimsToUser(UserEntity user)
        {

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier,user.Id),
                new Claim(ClaimTypes.Name, user.Customer.FullName),
                new Claim(ClaimTypes.Authentication, user.Customer.Id.ToString()),
            };

           var result = await _userManager.AddClaimsAsync(user, claims);
            if (!result.Succeeded)
            {
                throw new Exception(result.ToString());
            }

        }

        public class LoginDto
        {
            [Required]
            public string Email { get; set; }

            [Required]
            public string Password { get; set; }

        }

        public class RegisterDto
        {
            [Required, DataType(DataType.EmailAddress)]
            public string EmailAddress { get; set; }

            [Required, DataType(DataType.Password)]
            [StringLength(100, ErrorMessage = "PASSWORD_MIN_LENGTH", MinimumLength = 6)]
            public string Password { get; set; }

            [Required]
            public string FullName { get; set; }

            [DataType(DataType.Password), Required, MinLength(4), Display(Name = "ConfrimPassword")]
            public string ConfrimPassword { get; set; }


        }
    }
}