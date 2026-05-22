namespace SportSys.Contract.Models;

public record class Root
{
  public List<Item> Items { get; init; } = [];
}

public record class Item
{
  public List<double> Bbox { get; init; } = [];
  public string Label { get; init; } = "";
  public string Location { get; init; } = "";
  public string Name { get; init; } = "";
  public Position Position { get; init; } = new();
  public List<RegionalStructure> RegionalStructure { get; init; } = [];
  public string Type { get; init; } = "";
  public string? Zip { get; init; }
}

public record class Position
{
  public double Lat { get; init; }
  public double Lon { get; init; }
}

public record class RegionalStructure
{
  public string Name { get; init; } = "";
  public string Type { get; init; } = "";
  public string? IsoCode { get; init; }  // jen u country
}
