using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SportSys.Database.Models.inventory;

namespace SportSys.Database.Configurations.inventory;

public class InventoryItemConfiguration : IEntityTypeConfiguration<InventoryItem>
{
    public void Configure(EntityTypeBuilder<InventoryItem> builder)
    {
        // TPC – InventoryItem nemá vlastní tabulku; UseTpcMappingStrategy nelze vyjádřit atributem.
        builder.UseTpcMappingStrategy();
    }
}
