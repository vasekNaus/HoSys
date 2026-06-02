using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SportSys.Database.Models.sportSchema;

namespace SportSys.Database.Configurations.sport;

public class TrainingConfiguration : IEntityTypeConfiguration<Training>
{
  public void Configure(EntityTypeBuilder<Training> builder)
  {
    // TPC dědičnost nepodporuje pojmenované DEFAULT constrainty – nelze použít dvouparametrový overload.
    builder.Property(e => e.Id)
           .HasDefaultValueSql("(NEXT VALUE FOR [sport].[SportEventSeq])");

    builder.Property(e => e.DurationMinutes)
               .HasComputedColumnSql("(datediff(minute,[TimeFrom],[TimeTo]))", stored: true);
  }
}
