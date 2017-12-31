using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Store.MVC.Authentication;
using System;
using System.Net;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading;

namespace Store.MVC.Filters
{
    public class AuthActionFilter : IActionFilter
    {
        private IAuthHelper _authHelper;


        public AuthActionFilter(IAuthHelper authHelper)
        {
            _authHelper = authHelper;

        }
        public void OnActionExecuting(
            ActionExecutingContext context)
        {
            var viewBag = ((Controller)context.Controller).ViewBag;

            if (context.HttpContext.User.Identity.IsAuthenticated)
            {
                var token = context.HttpContext.User.FindFirst("AcessToken").Value;



                string[] rolesArray = { "managers", token };


                Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(
                    token, "token"), rolesArray);

                viewBag.CustomerName = context.HttpContext.User.FindFirst(ClaimTypes.Name).Value;
                viewBag.CustomerId = int.Parse(context.HttpContext.User.FindFirst(ClaimTypes.Authentication).Value);
            }
            else
            {
                viewBag.CustomerName = "Anonymous";
                viewBag.CustomerId = 0;
                
            }
        
            





            //var authHelper = (IAuthHelper)context.HttpContext.RequestServices.GetService(typeof(IAuthHelper));
            //var customer = _authHelper.GetCustomerInfo();


        }

        public void OnActionExecuted(ActionExecutedContext context)
        {

            if (context.Exception is UnauthorizedAccessException)
            {
               context.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                context.ExceptionHandled = true;                

            }

        }
    }
}




