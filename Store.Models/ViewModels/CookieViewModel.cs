using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Store.Service.Model
{
    public class CookieViewModel
    {

        public string Token { get; set; }

        public int CustomerId { get; set; }

        public string CustomerName { get; set; }

        public DateTime Expires { get; set; }

    }
}
