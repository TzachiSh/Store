using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Store.Models.Entities;
using Store.Models.ViewModels;
using Store.MVC.WebServiceAccess.Base;

namespace Store.MVC.Controllers
{
    [Authorize]
    [Route("[controller]/[action]/{customerId}")]
    public class OrdersController : Controller
    {
        private readonly IWebApiCalls _webApiCalls;
        public OrdersController(IWebApiCalls webApiCalls)
        {
            _webApiCalls = webApiCalls;
        }
        // index/customerId
        [HttpGet]
        public async Task<IActionResult> Index(int customerId)
        {
            ViewBag.Title = "Order History";
            ViewBag.Header = "Order History";
            IList<Order> orders = await _webApiCalls.GetOrdersAsync(customerId);
            if (orders == null) return NotFound();
            return View(orders);
        }
        // details/customerId/OrderId
        [HttpGet("{orderId}")]
        public async Task<IActionResult> Details(int customerId, int orderId)
        {
            ViewBag.Title = "Order Details";
            ViewBag.Header = "Order Details";
            OrderWithDetailsAndProductInfo orderDetails =
            await _webApiCalls.GetOrderDetailsAsync(customerId, orderId);
            if (orderDetails == null) return NotFound();
            return View(orderDetails);
        }
    }
}