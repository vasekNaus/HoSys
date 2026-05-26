// Spuštění:
//   dotnet scripts/import-teams.cs -- --conn "Server=...;Database=SportSys;..." --names "src/Data/Teams.html" --codes "src/Data/Teams-Code.html"
//
// Parametry:
//   --conn    Connection string k databázi SportSys (povinný)
//   --names   Cesta k Teams.html      (výchozí: src/Data/Teams.html od cwd)
//   --codes   Cesta k Teams-Code.html (výchozí: src/Data/Teams-Code.html od cwd)

#:project ../src/SportSys.Database/SportSys.Database.csproj
#:package Microsoft.Extensions.Logging.Console@10.0.8
#:property PublishAot=false

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SportSys.Database.Context;
using SportSys.Database.Models.sportSchema;
using System.Text.RegularExpressions;

static string? GetArg(string[] args, string name)
{
    var idx = Array.IndexOf(args, name);
    return idx >= 0 && idx + 1 < args.Length ? args[idx + 1] : null;
}

var connStr   = GetArg(args, "--conn")  ?? throw new ArgumentException("Chybí --conn <connection-string>");
var namesPath = GetArg(args, "--names") ?? "src/Data/Teams.html";
var codesPath = GetArg(args, "--codes") ?? "src/Data/Teams-Code.html";

using var loggerFactory = LoggerFactory.Create(b => b.AddConsole());
var logger = loggerFactory.CreateLogger("import-teams");

if (!File.Exists(namesPath)) { logger.LogError("Soubor nenalezen: {Path}", namesPath); return 1; }
if (!File.Exists(codesPath)) { logger.LogError("Soubor nenalezen: {Path}", codesPath); return 1; }

// Parsování: value → text z <option value="NNN">text</option>
static Dictionary<string, string> ParseOptions(string html) =>
    Regex.Matches(html, @"<option\s+value=""(\d+)"">([^<]+)</option>")
         .ToDictionary(m => m.Groups[1].Value, m => m.Groups[2].Value.Trim());

var namesHtml = await File.ReadAllTextAsync(namesPath);
var codesHtml = await File.ReadAllTextAsync(codesPath);

var names = ParseOptions(namesHtml); // value → Name
var codes = ParseOptions(codesHtml); // value → Code

logger.LogInformation("Týmy (Names):  {Count} záznamů.", names.Count);
logger.LogInformation("Kódy  (Codes): {Count} záznamů.", codes.Count);

// Kontrola 1:1 – varování při nesouladu
foreach (var key in names.Keys.Where(k => !codes.ContainsKey(k)))
    logger.LogWarning("WARNING: value={Key} ({Name}) nemá odpovídající záznam v Teams-Code.html.", key, names[key]);

foreach (var key in codes.Keys.Where(k => !names.ContainsKey(k)))
    logger.LogWarning("WARNING: value={Key} ({Code}) nemá odpovídající záznam v Teams.html.", key, codes[key]);

// Spojení do seznamu týmů
var teams = names
    .Select(kv => new
    {
        Name = kv.Value,
        Code = codes.TryGetValue(kv.Key, out var code) ? code : string.Empty,
    })
    .OrderBy(t => t.Name)
    .ToList();

logger.LogInformation("Celkem k importu: {Count} týmů.", teams.Count);

var dbOptions = new DbContextOptionsBuilder<SportSysDbContext>()
    .UseSqlServer(connStr, x => x.UseNetTopologySuite())
    .Options;
await using var db = new SportSysDbContext(dbOptions);

int created = 0, updated = 0;

foreach (var t in teams)
{
    var existing = await db.Teams.FirstOrDefaultAsync(r => r.Name == t.Name);
    if (existing is null)
    {
        db.Teams.Add(new Team
        {
            Name    = t.Name,
            Code    = t.Code.Length > 5 ? t.Code[..5] : t.Code,
            Address = string.Empty,
            City    = string.Empty,
        });
        logger.LogInformation("  + {Name} [{Code}]", t.Name, t.Code);
        created++;
    }
    else
    {
        existing.Code = t.Code.Length > 5 ? t.Code[..5] : t.Code;
        logger.LogInformation("  ~ {Name} [{Code}] (aktualizován)", t.Name, t.Code);
        updated++;
    }
}

await db.SaveChangesAsync();
logger.LogInformation("Dokončeno: přidáno {Created}, aktualizováno {Updated}.", created, updated);

return 0;
