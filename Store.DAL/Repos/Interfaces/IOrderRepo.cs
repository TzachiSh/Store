using System.Collections.Generic;
using Store.DAL.Repos.Base;
using Store.Models.Entities;
using Store.Models.ViewModels;

namespace Store.DAL.Repos.Interfaces
{
    public interface IOrderRepo :IRepo<Order>
    {
        IEnumerable<Order> GetOrderHistory(int customerId);
        OrderWithDetailsAndProductInfo GetOneWithDetails(int customerId, int orderId);
    }
}
