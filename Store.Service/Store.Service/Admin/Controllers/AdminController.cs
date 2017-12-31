using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Store.DAL.Repos.Base;
using Store.DAL.Repos.Interfaces;
using Store.Models.Entities;
using Store.Models.ViewModels.Base;

namespace Store.Service.Admin.Controllers
{
    
    [Route("api/[controller]/[action]")]
    public class AdminController : Controller
    {
        private readonly IProductRepo _productRepo;
        private readonly ICategoryRepo _categoryRepo;
        private readonly IOrderRepo _orderRepo;

        public AdminController(IProductRepo productRepo, ICategoryRepo categoryRepo, IOrderRepo orderRepo )
        {
            _productRepo = productRepo;
            _categoryRepo = categoryRepo;
            _orderRepo = orderRepo;
        }
        [HttpPost()]
        public IActionResult Product([FromBody]Product product)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var x = _productRepo.Add(product);

            return Ok(product);     

        }
        [HttpPost()]
        public IActionResult Category([FromBody]Category category)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var x = _categoryRepo.Add(category);

            return Ok(category);
        }

        [HttpGet()]
        public IActionResult Orders()
        {
           var orders = _orderRepo.GetAll().OrderBy(o => o.OrderDate).ToList();
            if (!orders.Any()) return NotFound();

            return Ok(orders);
        }

    }
}