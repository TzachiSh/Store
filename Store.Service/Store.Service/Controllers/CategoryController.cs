using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Store.DAL.Repos.Interfaces;

namespace Store.Service.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    public class CategoryController : Controller
    {
        public ICategoryRepo Repo { get; set; }
        public IProductRepo ProductRepo { get; set; }

        public CategoryController(ICategoryRepo repo, IProductRepo productRepo)
        {
            Repo = repo;
            ProductRepo = productRepo;
        }

        [HttpGet]
        public IActionResult GetCategories()
        {
            return Ok(Repo.GetAll());
        }

        [HttpGet("{id}")]
        public IActionResult GetCategory(int id)
        {
            var item = Repo.Find(id);

            if (item == null)
            {
                NotFound();
            }
         
            return Ok(item);
        }
        [HttpGet("{categoryid}/products")]
        public IActionResult GetProductForCategory(int categoryid) 
        {
            return Ok(ProductRepo.GetProductsForCategory(categoryid));
        }
    }
}