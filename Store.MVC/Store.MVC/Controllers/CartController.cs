using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Store.Models.Entities;
using Store.Models.ViewModels;
using Store.Models.ViewModels.Base;
using Store.MVC.ViewModels;
using Store.MVC.WebServiceAccess.Base;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Store.MVC.Controllers
{
    [Authorize]
    [Route("[controller]/[action]/{customerId}")]
    public class CartController : Controller
    {
        readonly MapperConfiguration _config;
        private readonly IWebApiCalls _webApiCalls;

        public CartController(IWebApiCalls webApiCalls)
        {
            _webApiCalls = webApiCalls;
            _config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CartRecordViewModel, ShoppingCartRecord>();
                cfg.CreateMap<CartRecordWithProductInfo, CartRecordViewModel>();
                cfg.CreateMap<ProductAndCategoryBase, AddToCartViewModel>();
            });
        }

        // /cart/index/customerid
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            ViewBag.Title = "Cart";
            ViewBag.Header = "Cart";
            // var customer = await _webApiCalls.GetCustomerAsync(customerId);
            try
            {
                int customerId  = ViewBag.CustomerId;
                var cartItems = await _webApiCalls.GetCartAsync(customerId);
                var customer = await _webApiCalls.GetCustomerAsync(customerId);
                var mapper = _config.CreateMapper();
                var viewModel = new CartViewModel
                {
                    Customer = customer,
                    CartRecords = mapper.Map<IList<CartRecordViewModel>>(cartItems)
                };
                ViewBag.Isempty = false;
                return View(viewModel);

            }
            catch (Exception ex)
            {
                throw ex;
                Console.WriteLine(ex);
                ViewBag.Isempty = true;
                return View();
            }



        }

        // /cart/index/customerId/productId

        [Route("~/[controller]/[action]/{productId}")]
        [AllowAnonymous]
        public async Task<IActionResult> AddToCart(int productId,
                                                   bool cameFromProducts = false)
        {
            ViewBag.CameFromProducts = cameFromProducts;
            ViewBag.Title = "Add to Cart";
            ViewBag.Header = "Add to Cart";
            ViewBag.ShowCategory = true;
            var prod = await _webApiCalls.GetOneProductAsync(productId);
            if (prod == null) return NotFound();
            var mapper = _config.CreateMapper();
            var cartRecord = mapper.Map<AddToCartViewModel>(prod);
            cartRecord.Quantity = 1;
            return View(cartRecord);
        }
        // cart/addToCart/customerId/productId
        
        [ActionName("AddToCart"), HttpPost("{productId}"), ValidateAntiForgeryToken]
        public async Task<IActionResult> AddToCartPost(int productId,
                                                       AddToCartViewModel item)
        {
            int customerId = ViewBag.CustomerId;

            if (!ModelState.IsValid) return View(item);
            try
            {
                await _webApiCalls.AddToCartAsync(customerId, productId, item.Quantity);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "There was an error adding the item to the cart.");
                return View(item);
            }
            return RedirectToAction(nameof(CartController.Index), new { customerId });
        }
        // /cart/customerId/cartId
        [HttpPost("{id}"), ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int customerId, int id,
                                                string timeStampString,
                                                CartRecordViewModel item)
        {
            item.TimeStamp = JsonConvert.DeserializeObject<byte[]>($"\"{timeStampString}\"");
            if (!ModelState.IsValid) return PartialView(item);
            var mapper = _config.CreateMapper();
            var newItem = mapper.Map<ShoppingCartRecord>(item);
            try
            {
                await _webApiCalls.UpdateCartItemAsync(newItem);
                var updatedItem = await _webApiCalls.GetCartRecordAsync(customerId, item.ProductId);
                var newViewModel = mapper.Map<CartRecordViewModel>(updatedItem);
                return PartialView(newViewModel);
            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty,
                "An error occurred updating the cart. Please reload the page and try again.");
                return PartialView(item);
            }
        }
        // /cart/customerId/cartId
        [HttpPost("{id}"), ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int customerId, int id, ShoppingCartRecord item)
        {
            await _webApiCalls.RemoveCartItemAsync(customerId, id, item.TimeStamp);
            return RedirectToAction(nameof(Index), new { customerId });
        }
        ///cart/customerId/buy
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Buy(int customerId, Customer customer)
        {
            int orderId = await _webApiCalls.PurchaseCartAsync(customer);
            return RedirectToAction(
            nameof(OrdersController.Details),
            nameof(OrdersController).Replace("Controller", ""),
            new { customerId, orderId });
        }
    }
}