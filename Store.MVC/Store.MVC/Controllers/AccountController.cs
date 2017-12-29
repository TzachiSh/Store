
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

        [HttpPost()]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {

            // var getTokenUrl = string.Format("api/account/login", "http://localhost:40001/");
            if (!ModelState.IsValid)
            {
                return View();
            }
            try
            {
                CookieViewModel Usermodel = await _apiCalls.LoginAsync(model);

                await CreateCookie(Usermodel);

            }
            catch (WebException ex)
            {
                if(ex.Status == WebExceptionStatus.ProtocolError)
                {

                    ModelState.AddModelError(string.Empty, "User not Found");
                    return View(model);

                }
               
            }
            
           
            return RedirectToAction("", returnUrl);
        }
        [HttpGet()]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost()]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            try
            {
                var token = await _apiCalls.RegisterAsync(model);


                    return RedirectToAction("Login", "Account");

            }
            catch(Exception e)
            {
                return View();
            }
            
            

        }
        public ActionResult LogOut()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            ModelState.Clear();
            return RedirectToAction("", "Products");
        }

        protected async Task CreateCookie(CookieViewModel userModel)
        {

            AuthenticationProperties options = new AuthenticationProperties
            {
                AllowRefresh = true,
                IsPersistent = true,
                ExpiresUtc = userModel.Expires
            };

            var claims = new List<Claim>
                   {
                       new Claim(ClaimTypes.Name, userModel.CustomerName ),
                       new Claim(ClaimTypes.NameIdentifier, userModel.CustomerId.ToString()),
                       new Claim("AcessToken", string.Format(userModel.Token)),

                   };

            var claimsIdentity = new ClaimsIdentity(
                claims,
                CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity), options);
        }
    }
    
}