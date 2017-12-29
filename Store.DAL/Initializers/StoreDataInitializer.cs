using System;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Store.DAL.EF;
using Store.Models.Entities;

namespace Store.DAL.Initializers
{
    public static class StoreDataInitializer
    {
        public static void InitializeData(IServiceProvider serviceProvider)
        {
            
            var context = serviceProvider.GetService<StoreContext>();
            var userManager = serviceProvider.GetService<UserManager<UserEntity>>();
            InitializeData(context, userManager);

        }
        public static void InitializeData(StoreContext context, UserManager<UserEntity> userManager)
        {
            context.Database.Migrate();
            ClearData(context);
            SeedData(context, userManager);
        }
        public static void ClearData(StoreContext context)
        {
            ExecuteDeleteSQL(context, "Categories");
            ExecuteDeleteSQL(context, "Customers");
            ExecuteDeleteSQL(context, "AspNetUsers");
            
            ResetIdentity(context);
        }
        public static void ExecuteDeleteSQL(StoreContext context, string tableName)
        {
            var table = tableName == "AspNetUsers" ? "dbo" : "Store";
            var sql = $"Delete from {table}.{tableName}";
            context.Database.ExecuteSqlCommand(sql);
        }
        public static void ResetIdentity(StoreContext context)
        {
            var tables = new[] {"Categories","Customers",
                "OrderDetails","Orders","Products","ShoppingCartRecords"};
            foreach (var itm in tables)
            {
                var sql = $"DBCC CHECKIDENT (\"Store.{itm}\", RESEED, -1);";
                context.Database.ExecuteSqlCommand(sql);
            }
        }

        public static void SeedData(StoreContext context,UserManager<UserEntity> userManager)
        {
            try
            {
                if (!context.Categories.Any())
                {
                    context.Categories.AddRange(StoreSampleData.GetCategories());
                    context.SaveChanges();
                }
                if (!context.Products.Any())
                {
                    context.Products.AddRange(
                        StoreSampleData.GetProducts(context.Categories.ToList()));
                    context.SaveChanges();
                }
                 if (!context.Users.Any() && !context.Customers.Any())
                {
                    var user = StoreSampleData.GetUserRecords(context);
                    var result = userManager.CreateAsync(user, "Password1!");

                }  
                var customer = context.Customers.FirstOrDefault();
                if (!context.Orders.Any())
                {
                    context.Orders.AddRange(StoreSampleData.GetOrders(customer, context));
                    context.SaveChanges();
                }
                if (!context.ShoppingCartRecords.Any())
                {
                    context.ShoppingCartRecords.AddRange(
                        StoreSampleData.GetCart(customer, context));
                    context.SaveChanges();
                }
             
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
