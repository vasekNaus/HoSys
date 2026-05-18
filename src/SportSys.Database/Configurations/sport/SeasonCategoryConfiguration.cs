using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SportSys.Database.Models.sportSchema;

namespace SportSys.Database.Configurations.sport;

public class SeasonCategoryConfiguration : IEntityTypeConfiguration<SeasonCategory>
{
    public void Configure(EntityTypeBuilder<SeasonCategory> builder)
    {
        builder.Property(e => e.BirthYears)
               .HasDefaultValue("[]", "DF_SeasonCategory_BirthYears");
    }
}
