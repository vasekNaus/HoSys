using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SportSys.Database.Models.sportSchema;

namespace SportSys.Database.Configurations.sport;

public class MatchConfiguration : IEntityTypeConfiguration<Match>
{
    public void Configure(EntityTypeBuilder<Match> builder)
    {
        // TPC dědičnost nepodporuje pojmenované DEFAULT constrainty – nelze použít dvouparametrový overload.
        builder.Property(e => e.Id)
               .HasDefaultValueSql("(NEXT VALUE FOR [sport].[SportEventSeq])");

        // Dvě FK na stejnou tabulku Team – explicitní konfigurace zabraňuje ambiguitě
        builder.HasOne(m => m.HomeTeam)
               .WithMany(o => o.HomeMatches)
               .HasForeignKey(m => m.HomeTeamId)
               .OnDelete(DeleteBehavior.ClientSetNull);

        builder.HasOne(m => m.AwayTeam)
               .WithMany(o => o.AwayMatches)
               .HasForeignKey(m => m.AwayTeamId)
               .OnDelete(DeleteBehavior.ClientSetNull);

    // Výsledek zápasu jako JSON sloupec přes value converter.
    // OwnsOne...ToJson není kompatibilní s TPC dědičností (EF Core omezení).
    builder.Property(e => e.Result)
           .HasColumnName("Result")
           .HasColumnType("json")
           .HasConversion(
               v => v == null ? null : JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
               v => v == null ? null : JsonSerializer.Deserialize<MatchResult>(v, (JsonSerializerOptions?)null));
  }
}

