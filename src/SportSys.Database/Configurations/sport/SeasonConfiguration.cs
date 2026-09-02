using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SportSys.Database.Models.sport;

namespace SportSys.Database.Configurations.sport;

public class SeasonConfiguration : IEntityTypeConfiguration<Season>
{
    public void Configure(EntityTypeBuilder<Season> builder)
    {
        builder.Property(e => e.IsActive)
               .HasDefaultValue(true, "DF_Season_IsActive");
    }
}
