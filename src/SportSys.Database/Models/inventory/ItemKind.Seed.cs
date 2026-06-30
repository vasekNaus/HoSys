using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace SportSys.Database.Models.inventory;

public partial class ItemKind
{
    private ItemKind() { Name = null!; }

    [SetsRequiredMembers]
    public ItemKind(EItemKind id)
    {
        Id   = (int)id;
        Name = Resources.EItemKind.ResourceManager
                   .GetString(id.ToString(), CultureInfo.GetCultureInfo("cs"))
               ?? id.ToString();
    }
}
