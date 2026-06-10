using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SportSys.Database.Models.dbo;

namespace SportSys.Database.Configurations.dbo;

public class CoachConfiguration : IEntityTypeConfiguration<Coach>
{
    public void Configure(EntityTypeBuilder<Coach> builder)
    {
        builder.Property(e => e.FullName)
               .HasComputedColumnSql("(([FirstName]+N' ')+[LastName])", stored: true);
    }
}
