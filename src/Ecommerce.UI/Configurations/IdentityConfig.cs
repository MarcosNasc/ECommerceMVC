using Ecommerce.UI.Data;
using Microsoft.AspNetCore.Identity;

namespace Ecommerce.UI.Configurations
{
    public static class IdentityExtensions
    {
        public static IServiceCollection AddIdentityConfig(this IServiceCollection services)
        {
            services.AddDefaultIdentity<IdentityUser>(options =>
                 {
                     options.SignIn.RequireConfirmedAccount = false;
                 })
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Account/Login"; 
                //options.AccessDeniedPath = "/Account/AccessDenied";
            });

            return services;
        }
    }
}
