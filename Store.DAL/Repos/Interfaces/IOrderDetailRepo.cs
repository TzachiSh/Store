using System.Collections.Generic;
using Store.DAL.Repos.Base;
using Store.Models.Entities;
using Store.Models.ViewModels;

namespace Store.DAL.Repos.Interfaces
{
    public interface IOrderDetailRepo :IRepo<OrderDetail>
    {
        IEnumerable<OrderDetailWithProductInfo> GetCustomersOrdersWithDetails(int customerId);
        IEnumerable<OrderDetailWithProductInfo> GetSingleOrderWithDetails(int orderId);
    }
}
