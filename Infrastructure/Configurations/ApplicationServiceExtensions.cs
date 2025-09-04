
using Application.ingoing;
using Application.Interface;
using Application.Mapper;
using Application.UseCases;
using Infrastructure.Persistence;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Configurations;

public static class ApplicationServiceExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // Configuración de AutoMapper
        services.AddAutoMapper(typeof(InfrastructureProfile));

        services.AddScoped<ILoggerServices, SerilogLoggerServices>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IEmailUseCases, EmailUseCases>();
        services.AddScoped<IUserUseCases, UserUseCases>();

        return services;
    }
}