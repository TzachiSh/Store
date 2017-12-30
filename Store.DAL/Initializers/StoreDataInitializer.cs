using System;
using System.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Store.DAL.EF;
using Store.Models.Entities;
using System.Collections.Generic;

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
            ClearData(context, userManager);
            SeedData(context, userManager);
        }
        public static void ClearData(StoreContext context, UserManager<UserEntity> userManager)
        {

            ExecuteDeleteSQL(context, "Categories" , "Store");
            ExecuteDeleteSQL(context, "Customers", "Store");
            ExecuteDeleteSQL(context, "AspNetUsers", "dbo");
            ExecuteDeleteSQL(context, "AspNetUserClaims", "dbo");

            ResetIdentity(context, userManager);
        }
        public static void ExecuteDeleteSQL(StoreContext context, string tableName , string table)
        {
            var sql = $"Delete from {table}.{tableName}";
            context.Database.ExecuteSqlCommand(sql);
        }
        public static void ResetIdentity(StoreContext context, UserManager<UserEntity> userManager)
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
                    var newUser = StoreSampleData.GetUserRecords(context);
                    var result = userManager.CreateAsync(newUser, "Password");

                }
                var user = userManager.Users
                                      .OrderBy(u => u.Id)
                                      .Include(u => u.Customer)
                                      .FirstOrDefault();

                var customer = user.Customer;


                if (!context.ShoppingCartRecords.Any())
                {
                    context.ShoppingCartRecords.AddRange(
                        StoreSampleData.GetCart(customer, context));
                    context.SaveChanges();
                }

                user = userManager.Users.SingleOrDefault(r => r.Email == user.Email);


                userManager.AddClaimsAsync(user, AddClaimsToUser(user));

            }

            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
        private static List<Claim> AddClaimsToUser(UserEntity user)
        {

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier,user.Id),
                new Claim(ClaimTypes.Name, user.Customer.FullName),
                new Claim(ClaimTypes.Authentication, user.Customer.Id.ToString()),
                new Claim("IsSuperUser","true")
            };

            return claims;

        }
    }
}
