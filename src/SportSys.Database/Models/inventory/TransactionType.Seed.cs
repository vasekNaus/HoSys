using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using SportSys.Database.Models.inventory;

namespace SportSys.Database.Models.inventory;

public partial class TransactionType
{
    private TransactionType() { Name = null!; }

    [SetsRequiredMembers]
    public TransactionType(ETransactionType id)
    {
        Id   = (int)id;
        Name = Resources.ETransactionType.ResourceManager
                   .GetString(id.ToString(), CultureInfo.GetCultureInfo("cs"))
               ?? id.ToString();
    }
}
