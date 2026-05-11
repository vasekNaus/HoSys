using System.Text.Json;

/// <summary>
/// Vyhledá adresu místa pomocí OpenStreetMap Nominatim API (zdarma, bez klíče).
/// Limit: max 1 požadavek za sekundu. User-Agent je povinný dle podmínek použití.
/// </summary>
public static class NominatimService
{
  private const string BaseUrl = "https://nominatim.openstreetmap.org/search";
  private const string UserAgent = "SportSys/1.0 (vasek.naus@outlook.cz)";

  public record GeoResult(string Address, string City);

  /// <summary>
  /// Hledá adresu zadaného místa (např. "Klatovy, zimní stadion").
  /// Vrátí null, pokud nebylo nalezeno žádné výsledek.
  /// </summary>
  public static async Task<GeoResult?> SearchAsync(string query, HttpClient http, CancellationToken ct = default)
  {
    if (string.IsNullOrWhiteSpace(query)) return null;

    var url = $"{BaseUrl}?q={Uri.EscapeDataString(query)}&format=json&addressdetails=1&countrycodes=cz&limit=1";

    using var request = new HttpRequestMessage(HttpMethod.Get, url);
    request.Headers.Add("User-Agent", UserAgent);
    request.Headers.Add("Accept-Language", "cs");

    HttpResponseMessage response;
    try
    {
      response = await http.SendAsync(request, ct);
      response.EnsureSuccessStatusCode();
    }
    catch (Exception ex)
    {
      Console.WriteLine($"  [Nominatim] Chyba dotazu pro '{query}': {ex.Message}");
      return null;
    }

    // Rate limit: 1 req/s dle podmínek OSM
    await Task.Delay(1100, ct);

    await using var stream = await response.Content.ReadAsStreamAsync(ct);
    using var doc = await JsonDocument.ParseAsync(stream, cancellationToken: ct);
    var root = doc.RootElement;

    if (root.ValueKind != JsonValueKind.Array || root.GetArrayLength() == 0)
    {
      Console.WriteLine($"  [Nominatim] Žádný výsledek pro '{query}'.");
      return null;
    }

    var first = root[0];
    var addrParts = new List<string>();
    string city = string.Empty;

    if (first.TryGetProperty("address", out var addr))
    {
      // Ulice + číslo
      if (addr.TryGetProperty("road", out var road))
        addrParts.Add(road.GetString() ?? string.Empty);
      if (addr.TryGetProperty("house_number", out var houseNum))
        addrParts.Add(houseNum.GetString() ?? string.Empty);

      // Město — postupně od nejkonkrétnějšího
      city = addr.TryGetProperty("city", out var c) ? c.GetString() ?? string.Empty
           : addr.TryGetProperty("town", out var t) ? t.GetString() ?? string.Empty
           : addr.TryGetProperty("village", out var v) ? v.GetString() ?? string.Empty
           : string.Empty;
    }

    string address = string.Join(" ", addrParts).Trim();

    Console.WriteLine($"  [Nominatim] '{query}' → adresa: '{address}', město: '{city}'");
    return new GeoResult(address, city);
  }
}
