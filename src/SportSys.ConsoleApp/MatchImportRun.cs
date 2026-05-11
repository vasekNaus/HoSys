using System.Data;
using System.Globalization;
using System.IO;
using System.Text;
using Microsoft.EntityFrameworkCore;
using ExcelDataReader;
using SportSys.Database.Context;
using IceRink = SportSys.Database.Models.Sport.IceRink;
using Opponent = SportSys.Database.Models.Sport.Opponent;
using MatchType = SportSys.Database.Models.Sport.MatchType;
using Match = SportSys.Database.Models.Sport.Match;

public static class MatchImportRun
{
  // Pokud je místo v tomto městě, hrajeme doma
  private const string HomeCity = "Klatovy";

  public static async Task ImportAsync(
    string filePath,
    SportSysDbContext db,
    HttpClient http,
    CancellationToken ct = default)
  {
    if (string.IsNullOrEmpty(filePath)) throw new ArgumentNullException(nameof(filePath));
    if (!File.Exists(filePath))
    {
      Console.WriteLine($"Soubor nenalezen: {filePath}");
      return;
    }

    Console.WriteLine($"Importuji zápasy ze souboru: {filePath}");

    // ── Krok A: načtení referenčních tabulek do Dictionary ──────────────────
    // Klíč je Name (case-insensitive), hodnota je Id
    var iceRinks = await db.IceRinks
      .ToDictionaryAsync(x => x.Name, x => x.Id, StringComparer.OrdinalIgnoreCase, ct);

    var opponents = await db.Opponents
      .ToDictionaryAsync(x => x.Name, x => x.Id, StringComparer.OrdinalIgnoreCase, ct);

    var matchTypes = await db.MatchTypes
      .ToDictionaryAsync(x => x.Name, x => x.Id, StringComparer.OrdinalIgnoreCase, ct);

    Console.WriteLine($"  Stadiony v DB:   {iceRinks.Count}");
    Console.WriteLine($"  Soupeři v DB:    {opponents.Count}");
    Console.WriteLine($"  Typy zápasů v DB:{matchTypes.Count}");

    // ── Krok B: načtení Excel souboru ───────────────────────────────────────
    Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
    await using var fs = File.OpenRead(filePath);
    using var reader = ExcelReaderFactory.CreateReader(fs);
    var ds = reader.AsDataSet(new ExcelDataSetConfiguration
    {
      ConfigureDataTable = _ => new ExcelDataTableConfiguration { UseHeaderRow = true }
    });

    if (ds.Tables.Count == 0) return;
    var table = ds.Tables[0];

    // ── Krok C: procházení řádků ─────────────────────────────────────────────
    int inserted = 0;
    int skipped = 0;

    foreach (DataRow dr in table.Rows)
    {
      // Datum
      var datumObj = dr.Table.Columns.Contains("Datum") ? dr["Datum"] : null;
      DateTime? datum = TryParseExcelDate(datumObj);
      if (!datum.HasValue)
      {
        skipped++;
        continue;
      }

      // Čas – pouze začátek, konec = začátek (Excel ho neobsahuje)
      var casObj = dr.Table.Columns.Contains("Čas") ? dr["Čas"]
                 : dr.Table.Columns.Contains("Cas") ? dr["Cas"]
                 : null;
      TimeSpan? casOd = TryParseTime(casObj);
      if (!casOd.HasValue)
      {
        skipped++;
        continue;
      }

      // Sezóna
      string seasonIdStr = GetString(dr, "SeasonId");
      if (!int.TryParse(seasonIdStr, out int seasonId))
      {
        Console.WriteLine($"  Přeskočen řádek – chybí SeasonId (datum {datum:d}).");
        skipped++;
        continue;
      }

      // Kategorie (soupiska)
      string soupiska = GetString(dr, "Soupiska");
      if (string.IsNullOrEmpty(soupiska))
      {
        skipped++;
        continue;
      }

      // Domácí / Hosté / Místo
      string domaci = GetString(dr, "Domácí");
      string hoste  = GetString(dr, "Hosté");
      string misto  = GetString(dr, "Místo");

      bool isHome = misto.Contains(HomeCity, StringComparison.OrdinalIgnoreCase);
      string opponentName = isHome ? hoste : domaci;

      if (string.IsNullOrEmpty(opponentName))
      {
        Console.WriteLine($"  Přeskočen řádek – chybí název soupeře (datum {datum:d}).");
        skipped++;
        continue;
      }

      // Typ zápasu
      string typZapasu = GetString(dr, "Typ");
      if (string.IsNullOrEmpty(typZapasu)) typZapasu = "Ligový zápas";

      string note = GetString(dr, "Poznámka");

      // ── Lookup nebo vytvoření: IceRink ──────────────────────────────────
      int iceRinkId = await GetOrCreateIceRinkAsync(misto, iceRinks, db, http, ct);

      // ── Lookup nebo vytvoření: Opponent ─────────────────────────────────
      int opponentId = await GetOrCreateOpponentAsync(opponentName, opponents, db, http, ct);

      // ── Lookup nebo vytvoření: MatchType ────────────────────────────────
      int matchTypeId = await GetOrCreateMatchTypeAsync(typZapasu, matchTypes, db, ct);

      // ── Id zápasu: získáme z DB sekvence ────────────────────────────────
      int newId = await GetNextSportEventIdAsync(db, ct);

      var match = new Match
      {
        Id = newId,
        SeasonId = seasonId,
        SeasonCategoryName = soupiska.Length > 10 ? soupiska[..10] : soupiska,
        IceRinkId = iceRinkId,
        Date = DateOnly.FromDateTime(datum.Value),
        TimeFrom = TimeOnly.FromTimeSpan(casOd.Value),
        TimeTo = TimeOnly.FromTimeSpan(casOd.Value), // konec = začátek (bude doplněno později)
        Note = note.Length > 50 ? note[..50] : note,
        MatchCode = null,
        OpponentId = opponentId,
        IsHome = isHome,
        GoalsScored = null,
        GoalsConceded = null,
        MatchTypeId = matchTypeId
      };

      db.Matches.Add(match);
      inserted++;
    }

    if (inserted > 0)
      await db.SaveChangesAsync(ct);

    Console.WriteLine($"  Vloženo: {inserted}, přeskočeno: {skipped}");
  }

