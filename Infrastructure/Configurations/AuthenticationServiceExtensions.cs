using System.Text;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Configurations;

public static class AuthenticationServiceExtensions
{
    public static IServiceCollection AddAuthenticationServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Configuración de Identity
        services.AddIdentity<ApplicationUser, ApplicationRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

        // Configuración de autenticación JWT
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.SaveToken = true;
            options.RequireHttpsMetadata = false;
            options.Events = new JwtBearerEvents
            {
                OnMessageReceived = context =>
                {
                    if (context.Request.Cookies.ContainsKey("MyAuthCookie"))
                    {
                        context.Token = context.Request.Cookies["MyAuthCookie"];
                    }
                    return Task.CompletedTask;
                }
            };

            options.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidAudience = configuration["JWT:ValidAudience"],
                ValidIssuer = configuration["JWT:ValidIssuer"],
                ClockSkew = TimeSpan.Zero,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]))
            };
        });

        // Configuración adicional de Identity
        services.Configure<IdentityOptions>(opt => opt.SignIn.RequireConfirmedEmail = true);
        services.Configure<DataProtectionTokenProviderOptions>(op => op.TokenLifespan = TimeSpan.FromHours(10));

        return services;
    }
}