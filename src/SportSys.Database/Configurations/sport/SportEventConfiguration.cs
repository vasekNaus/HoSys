using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SportSys.Database.Models.sportSchema;

namespace SportSys.Database.Configurations.sport;

public class SportEventConfiguration : IEntityTypeConfiguration<SportEvent>
{
    public void Configure(EntityTypeBuilder<SportEvent> builder)
    {
        builder.UseTpcMappingStrategy();

        // IdConvention zpracovává pouze GetDeclaredForeignKeys(); SportEvent nemá tabulku, takže by
        // přejmenování SeasonId a SeasonCategoryName bylo přeskočeno. Nastavujeme explicitně.
        builder.Property(e => e.SeasonId).HasColumnName("SeasonCategory_Season_Id");
        builder.Property(e => e.SeasonCategoryName).HasColumnName("SeasonCategory_Name");
    }
}
