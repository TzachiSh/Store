using Store.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Store.MVC.Authentication
{
    public interface IAuthHelper
    {
       Customer GetCustomerInfo();
    }
}
