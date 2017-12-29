using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Store.DAL.Repos.Interfaces;

namespace Store.Service.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    public class ProductController : Controller
    {
        private readonly IProductRepo _productRepo;

        public ProductController(IProductRepo productRepo)
        {
            _productRepo = productRepo;
        }
        public IActionResult GetProducts()
        {           

            return Ok(_productRepo.GetAllWithCategoryName());
        }
        [HttpGet("{productId}")]
        public IActionResult GetProduct(int productId)
        {
            var product = _productRepo.GetOneWithCategoryName(productId);

            
            return product == null ? (IActionResult)NotFound() : Ok(product);
        }
        [HttpGet("featured")]
        public IActionResult GetFetured()
        {
            return Ok(_productRepo.GetFeaturedWithCategoryName());
        }

    }
}