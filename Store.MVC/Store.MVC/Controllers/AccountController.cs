
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Store.MVC.ViewModels;
using Store.MVC.WebServiceAccess.Base;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Net;
using System.Threading;
using System.Security.Principal;
using Store.Models.Entities;
using Store.Service.Model;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Identity;

namespace Store.MVC.Controllers
{
    
    public class AccountController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IWebApiCalls _apiCalls;
      

        public AccountController(IConfiguration configuration, IWebApiCalls apiCalls)
        {
            _configuration = configuration;
            _apiCalls = apiCalls;
        }
        [HttpGet()]
        public IActionResult Login()
        {
            return View();
        }

        [ValidateAntiForgeryToken]
        [HttpPost()]
        public async Task<ActionResult> Login(LoginViewModel model,[FromQuery] string returnUrl = null)
        {

            // var getTokenUrl = string.Format("api/account/login", "http://localhost:40001/");
            if (!ModelState.IsValid) return View(model);
            try
            {
                CookieViewModel Usermodel = await _apiCalls.LoginAsync(model);

                await CreateCookie(Usermodel);

  

            }
            catch (WebException ex)
            {
                if (ex.Status == WebExceptionStatus.ProtocolError)
                {

                    ModelState.AddModelError(string.Empty, "Password Or Email incorrect");
                    return View(model);

                }

            }
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction("","products");

        }
        
        [HttpGet()]
        public IActionResult Register()
        {
            return View();
        }
        [ValidateAntiForgeryToken]
        [HttpPost()]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid) return View(model);
            try
            {
                var token = await _apiCalls.RegisterAsync(model);


                return RedirectToAction("Login", "Account");

            }
            catch (Exception e)
            {
                ModelState.AddModelError(string.Empty, "Somting Worng");
                return View();
            }



        }
        public ActionResult LogOut()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            ModelState.Clear();
            return RedirectToAction("","products");
        }

        protected async Task CreateCookie(CookieViewModel userModel)
        {

            AuthenticationProperties options = new AuthenticationProperties
            {
                AllowRefresh = true,
                IsPersistent = true,
                ExpiresUtc = userModel.Expires
            };

            var token = new JwtSecurityTokenHandler().ReadJwtToken(userModel.Token) as JwtSecurityToken;

             



             var claimsIdentity = new ClaimsIdentity(
                token.Claims,
                CookieAuthenticationDefaults.AuthenticationScheme); 

            claimsIdentity.AddClaim(new Claim("AcessToken", userModel.Token));

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity), options);
        }
    }

}