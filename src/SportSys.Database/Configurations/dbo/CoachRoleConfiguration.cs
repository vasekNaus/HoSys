using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SportSys.Database.Enums;
using SportSys.Database.Models.dboSchema;

namespace SportSys.Database.Configurations.dbo;

public class CoachRoleConfiguration : IEntityTypeConfiguration<CoachRole>
{
    public void Configure(EntityTypeBuilder<CoachRole> builder)
    {
        builder.HasData(
            Enum.GetValues<ECoachRole>()
                .Select(e => new CoachRole(e))
        );
    }
}
