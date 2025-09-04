using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Json;

namespace Infrastructure.Configurations;

public static class SerilogConfiguration
{
    public static IServiceCollection AddSerilogServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Configurar Serilog
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .WriteTo.File(
                new JsonFormatter(),
                path: "logs/log-.json",
                rollingInterval: RollingInterval.Day,
                retainedFileCountLimit: 7,  // Mantener archivos de una semana
                fileSizeLimitBytes: 10 * 1024 * 1024, // 10 MB por archivo
                rollOnFileSizeLimit: true,
                shared: true)
            .CreateLogger();

        // Agregar Serilog como proveedor de logging
        services.AddLogging(loggingBuilder =>
        {
            loggingBuilder.ClearProviders();
            loggingBuilder.AddSerilog(dispose: true);
        });

        return services;
    }
}