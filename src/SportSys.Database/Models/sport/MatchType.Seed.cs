using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using SportSys.Database.Enums;

namespace SportSys.Database.Models.sportSchema;

public partial class MatchType
{
    private MatchType() { Name = null!; }

    [SetsRequiredMembers]
    public MatchType(EMatchType id)
    {
        Id   = (int)id;
        Name = Resources.EMatchType.ResourceManager
                   .GetString(id.ToString(), CultureInfo.GetCultureInfo("cs"))
               ?? id.ToString();
    }
}