  // ── Pomocné metody: lookup nebo vytvoření referenčního záznamu ───────────

  private static async Task<int> GetOrCreateIceRinkAsync(
    string name,
    Dictionary<string, int> dict,
    SportSysDbContext db,
    HttpClient http,
    CancellationToken ct)
  {
    if (string.IsNullOrWhiteSpace(name)) name = "Neznámý stadion";

    if (dict.TryGetValue(name, out int id)) return id;

    Console.WriteLine($"  Stadion '{name}' nenalezen v DB – hledám adresu přes Nominatim...");
    var geo = await NominatimService.SearchAsync(name, http, ct);

    var rink = new IceRink
    {
      Name    = name.Length > 100 ? name[..100] : name,
      Address = (geo?.Address ?? string.Empty).Length > 200 ? (geo!.Address)[..200] : (geo?.Address ?? string.Empty),
      City    = (geo?.City    ?? string.Empty).Length > 100 ? (geo!.City)[..100]    : (geo?.City    ?? string.Empty)
    };

    db.IceRinks.Add(rink);
    await db.SaveChangesAsync(ct); // nutné, abychom získali Id

    dict[name] = rink.Id;
    Console.WriteLine($"  Stadion '{name}' vložen s Id={rink.Id}.");
    return rink.Id;
  }

