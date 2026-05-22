// Spuštění: dotnet scripts/import-ice-rinks/import-ice-rinks.cs [-- cesta/k/IceRink.html]
// Výchozí cesta k HTML: src/DB Model/Migration/IceRink.html (relativně k working directory)
//
// Konfigurace (user secrets nebo env vars):
//   dotnet user-secrets set "ConnectionStrings:DefaultConnection" "..." --file scripts/import-ice-rinks/import-ice-rinks.cs
//   dotnet user-secrets set "MapyCom:ApiKey" "..."                      --file scripts/import-ice-rinks/import-ice-rinks.cs
//   DOTNET_ENVIRONMENT=Development   (nutné pro načtení user secrets)
//
// Alternativně env vars (fungují vždy):
//   $env:ConnectionStrings__DefaultConnection = "..."
//   $env:MapyCom__ApiKey = "..."

#:project ../../src/SportSys.ConsoleApp/SportSys.ConsoleApp.csproj
#:project ../../src/SportSys.Database/SportSys.Database.csproj
#:property PublishAot=false

using DbIceRink = SportSys.Database.Models.sportSchema.IceRink;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NetTopologySuite.Geometries;
using SportSys.ConsoleApp;
using SportSys.ConsoleApp.Model.Config;
using SportSys.Database.Context;
using System.Text.RegularExpressions;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((ctx, services) =>
    {
        var connStr = ctx.Configuration.GetConnectionString("DefaultConnection")!;
        services.AddDbContext<SportSysDbContext>(opt =>
            opt.UseSqlServer(connStr, x => x.UseNetTopologySuite()));
        services.Configure<MapyCom>(ctx.Configuration.GetSection("MapyCom"));
        services.AddSingleton<HttpService>();
    })
    .Build();

var logger = host.Services.GetRequiredService<ILogger<Program>>();

var htmlPath = args.Length > 0
    ? args[0]
    : Path.Combine(Directory.GetCurrentDirectory(), "src", "DB Model", "Migration", "IceRink.html");

if (!File.Exists(htmlPath))
{
    logger.LogError("HTML soubor nenalezen: {Path}", htmlPath);
    return 1;
}

var html = await File.ReadAllTextAsync(htmlPath);

// Extrahovat texty neprázdných <option value="NNN">text</option>
var stadiums = Regex.Matches(html, @"<option\s+value=""(\d+)"">([^<]+)</option>")
    .Select(m => m.Groups[2].Value.Trim())
    .Where(t => !string.IsNullOrWhiteSpace(t))
    .ToList();

logger.LogInformation("Nalezeno {Count} stadionů k importu.", stadiums.Count);

await using var scope = host.Services.CreateAsyncScope();
var db    = scope.ServiceProvider.GetRequiredService<SportSysDbContext>();
var http  = scope.ServiceProvider.GetRequiredService<HttpService>();

// SRID 4326 = WGS84 (GPS souřadnice): X = zeměpisná délka (lon), Y = zeměpisná šířka (lat)
var geoFactory = new GeometryFactory(new PrecisionModel(), 4326);
int created = 0, updated = 0, errors = 0;

foreach (var name in stadiums)
{
    try
    {
        logger.LogInformation("Geocoding: {Name}", name);
        var geo = await http.Search(name);

        var location = geoFactory.CreatePoint(new Coordinate(geo.Lon, geo.Lat));

        var existing = await db.IceRinks.FirstOrDefaultAsync(r => r.Name == name);
        if (existing is null)
        {
            db.IceRinks.Add(new DbIceRink
            {
                Name     = name,
                Street   = geo.Street,
                City     = geo.City,
                ZipCode  = geo.ZipCode,
                Location = location
            });
            logger.LogInformation("  → přidán");
            created++;
        }
        else
        {
            existing.Street   = geo.Street;
            existing.City     = geo.City;
            existing.ZipCode  = geo.ZipCode;
            existing.Location = location;
            logger.LogInformation("  → aktualizován");
            updated++;
        }
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "  → chyba: {Name}", name);
        errors++;
    }

    // Rate limiting – Mapy.com geocoding API
    await Task.Delay(500);
}

await db.SaveChangesAsync();
logger.LogInformation(
    "Dokončeno: přidáno {Created}, aktualizováno {Updated}, chyb {Errors}.",
    created, updated, errors);

return 0;
