using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SportSys.Database.Enums;
using SportSys.Database.Models.sportSchema;

namespace SportSys.Database.Configurations.sport;

public class TrainingStateConfiguration : IEntityTypeConfiguration<TrainingState>
{
    public void Configure(EntityTypeBuilder<TrainingState> builder)
    {
        builder.HasData(
            Enum.GetValues<ETrainingState>()
                .Select(e => new TrainingState(e))
        );
    }
}
