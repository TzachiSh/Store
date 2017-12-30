using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Store.MVC.ViewModels
{
    public class RegisterViewModel
    {
        [DataType(DataType.EmailAddress), Required, Display(Name = "Email Address")]
        public string EmailAddress { get; set; }

        [DataType(DataType.Text), Required, MinLength(4), Display(Name = "Full Name")]
        public string FullName { get; set; }
        
        [DataType(DataType.Password), Required, MinLength(8), Display(Name = "Password")]
        public string Password { get; set; }

        [CompareAttribute("Password", ErrorMessage = "Password doesn't match.")]
        [DataType(DataType.Password), Display(Name = "Confrim Password")]
        public string ConfrimPassword { get; set; }
    }
}
