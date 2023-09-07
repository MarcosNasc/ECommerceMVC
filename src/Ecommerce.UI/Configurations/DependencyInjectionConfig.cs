using Ecommerce.BLL.Interfaces;
using Ecommerce.BLL.Interfaces.Repositories;
using Ecommerce.BLL.Interfaces.Services;
using Ecommerce.BLL.Notifications;
using Ecommerce.BLL.Services;
using Ecommerce.DAL.Repository;
using Ecommerce.UI.Extensions.Attribute;
using Microsoft.AspNetCore.Mvc.DataAnnotations;

namespace Ecommerce.UI.Configurations
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection ResolveDependencies(this IServiceCollection services)
        {
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<ISupplierRepository, SupplierRepository>();
            services.AddScoped<IAddressRepository, AddressRepository>();
            services.AddSingleton<IValidationAttributeAdapterProvider, MoneyValidateAttributeAdapterProvider>();

            services.AddScoped<INotificator, Notificator>();
            services.AddScoped<ISupplierService, SupplierService>();
            services.AddScoped<IProductService, ProductService>();

            return services;
        }
    }
}
