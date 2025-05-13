using Ecom.Core.interfaces;
using Ecom.Core.Services;
using Ecom.Infrastructure.Data;
using Ecom.Infrastructure.Repositories;
using Ecom.Infrastructure.Repositories.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using StackExchange.Redis;
using System;
using System.IO;

namespace Ecom.Infrastructure
{
    public static class InfrastructureRegistration
    {
        public static IServiceCollection InfrastructureConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IUnitOfWork, UnitOfWork>();


            //applay redis connection
            services.AddSingleton<IConnectionMultiplexer>(i =>
            {
                var config = ConfigurationOptions.Parse(configuration.GetConnectionString("redis"));
                return ConnectionMultiplexer.Connect(config);
            });
            services.AddSingleton<IManagementServices, ManagementServices>();

            // Register IFileProvider
            //services.AddSingleton<IFileProvider>(
            //    new PhysicalFileProvider(Directory.GetCurrentDirectory())
            //);

            services.AddSingleton<IFileProvider>(
               new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot"))
           );

            // Apply DbContext
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("EcomDatabase"));
            });

            return services;
        }
    }
}
