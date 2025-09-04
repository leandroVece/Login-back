using Application.ingoing;
using Application.Interface;
using Application.outgoing;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Configurations;

public static class PersistenceServiceExtensions
{
    public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Configuración de la base de datos
        var connectionString = configuration.GetConnectionString("Mysql");
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

        // Registro de repositorios
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUserTokenManager, UserTokenManager>();
        services.AddScoped<IUserRepository, UserRepository>();


        return services;
    }
}