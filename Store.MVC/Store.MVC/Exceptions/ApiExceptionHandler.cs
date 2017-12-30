using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Store.MVC.Exceptions
{
    public class ApiException : Exception
    {
        public HttpResponseMessage Response { get; set; }
        public ApiException(HttpResponseMessage response)
        {
            this.Response = response;
        }


        public HttpStatusCode StatusCode
        {
            get
            {
                return this.Response.StatusCode;
            }
        }


        public IEnumerable<string> Errors
        {
            get
            {
                return Data.Values.Cast<string>().ToList();
            }
        }
    }
}
