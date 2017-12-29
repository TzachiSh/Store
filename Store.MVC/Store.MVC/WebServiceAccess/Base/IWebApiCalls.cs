using Store.Models.Entities;
using Store.Models.ViewModels;
using Store.Models.ViewModels.Base;
using Store.MVC.ViewModels;
using Store.Service.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Store.MVC.WebServiceAccess.Base
{
    public interface IWebApiCalls
    {
        Task<IList<Category>> GetCategoriesAsync();
        Task<Category> GetCategoryAsync(int id);
        Task<IList<ProductAndCategoryBase>> GetProductsForACategoryAsync(int categoryId);
        Task<IList<Customer>> GetCustomersAsync();
        Task<Customer> GetCustomerAsync(int id);
        Task<IList<Order>> GetOrdersAsync(int customerId);
        Task<OrderWithDetailsAndProductInfo> GetOrderDetailsAsync(int customerId, int orderId);
        Task<ProductAndCategoryBase> GetOneProductAsync(int productId);
        Task<IList<ProductAndCategoryBase>> GetFeaturedProductsAsync();
        Task<IList<ProductAndCategoryBase>> SearchAsync(string searchTerm);
        Task<IList<CartRecordWithProductInfo>> GetCartAsync(int customerId);
        Task<CartRecordWithProductInfo> GetCartRecordAsync(int customerId, int productId);
        Task<string> AddToCartAsync(int customerId, int productId, int quantity);
        Task<string> UpdateCartItemAsync(ShoppingCartRecord item);
        Task RemoveCartItemAsync(int customerId, int shoppingCartRecordId, byte[] timeStamp);
        Task<int> PurchaseCartAsync(Customer customer);
        Task<IList<ProductAndCategoryBase>> GetProductsAsync();
        Task<CookieViewModel> LoginAsync(LoginViewModel loginViewModel);
        Task<string> RegisterAsync(RegisterViewModel model);
    }
}
