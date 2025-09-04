using AutoMapper;
using Infrastructure.Configurations;
using Serilog;

try
{
    var builder = WebApplication.CreateBuilder(args);

    // Agregar servicios a la aplicación
    builder.Services.AddSerilogServices(builder.Configuration); // Agregar Serilog
    builder.Services.AddPersistenceServices(builder.Configuration);
    builder.Services.AddApplicationServices();
    builder.Services.AddAuthenticationServices(builder.Configuration);
    builder.Services.AddInfrastructureServices(builder.Configuration);
    builder.Services.AddAuthenticationConfig();

    builder.Services.AddStackExchangeRedisCache(options =>
    {
        options.Configuration = "localhost:6379"; // dirección de tu servidor Redis
        options.InstanceName = "BootCamp";
    });

    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();

    var app = builder.Build();


    // Configuración del pipeline HTTP
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();

        using (var scope = app.Services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var mapper = scope.ServiceProvider.GetRequiredService<IMapper>();
            var basePath = AppContext.BaseDirectory;

            if (!context.Locations.Any())
            {
                var jsonPath = Path.Combine(basePath, "Data", "countries.json");
                var importer = new CountryImporter(context);
                await importer.ImportCountriesAsync(jsonPath);
            }
        }
    }

    app.UseCors("AllowAll");
    app.UseStaticFiles();

    app.UseHttpsRedirection();

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();

    app.Run();

}
catch (Exception ex)
{
    Log.Fatal(ex, "La aplicación falló al iniciar");
}
finally
{
    Log.CloseAndFlush();
}