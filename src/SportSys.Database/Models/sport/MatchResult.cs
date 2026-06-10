namespace SportSys.Database.Models.sport;

/// <summary>
/// Výsledek zápasu ukládaný jako JSON sloupec na entitě <see cref="Match"/>.
/// Třídu je možné libovolně rozšiřovat o nové údaje bez nutnosti databázové migrace.
/// </summary>
public class MatchResult
{
    public int? HomeGoals { get; set; }
    public int? AwayGoals { get; set; }
}
