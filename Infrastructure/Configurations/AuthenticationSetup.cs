using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Configurations;

public static class AuthenticationSetup
{
    //para las cookies seguras 
    public static IServiceCollection AddAuthenticationConfig(this IServiceCollection services)
    {
        services.ConfigureApplicationCookie(options =>
        {
            options.Cookie.HttpOnly = true;
            options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            options.Cookie.SameSite = SameSiteMode.Strict;
            options.Cookie.Name = "MyAuthCookie";
            options.LoginPath = "/Account/Login";
            options.LogoutPath = "/Account/Logout";
            options.ExpireTimeSpan = TimeSpan.FromMinutes(15);
            options.SlidingExpiration = true;
        });

        return services;
    }
}

