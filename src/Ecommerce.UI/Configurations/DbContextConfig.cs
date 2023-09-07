using Ecommerce.DAL.Context;
using Ecommerce.UI.Data;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.UI.Configurations
{
    public static class DbContextConfig
    {
        public static IServiceCollection AddContextsConfig(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(connectionString));

            services.AddDbContext<EcommerceDBContext>(options =>
                options.UseSqlServer(connectionString));

            services.AddDatabaseDeveloperPageExceptionFilter();

            return services;
        }
    }
}
