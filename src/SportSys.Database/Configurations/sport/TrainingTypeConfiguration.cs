using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SportSys.Database.Enums;
using SportSys.Database.Models.sportSchema;

namespace SportSys.Database.Configurations.sport;

public class TrainingTypeConfiguration : IEntityTypeConfiguration<TrainingType>
{
    public void Configure(EntityTypeBuilder<TrainingType> builder)
    {
        builder.HasData(
            Enum.GetValues<ETrainingType>()
                .Select(e => new TrainingType(e))
        );
    }
}
