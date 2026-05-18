using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SportSys.Database.Models.sportSchema;

namespace SportSys.Database.Configurations.sport;

public class TrainingPhaseConfiguration : IEntityTypeConfiguration<TrainingPhase>
{
    public void Configure(EntityTypeBuilder<TrainingPhase> builder)
    {
        // Název PK constraint se liší od konvence — nelze vyjádřit Data Annotation
        builder.HasKey(e => e.Id).HasName("PK_Phase");
    }
}
