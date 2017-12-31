using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Store.DAL.EF;
using Store.DAL.Initializers;
using Store.DAL.Repos;
using Store.DAL.Repos.Interfaces;
using Store.Models.Entities;
using Store.Service.Filters;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace Store.Service
{
    public class Startup
    {
        private readonly IHostingEnvironment _env;

        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;
            _env = env;
    
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {


            services.AddDbContext<StoreContext>(options =>
            {
                if (_env.IsDevelopment())
                {
                    options.UseSqlServer(Configuration.GetConnectionString("Store"));
                }
                else
                {
                    options.UseSqlServer(Configuration.GetConnectionString("ServerDb"));
                }
                
            });

            // ===== Add Identity ========
            services.AddIdentity<UserEntity, IdentityRole>( o => {
                o.Password.RequireUppercase = false;
                o.Password.RequireNonAlphanumeric = false;
                o.Password.RequiredUniqueChars = 4;
                o.Password.RequireUppercase = false;
                o.Password.RequireDigit = false;
            })
                .AddEntityFrameworkStores<StoreContext>()
                .AddDefaultTokenProviders();
           
            // ===== Add Jwt Authentication ========
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear(); // => remove default claims
            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

                    


                })
                .AddJwtBearer(cfg =>
                {
                    cfg.RequireHttpsMetadata = false;
                    cfg.SaveToken = true;
                    cfg.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidIssuer = Configuration["JwtIssuer"],
                        ValidAudience = Configuration["JwtIssuer"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JwtKey"])),
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.FromMinutes(5)

                    };
                });

            DIContainer(services);

            services.AddAuthorization(cfg =>
            {
                cfg.AddPolicy("isSuperUser", p => p.RequireClaim("isSuperUser", "true"));
            });

       

            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", builder =>
                {
                    builder.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin().AllowCredentials();
                });
                     
            });
            services.AddMvcCore(config =>
            {

                config.Filters.Add(new StoreExceptionFilter(_env.IsDevelopment()));

                //config.Filters.Add(new RequireHttpsAttribute());
                var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
                config.Filters.Add(new AuthorizeFilter(policy));
;

            }
            ).AddJsonFormatters(j => {
                   j.ContractResolver = new DefaultContractResolver();
                   j.Formatting = Formatting.Indented;
               }).AddDataAnnotations().AddAuthorization(o => {

               });
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(ILoggerFactory loggerFactory, IApplicationBuilder app, IHostingEnvironment env, IServiceProvider serviceProvider)
        {

            if (!env.IsDevelopment())
            {
                var options = new RewriteOptions()
                .AddRedirectToHttps();
            }

            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
               

                


                app.UseDeveloperExceptionPage();

                app.UseCors("AllowAll");
            }
            var context = serviceProvider;

            StoreDataInitializer.InitializeData(context);


            app.UseAuthentication();

            app.UseMvc(cfg => {
                cfg.MapRoute("Default",
                "{controller}/{action}/{id?}",
                new { controller = "App", action = "Index" });
            });
        }

        private void DIContainer(IServiceCollection services)
        {
            services.AddScoped<ICategoryRepo, CategoryRepo>();
            services.AddScoped<IProductRepo, ProductRepo>();
            services.AddScoped<ICustomerRepo, CustomerRepo>();
            services.AddScoped<IShoppingCartRepo, ShoppingCartRepo>();
            services.AddScoped<IOrderRepo, OrderRepo>();
            services.AddScoped<IOrderDetailRepo, OrderDetailRepo>();
        }
    }
}
