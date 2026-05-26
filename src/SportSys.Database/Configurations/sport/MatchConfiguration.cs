using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SportSys.Database.Models.sportSchema;

namespace SportSys.Database.Configurations.sport;

public class MatchConfiguration : IEntityTypeConfiguration<Match>
{
    public void Configure(EntityTypeBuilder<Match> builder)
    {
        builder.Property(e => e.Id)
               .HasDefaultValueSql("(NEXT VALUE FOR [sport].[SportEventSeq])", "DF_Match_Id");

        // Dvě FK na stejnou tabulku Team – explicitní konfigurace zabraňuje ambiguitě
        builder.HasOne(m => m.HomeTeam)
               .WithMany(o => o.HomeMatches)
               .HasForeignKey(m => m.HomeTeamId)
               .OnDelete(DeleteBehavior.ClientSetNull);

        builder.HasOne(m => m.AwayTeam)
               .WithMany(o => o.AwayMatches)
               .HasForeignKey(m => m.AwayTeamId)
               .OnDelete(DeleteBehavior.ClientSetNull);

        // Výsledek zápasu jako JSON sloupec
        builder.OwnsOne(m => m.Result, r => r.ToJson("Result"));
    }
}
