using System.Collections.Generic;
using Store.DAL.Repos.Base;
using Store.Models.Entities;

namespace Store.DAL.Repos.Interfaces
{
    public interface ICategoryRepo : IRepo<Category>
    {
        IEnumerable<Category> GetAllWithProducts();
        Category GetOneWithProducts(int? id);
    }
}