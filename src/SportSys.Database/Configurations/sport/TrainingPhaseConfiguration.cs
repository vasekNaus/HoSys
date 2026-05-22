using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SportSys.Database.Enums;
using SportSys.Database.Models.sportSchema;

namespace SportSys.Database.Configurations.sport;

public class TrainingPhaseConfiguration : IEntityTypeConfiguration<TrainingPhase>
{
    public void Configure(EntityTypeBuilder<TrainingPhase> builder)
    {
        builder.HasData(
            Enum.GetValues<ETrainingPhase>()
                .Select(e => new TrainingPhase(e))
        );
    }
}
