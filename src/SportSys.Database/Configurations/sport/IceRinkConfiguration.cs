using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SportSys.Database.Models.sport;

namespace SportSys.Database.Configurations.sport;

public class IceRinkConfiguration : IEntityTypeConfiguration<IceRink>
{
    public void Configure(EntityTypeBuilder<IceRink> builder)
    {
        builder.Property(e => e.ZipCode)
               .HasDefaultValue("", "DF_IceRink_ZipCode");

        builder.Property(e => e.IsActive)
               .HasDefaultValue(true, "DF_IceRink_IsActive");
    }
}
