using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Store.MVC.Configuration
{
    public class WebServiceLocator : IWebServiceLocator
    {
        private readonly IHostingEnvironment _hostingEnvironment;

        public WebServiceLocator(IConfigurationRoot config, IHostingEnvironment hostingEnvironment)
        {



            _hostingEnvironment = hostingEnvironment;

            var customSection = config.GetSection(nameof(WebServiceLocator));
            if (_hostingEnvironment.IsDevelopment())
            {

                ServiceAddress = customSection?.GetSection(nameof(LocalAddress))?.Value;               
            }
            else
            {
                ServiceAddress = customSection?.GetSection(nameof(ServiceAddress))?.Value;

            }
           

          
            
        }

        public string ServiceAddress { get; }
        public object LocalAddress { get; }
    }
}
