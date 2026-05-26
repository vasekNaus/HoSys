// Spuštění:
//   dotnet scripts/import-ice-rinks.cs -- --conn "Server=...;Database=SportSys;..." --api-key "..." --html "src/DB Model/Migration/IceRink.html"
//
// Parametry:
//   --conn           Connection string k databázi SportSys (povinný)
//   --api-key        API klíč Mapy.com (povinný)
//   --html           Cesta k IceRink.html (výchozí: src/data/IceRink.html od cwd)
//   --base-url       Base URL Mapy.com API (výchozí: https://api.mapy.com/)
//   --geocode-route  Route pro geocoding (výchozí: /v1/geocode)

#:project ../src/SportSys.Contract/SportSys.Contract.csproj
#:project ../src/SportSys.Database/SportSys.Database.csproj
#:package Microsoft.Extensions.Logging.Console@10.0.8
#:property PublishAot=false

using DbIceRink = SportSys.Database.Models.sportSchema.IceRink;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NetTopologySuite.Geometries;
using SportSys.Contract.Config;
using SportSys.Contract.Services;
using SportSys.Database.Context;
using System.Text.Json;
using System.Text.RegularExpressions;

static string? GetArg(string[] args, string name)
{
    var idx = Array.IndexOf(args, name);
    return idx >= 0 && idx + 1 < args.Length ? args[idx + 1] : null;
}

var connStr      = GetArg(args, "--conn")          ?? throw new ArgumentException("Chybí --conn <connection-string>");
var apiKey       = GetArg(args, "--api-key")       ?? throw new ArgumentException("Chybí --api-key <key>");
var htmlPath     = GetArg(args, "--html")          ?? throw new ArgumentException("Chybí --html <vstupní soubor>");
var baseUrl      = GetArg(args, "--base-url")      ?? "https://api.mapy.com/";
var geocodeRoute = GetArg(args, "--geocode-route") ?? "/v1/geocode";

using var loggerFactory = LoggerFactory.Create(b => b.AddConsole());
var logger = loggerFactory.CreateLogger("import-ice-rinks");

if (!File.Exists(htmlPath))
{
    logger.LogError("HTML soubor nenalezen: {Path}", htmlPath);
    return 1;
}

var mapyComOptions = Options.Create(new MapyCom { ApiKey = apiKey, BaseUrl = baseUrl, GeocodeRoute = geocodeRoute });
var jsonOptions    = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
var httpService    = new HttpService(loggerFactory, mapyComOptions, jsonOptions);

var dbContextOptions = new DbContextOptionsBuilder<SportSysDbContext>()
    .UseSqlServer(connStr, x => x.UseNetTopologySuite())
    .Options;
await using var db = new SportSysDbContext(dbContextOptions);

var html = await File.ReadAllTextAsync(htmlPath);

// Extrahovat texty neprázdných <option value="NNN">text</option>
var stadiums = Regex.Matches(html, @"<option\s+value=""(\d+)"">([^<]+)</option>")
    .Select(m => m.Groups[2].Value.Trim())
    .Where(t => !string.IsNullOrWhiteSpace(t))
    .ToList();

logger.LogInformation("Nalezeno {Count} stadionů k importu.", stadiums.Count);

// SRID 4326 = WGS84 (GPS souřadnice): X = zeměpisná délka (lon), Y = zeměpisná šířka (lat)
var geoFactory = new GeometryFactory(new PrecisionModel(), 4326);
int created = 0, updated = 0, errors = 0;

foreach (var name in stadiums)
{
    try
    {
        logger.LogInformation("Geocoding: {Name}", name);
        var geo = await httpService.Search(name);
        if (geo is null)
        {
            logger.LogWarning("  → geocoding nenašel výsledek, přeskakuji");
            errors++;
            continue;
        }

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
    await Task.Delay(100);
}

await db.SaveChangesAsync();
logger.LogInformation(
    "Dokončeno: přidáno {Created}, aktualizováno {Updated}, chyb {Errors}.",
    created, updated, errors);

return 0;