  private static async Task<int> GetOrCreateOpponentAsync(
    string name,
    Dictionary<string, int> dict,
    SportSysDbContext db,
    HttpClient http,
    CancellationToken ct)
  {
    if (dict.TryGetValue(name, out int id)) return id;

    Console.WriteLine($"  Soupeř '{name}' nenalezen v DB – hledám adresu přes Nominatim...");
    var geo = await NominatimService.SearchAsync(name, http, ct);

    var opponent = new Opponent
    {
      Name    = name.Length > 100 ? name[..100] : name,
      Address = (geo?.Address ?? string.Empty).Length > 200 ? (geo!.Address)[..200] : (geo?.Address ?? string.Empty),
      City    = (geo?.City    ?? string.Empty).Length > 100 ? (geo!.City)[..100]    : (geo?.City    ?? string.Empty)
    };

    db.Opponents.Add(opponent);
    await db.SaveChangesAsync(ct);

    dict[name] = opponent.Id;
    Console.WriteLine($"  Soupeř '{name}' vložen s Id={opponent.Id}.");
    return opponent.Id;
  }

  private static async Task<int> GetOrCreateMatchTypeAsync(
    string name,
    Dictionary<string, int> dict,
    SportSysDbContext db,
    CancellationToken ct)
  {
    if (dict.TryGetValue(name, out int id)) return id;

    Console.WriteLine($"  Typ zápasu '{name}' nenalezen v DB – vytvářím...");
    var matchType = new MatchType
    {
      Name = name.Length > 100 ? name[..100] : name
    };

    db.MatchTypes.Add(matchType);
    await db.SaveChangesAsync(ct);

    dict[name] = matchType.Id;
    Console.WriteLine($"  Typ zápasu '{name}' vložen s Id={matchType.Id}.");
    return matchType.Id;
  }

  /// <summary>
  /// Získá další hodnotu ze sekvence sport.SportEventSeq.
  /// Match.Id není IDENTITY, ale DEFAULT (NEXT VALUE FOR sport.SportEventSeq).
  /// EF Core pro ValueGeneratedNever nepřiřadí Id automaticky → musíme ho přečíst sami.
  /// </summary>
  private static async Task<int> GetNextSportEventIdAsync(SportSysDbContext db, CancellationToken ct)
  {
    var conn = db.Database.GetDbConnection();
    bool opened = conn.State != System.Data.ConnectionState.Open;
    if (opened) await conn.OpenAsync(ct);

    try
    {
      using var cmd = conn.CreateCommand();
      cmd.CommandText = "SELECT NEXT VALUE FOR [sport].[SportEventSeq]";
      var result = await cmd.ExecuteScalarAsync(ct);
      return Convert.ToInt32(result);
    }
    finally
    {
      if (opened) await conn.CloseAsync();
    }
  }

  // ── Parser helpers (stejný vzor jako ImportRun) ──────────────────────────

  private static string GetString(DataRow dr, string col)
    => dr.Table.Columns.Contains(col) ? dr[col]?.ToString()?.Trim() ?? string.Empty : string.Empty;

  private static DateTime? TryParseExcelDate(object? v)
  {
    if (v == null || v == DBNull.Value) return null;
    if (v is DateTime dt) return dt.Date;
    if (double.TryParse(v.ToString(), NumberStyles.Any, CultureInfo.InvariantCulture, out var d))
      return DateTime.FromOADate(d).Date;
    if (DateTime.TryParse(v.ToString(), new CultureInfo("cs-CZ"), DateTimeStyles.None, out var parsed))
      return parsed.Date;
    return null;
  }

  private static TimeSpan? TryParseTime(object? v)
  {
    if (v == null || v == DBNull.Value) return null;
    if (v is DateTime dt) return dt.TimeOfDay;
    if (double.TryParse(v.ToString(), NumberStyles.Any, CultureInfo.InvariantCulture, out var d))
    {
      if (d >= 0 && d < 1) return TimeSpan.FromDays(d);
    }
    if (TimeSpan.TryParse(v.ToString(), out var ts)) return ts;
    if (TimeSpan.TryParse(v.ToString(), new CultureInfo("cs-CZ"), out ts)) return ts;
    return null;
  }
}
