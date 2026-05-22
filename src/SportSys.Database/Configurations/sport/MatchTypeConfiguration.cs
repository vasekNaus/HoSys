using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SportSys.Database.Enums;
using MatchType = SportSys.Database.Models.sportSchema.MatchType;

namespace SportSys.Database.Configurations.sport;

public class MatchTypeConfiguration : IEntityTypeConfiguration<MatchType>
{
    public void Configure(EntityTypeBuilder<MatchType> builder)
    {
        builder.HasData(
            Enum.GetValues<EMatchType>()
                .Select(e => new MatchType(e))
        );
    }
}
