using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SportSys.Database.Models.sport;

namespace SportSys.Database.Configurations.sport;

public class SportEventConfiguration : IEntityTypeConfiguration<SportEvent>
{
    public void Configure(EntityTypeBuilder<SportEvent> builder)
    {
        // TPC – Sport Event nemá vlastní tabulku; UseTpcMappingStrategy nelze vyjádřit data atributem.
        builder.UseTpcMappingStrategy();
    }
}
