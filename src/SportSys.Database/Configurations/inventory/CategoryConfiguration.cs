using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SportSys.Database.Models.inventory;

namespace SportSys.Database.Configurations.inventory;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    private static readonly JsonSerializerOptions JsonOpts = new()
    {
        Converters = { new JsonStringEnumConverter() }
    };

    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.Property(e => e.CategoryKinds)
               .HasColumnName("CategoryKindJson")
               .HasConversion(
                   v => v == null || v.Length == 0
                       ? null
                       : JsonSerializer.Serialize(v, JsonOpts),
                   v => string.IsNullOrEmpty(v)
                       ? null
                       : JsonSerializer.Deserialize<CategoryKind[]>(v, JsonOpts)
               );
    }
}
