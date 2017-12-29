using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Store.DAL.Repos.Interfaces;

namespace Store.Service.Controllers
{

    [Route("api/[controller]"), AllowAnonymous]
    public class SearchController : Controller
    {
        private readonly IProductRepo _productRepo;

        public SearchController(IProductRepo productRepo)
        {
            _productRepo = productRepo;
        }
        [HttpGet("{searchString}", Name = "SearchProducts" )]
        public IActionResult Search(string searchString)
        {
            return Ok(_productRepo.Search(searchString));
        }
    }
}