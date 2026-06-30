using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SportSys.Database.Models;
using SportSys.Database.Models.inventory;

namespace SportSys.Database.Configurations.inventory;

public class EquipmentConfiguration : IEntityTypeConfiguration<Equipment>
{
    public void Configure(EntityTypeBuilder<Equipment> builder)
    {
        // TPC dědičnost nepodporuje pojmenované DEFAULT constrainty.
        builder.Property(e => e.Id)
               .HasDefaultValueSql("(NEXT VALUE FOR [inventory].[InventoryItemSeq])");

        // Poznámka: HasColumnName pro ItemKindId NELZE řešit zde —
        // ApplyConfigurationsFromAssembly() běží před IdConvention() v OnModelCreating,
        // takže by konvence override přepsala. Oprava je v SportSysDbContext.OnModelCreating
        // za voláním IdConvention().
    }
}
