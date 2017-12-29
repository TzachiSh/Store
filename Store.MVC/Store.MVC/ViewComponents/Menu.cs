using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Store.MVC.WebServiceAccess.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Store.MVC.ViewComponents
{
    public class Menu : ViewComponent
    {
        private readonly IWebApiCalls _webApiCalls;

        public Menu(IWebApiCalls webApiCalls)
        {
            _webApiCalls = webApiCalls;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var cats = await _webApiCalls.GetCategoriesAsync();
            if(cats == null)
            {
                return new ContentViewComponentResult("There was an error getting the categories");
            }
            return View("MenuView", cats);
        }
    }
}
