using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SportSys.Database.Enums;
using SportSys.Database.Models.sportSchema;

namespace SportSys.Database.Configurations.sport;

public class ParticipationTypeConfiguration : IEntityTypeConfiguration<ParticipationType>
{
    public void Configure(EntityTypeBuilder<ParticipationType> builder)
    {
        builder.HasData(
            Enum.GetValues<EParticipationType>()
                .Select(e => new ParticipationType(e))
        );
    }
}
