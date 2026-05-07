using System;
using System.Data;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using ExcelDataReader;


public sealed record Training(int SessionId, DateOnly Date, TimeOnly From, TimeOnly To, string CategoryName, string ParticipationType, string Note, string CoachLastName);


public static class ImportRun
{
  public static async Task ImportAsync(string filePath, string connectionString, CancellationToken ct = default)
  {
    string GetCoachName()
    {
      string fileName = Path.GetFileName(filePath);
      var spaceIndex = Path.GetFileName(filePath).IndexOf(' ');
      var dotIndex = Path.GetFileName(filePath).IndexOf('.');

      return fileName.Substring(spaceIndex + 1, dotIndex - spaceIndex - 1);
    }
    if (string.IsNullOrEmpty(filePath)) throw new ArgumentNullException(nameof(filePath));

    Console.WriteLine($"Importuji soubor: {filePath}");
    if (File.Exists(filePath))
    {
      await using var fs = File.OpenRead(filePath); // dle tvé konvence bez diakritiky :)

      string coachLastName = GetCoachName();

      // Pro .xls (BIFF) je nutné povolit kódové stránky:
      Encoding.RegisterProvider(CodePagesEncodingProvider.Instance); // ExcelDataReader doporučuje pro staré formáty [2](https://github.com/ExcelDataReader/ExcelDataReader)

      using var reader = ExcelReaderFactory.CreateReader(fs ); // auto-detekce xls/xlsx/xlsb [2](https://github.com/ExcelDataReader/ExcelDataReader)

      // Pokud chceš DataSet (např. pro pohodlný přístup přes názvy sloupců):
      var ds = reader.AsDataSet(new ExcelDataSetConfiguration
      {
        ConfigureDataTable = _ => new ExcelDataTableConfiguration
        {
          UseHeaderRow = true // první řádek je hlavička
        }
      }); // AsDataSet poskytuje tabulky po listech [2](https://github.com/ExcelDataReader/ExcelDataReader)

      if (ds.Tables.Count == 0) return;
      var table = ds.Tables[0];

      using var conn = new SqlConnection(connectionString);

      var messages = new StringBuilder();
      // DŮLEŽITÉ: Přihlásit se k události ještě před OpenAsync.
      conn.InfoMessage += (sender, e) =>
      {
        // e.Message = sloučený text; e.Errors může obsahovat víc položek
        messages.AppendLine(e.Message);
      };

      // Zajistí, že i RAISERROR se závažností 10 a nižší dorazí jako InfoMessage.
      // (PRINT dorazí tak jako tak.)
      conn.FireInfoMessageEventOnUserErrors = true;


      await conn.OpenAsync(ct);
      using var tx = await conn.BeginTransactionAsync(ct);

      try
      {
        foreach (DataRow dr in table.Rows)
        {
          // načti hodnoty podle hlaviček ze vzorku („Datum“, „Čas od“, …)
          var datumObj = dr.Table.Columns.Contains("Datum") ? dr["Datum"] : null;
          var casOdObj = dr.Table.Columns.Contains("Čas od") ? dr["Čas od"] : dr.Table.Columns.Contains("Cas od") ? dr["Cas od"] : null;
          var casDoObj = dr.Table.Columns.Contains("Čas do") ? dr["Čas do"] : dr.Table.Columns.Contains("Cas do") ? dr["Cas do"] : null;

          DateTime? datum = TryParseExcelDate(datumObj);
          TimeSpan? casOd = TryParseTime(casOdObj);
          TimeSpan? casDo = TryParseTime(casDoObj);

          string iceTyp = GetString(dr, "Typ");
          string categoryName = GetString(dr, "Soupiska");
          string participationType = GetString(dr, "Účast / neúčast");
          string note = GetString(dr, "Poznámka");

          if (datum.HasValue && casOd.HasValue && casDo.HasValue && iceTyp.ToLower() == "trénink" && !string.IsNullOrEmpty(categoryName) && !string.IsNullOrEmpty(participationType))
          {
            var t = new Training(1, DateOnly.FromDateTime(datum.Value), TimeOnly.FromTimeSpan(casOd.Value), TimeOnly.FromTimeSpan(casDo.Value),
                                 categoryName, participationType, note, coachLastName);

            using var cmd = new SqlCommand("[dbo].[procImportCoachTrainingKIS]", conn, (SqlTransaction)tx);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add(new SqlParameter("@seasonId", SqlDbType.Int) { Value = t.SessionId });
            cmd.Parameters.Add(new SqlParameter("@date", SqlDbType.Date) { Value = t.Date });
            cmd.Parameters.Add(new SqlParameter("@from", SqlDbType.Time) { Value = t.From });
            cmd.Parameters.Add(new SqlParameter("@to", SqlDbType.Time) { Value = t.To });
            cmd.Parameters.Add(new SqlParameter("@category", SqlDbType.NVarChar, 200) { Value = t.CategoryName });
            cmd.Parameters.Add(new SqlParameter("@ParticipationType", SqlDbType.NVarChar, 50) { Value = t.ParticipationType });
            cmd.Parameters.Add(new SqlParameter("@CoachLastName", SqlDbType.NVarChar, 50) { Value = t.CoachLastName });
            cmd.Parameters.Add(new SqlParameter("@Note", SqlDbType.NVarChar, 50) { Value = t.Note });

            messages.Clear();
            int affected = await cmd.ExecuteNonQueryAsync(ct);

            // Můžeš zalogovat:
            Console.WriteLine($"PRINT/InfoMessage z procedury:\n{messages.ToString()}");
            Console.WriteLine($"Počet ovlivněných řádků: {affected}");
          }
        }
        messages.Clear();
        await tx.CommitAsync(ct);
      }
      catch
      {
        await tx.RollbackAsync(ct);
        throw;
      }
    }
  }

  private static string GetString(DataRow dr, string col) => dr.Table.Columns.Contains(col) ? dr[col]?.ToString()?.Trim() ?? string.Empty : string.Empty;

  private static DateTime? TryParseExcelDate(object? v)
  {
    if (v == null || v == DBNull.Value) return null;
    // ExcelDataReader vrací pro .xlsx datum často jako double (OA Date) anebo už DateTime
    if (v is DateTime dt) return dt.Date;
    if (double.TryParse(v.ToString(), NumberStyles.Any, CultureInfo.InvariantCulture, out var d))
      return DateTime.FromOADate(d).Date; // převod OADate (Excel serial) [2](https://github.com/ExcelDataReader/ExcelDataReader)
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
      if (d >= 0 && d < 1) return TimeSpan.FromDays(d); // čas jako zlomek dne
    }
    if (TimeSpan.TryParse(v.ToString(), out var ts)) return ts;
    if (TimeSpan.TryParse(v.ToString(), new CultureInfo("cs-CZ"), out ts)) return ts;
    return null;
  }

  private static bool? ParseUcast(string s)
  {
    s = s?.Trim().ToLowerInvariant();
    if (string.IsNullOrEmpty(s)) return null;
    if (s is "účast" or "ucast" or "ano" or "a" or "yes" or "y" or "1") return true;
    if (s is "neúčast" or "neucast" or "ne" or "n" or "no" or "0") return false;
    return null;
  }
}
