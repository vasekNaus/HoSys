using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SportSys.Database.Models.sportSchema;

namespace SportSys.Database.Configurations.sport;

public class MatchConfiguration : IEntityTypeConfiguration<Match>
{
    public void Configure(EntityTypeBuilder<Match> builder)
    {
        builder.Property(e => e.Id)
               .HasDefaultValueSql("(NEXT VALUE FOR [sport].[SportEventSeq])", "DF__Match__Id");

        builder.Property(e => e.DurationMinutes)
               .HasComputedColumnSql("(datediff(minute,[TimeFrom],[TimeTo]))", stored: true);
    }
}
