using System.Text.Json;
using Domain.Entities;
using Infrastructure.Configurations;


public class CountryImporter
{
    private readonly ApplicationDbContext _context;

    public CountryImporter(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task ImportCountriesAsync(string jsonFilePath)
    {
        var json = await File.ReadAllTextAsync(jsonFilePath);
        var countries = JsonSerializer.Deserialize<List<CountryJson>>(json);

        var userLocations = countries.Select(c => new Location
        {
            Id = c.id,
            Name = c.native
        }).ToList();

        _context.Locations.AddRange(userLocations);
        await _context.SaveChangesAsync();
    }
}

