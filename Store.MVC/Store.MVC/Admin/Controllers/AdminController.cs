using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Store.Models.Entities;
using Store.Models.ViewModels.Base;
using Store.MVC.WebServiceAccess.Base;

namespace Store.MVC.Admin.Controllers
{
    [Authorize(Policy = "IsSuperUser")]
    [Route("[controller]/[action]")]
    public class AdminController : Controller
    {
        private readonly IWebApiCalls _apiCalls;
        


        public AdminController(IWebApiCalls apiCalls)
        {
            _apiCalls = apiCalls;
        }
        // GET: Admin
        public async Task <ActionResult> Index()
        {
            try
            {
                IList<Order> orders = await _apiCalls.GetAllOrdersAsync();
                ViewBag.Header = "Orders";

                return View("orders", orders);
            }
            catch(Exception)
            {                
                return View("orders");
            }

            


           
            
        }
  
        [HttpGet()]
        public IActionResult Product()
        {

            return View();

        }
        [ValidateAntiForgeryToken]
        [HttpPost()]
        public async Task<IActionResult> Product(Product product)
        {

            try
            {
                await _apiCalls.CreateProduct(product);

                return Ok(product);

            }
            catch (Exception)
            {

                ModelState.AddModelError(string.Empty, "There was an error adding new product");
                return View();
            }

            


        }
        [HttpGet()]
        public IActionResult Category()
        {

            return View();

        }
        [ValidateAntiForgeryToken]
        [HttpPost()]
        public async Task<IActionResult> Category(Category category)
        {
            if (!ModelState.IsValid) return View(category);

            await _apiCalls.CreateCategory(category);


            return Ok();


            
        }
        //[HttpGet()]
        //public async Task <IActionResult> Orders()
        //{
        //    IList<Order> orders = await _apiCalls.GetAllOrdersAsync();

        //    return View(orders.ToList());



        //}

    }
}