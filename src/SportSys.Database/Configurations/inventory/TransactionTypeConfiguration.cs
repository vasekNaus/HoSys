using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SportSys.Database.Models.inventory;


namespace SportSys.Database.Configurations.inventory;

public class TransactionTypeConfiguration : IEntityTypeConfiguration<TransactionType>
{
    public void Configure(EntityTypeBuilder<TransactionType> builder)
    {
        builder.HasData(
            Enum.GetValues<ETransactionType>()
                .Select(e => new TransactionType(e))
        );
    }
}
