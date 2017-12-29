using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Store.Models.ViewModels.Base;
using Store.MVC.WebServiceAccess.Base;

namespace Store.MVC.Controllers
{
   
    public class ProductsController : Controller
    {
        private readonly IWebApiCalls _webApiCalls;

        public ProductsController(IWebApiCalls webApiCalls)
        {
            _webApiCalls = webApiCalls;
        }
       [HttpGet]
       public IActionResult Error()
        {
            return View();
        }
        public async Task <IActionResult> Index()
        {
            
            ViewBag.Title = "Products";
            ViewBag.Header = "Products";
            ViewBag.ShowCategory = true;           
            return await GetListOfProducts(featured: false);
        }
        public ActionResult Details(int id)
        {
            return RedirectToAction(
            nameof(CartController.AddToCart),
            nameof(CartController).Replace("Controller", ""),
            new { customerId = ViewBag.CustomerId, productId = id, cameFromProducts = true });
        }
        internal async Task<IActionResult> GetListOfProducts(
           int id = -1, bool featured = false, string searchString = "")
        {
            IList<ProductAndCategoryBase> prods;
            if (featured)
            {
                prods = await _webApiCalls.GetFeaturedProductsAsync();
            }
            else if (!string.IsNullOrEmpty(searchString))
            {
                prods = await _webApiCalls.SearchAsync(searchString);
            }
            else if(id != -1)
            {
                prods = await _webApiCalls.GetProductsForACategoryAsync(id);
            }
            else
            {
                prods = await _webApiCalls.GetProductsAsync();
            }
            if (prods == null)
            {
                return NotFound();
            }
            return View("ProductList", prods);
        }

        [HttpGet]
        public async Task<IActionResult> Featured()
        {
            ViewBag.Title = "Featured Products";
            ViewBag.Header = "Featured Products";
            ViewBag.ShowCategory = true;
            ViewBag.Featured = true;
            return await GetListOfProducts(featured: true);
        }
        [HttpGet]
        public async Task<IActionResult> ProductList(int id)
        {
            var cat = await _webApiCalls.GetCategoryAsync(id);
            ViewBag.Title = cat?.CategoryName;
            ViewBag.Header = cat?.CategoryName;
            ViewBag.ShowCategory = false;
            ViewBag.Featured = false;
            return await GetListOfProducts(id);
        }
        [Route("[controller]/[action]")]
        [HttpPost("{searchString}")]
        public async Task<IActionResult> Search(string searchString)
        {
            ViewBag.Title = "Search Results";
            ViewBag.Header = "Search Results";
            ViewBag.ShowCategory = true;
            ViewBag.Featured = false;
            return await GetListOfProducts(searchString: searchString);
        }
    }
}