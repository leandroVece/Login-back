using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Configurations;

public class DataContextDesignTimeFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();

        string currentDirectory = Directory.GetCurrentDirectory();
        string basePath = Path.Combine(currentDirectory, "..", "Presentation");

        IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(basePath)
            .AddJsonFile("appsettings.json")
            .Build();

        string connectionString = configuration.GetConnectionString("Mysql");

        optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));

        return new ApplicationDbContext(optionsBuilder.Options);
    }
}