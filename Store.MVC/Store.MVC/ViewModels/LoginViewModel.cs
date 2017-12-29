using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Store.MVC.ViewModels
{
    public class LoginViewModel
    {
        [DataType(DataType.EmailAddress) , Required , MinLength(3), Display(Name = "Email")]
        public string Email { get; set; }
        [DataType(DataType.Password), Required, MinLength(3), Display(Name = "Password")]
        public string Password { get; set; }
    }
}
