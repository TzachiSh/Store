using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Store.MVC.ViewModels
{
    public class RegisterViewModel
    {
        [DataType(DataType.EmailAddress), Required, Display(Name = "EmailAddress")]
        public string EmailAddress { get; set; }

        [DataType(DataType.Text), Required, MinLength(4), Display(Name = "FullName")]
        public string FullName { get; set; }

        [DataType(DataType.Password), Required, MinLength(4), Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password), Required, MinLength(4), Display(Name = "ConfrimPassword")]
        public string ConfrimPassword { get; set; }
    }
}
