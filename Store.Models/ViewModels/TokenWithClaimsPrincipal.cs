using Microsoft.AspNetCore.Authentication;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Store.Service.Model
{
    public class CookieViewModel
    {

        public string Token { get; set; }

        public DateTime Expires { get; set; }

        public ClaimsPrincipal ClaimsPrincipal { get; set; }

        public AuthenticationProperties AuthProp { get; set; }

    }
}
