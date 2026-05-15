using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SportSys.Database.Models.sportSchema;

namespace SportSys.Database.Configurations;

public class ParticipationTypeConfiguration : IEntityTypeConfiguration<ParticipationType>
{
    public void Configure(EntityTypeBuilder<ParticipationType> builder)
    {
        // Název PK constraint se liší od konvence (obsahuje mezeru) — nelze vyjádřit Data Annotation
        builder.HasKey(e => e.Id).HasName("PK_ParticipationType ");
    }
}
