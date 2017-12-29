using Store.DAL.Repos.Base;
using Store.Models.Entities;
using System;

namespace Store.DAL.Repos.Interfaces
{
    public interface ICustomerRepo : IRepo<Customer>
    {
        Customer FindByUserId(string userId);
    }
}
