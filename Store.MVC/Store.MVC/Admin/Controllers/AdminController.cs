using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Store.Models.ViewModels.Base;
using Store.MVC.WebServiceAccess.Base;

namespace Store.MVC.Admin.Controllers
{
    [Authorize(Policy = "IsSuperUser")]
    public class AdminController : Controller
    {
        private readonly IWebApiCalls _apiCalls;

        public AdminController(IWebApiCalls apiCalls)
        {
            _apiCalls = apiCalls;
        }
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet()]
        public IActionResult Product()
        {

            return View();

        }
        [ValidateAntiForgeryToken]
        [HttpPost()]
        public async Task<IActionResult> Product(ProductAndCategoryBase product)
        {
            if (!ModelState.IsValid) return View(product);

            await _apiCalls.CreateProduct(product);


            return Ok();



        }

        // GET: Admin/Details/5
        public ActionResult Details(int id)
        {


            return View();
        }

        // GET: Admin/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Admin/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Admin/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Admin/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Admin/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}