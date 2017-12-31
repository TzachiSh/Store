using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Store.DAL.Repos.Interfaces;
using Store.Models.Entities;

namespace Store.Service.Controllers
{
    [Route("api/[controller]")]
    public class CustomerController : Controller
    {
        private readonly ICustomerRepo _customerRepo;
        private readonly UserManager<UserEntity> _userManager;

        public CustomerController(ICustomerRepo customerRepo, UserManager<UserEntity> userManager)
        {
            _customerRepo = customerRepo;
            _userManager = userManager;
        }



        [HttpGet("{customerId}")]
        public IActionResult GetCustomer(int customerId)
        {
            
            var userId = _userManager.GetUserId(User);
            var customer = _customerRepo.Find(customerId);
            if (customer == null || customer.UserId != userId) return NotFound();

            return Ok(customer);
        }
    }
}