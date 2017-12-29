using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Store.DAL.Repos.Interfaces;
using Store.Models.Entities;


namespace Store.Service.Controllers
{

    [Route("api/[controller]/{customerId}")]
    public class OrdersController : Controller
    {
        private readonly IOrderRepo _orderRepo;

        public OrdersController(IOrderRepo orderRepo)
        {
            _orderRepo = orderRepo;
        }

        public IActionResult GetOrderHistory(int customerId)
        {
            if (Int32.Parse(User.FindFirst(ClaimTypes.Authentication).Value) != customerId) return NotFound();

            var orders = _orderRepo.GetOrderHistory(customerId);

            return (!orders.Any()) ? (IActionResult)NotFound() : Ok(orders) ;
        }
        [HttpGet("{orderId}", Name = "GetOrderDetails")]
        public IActionResult GetOrderForCustomer(int customerId , int orderId) {

            var order = _orderRepo.GetOneWithDetails(customerId, orderId);

            return (order == null) ? (IActionResult)NotFound() : Ok(order);
        }
    }
}