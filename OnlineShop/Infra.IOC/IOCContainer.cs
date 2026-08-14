using Application.Services.Implementation;
using Application.Services.Interfaces;
using Domain.Interfaces;
using Domain.Model;
using Infra.Data.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Identity.Client.Extensibility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infra.IOC
{
    public static class IOCContainer
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            #region Service Registration

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IEmailSender, EmailSender>();
            services.AddScoped<IProductGroupService, ProductGroupService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IPasswordHasher<Users>, PasswordHasher<Users>>();
            #endregion

            #region Repository Registration
            
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IProductGroupRepository, ProductGroupRepository>();
            services.AddScoped<IProductRepository,ProductRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();

            #endregion


        }
    }
}
