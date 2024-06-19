using Microsoft.AspNetCore.Identity;

namespace OutFitMaker.API.Utilities
{
    public static class ConfigIdentity
    {
        public static IServiceCollection ConfigIdentityOptions(this IServiceCollection services)
        {
            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
                options.User.RequireUniqueEmail = false;
                //options.SignIn.RequireConfirmedEmail = true;
            });
            return services;
        }
    }
}
