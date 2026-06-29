using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SportSys.Database.Models.inventory;

namespace SportSys.Database.Configurations.inventory;

public class EquipmentConfiguration : IEntityTypeConfiguration<Equipment>
{
    public void Configure(EntityTypeBuilder<Equipment> builder)
    {
        // TPC dědičnost nepodporuje pojmenované DEFAULT constrainty.
        builder.Property(e => e.Id)
               .HasDefaultValueSql("(NEXT VALUE FOR [inventory].[InventoryItemSeq])");
    }
}
