using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SportSys.Database.Models.dboSchema;
using SportSys.Database.Models.sport;

namespace SportSys.Database.Configurations.dbo;

public class CoachTrainingConfiguration : IEntityTypeConfiguration<CoachTraining>
{
    public void Configure(EntityTypeBuilder<CoachTraining> builder)
    {
        builder.Property(e => e.Note)
               .HasDefaultValue("", "DF_CoachTraining_Note");
    }
}
