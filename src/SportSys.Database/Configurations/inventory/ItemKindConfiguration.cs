using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SportSys.Database.Models.inventory;

namespace SportSys.Database.Configurations.inventory;

public class ItemKindConfiguration : IEntityTypeConfiguration<ItemKind>
{
    public void Configure(EntityTypeBuilder<ItemKind> builder)
    {
        builder.HasData(
            Enum.GetValues<EItemKind>()
                .Select(e => new ItemKind(e))
        );
    }
}
