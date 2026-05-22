using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using SportSys.Database.Enums;

namespace SportSys.Database.Models.dboSchema;

public partial class CoachRole
{
    private CoachRole() { Name = null!; }

    [SetsRequiredMembers]
    public CoachRole(ECoachRole id)
    {
        Id   = (int)id;
        Name = Resources.ECoachRole.ResourceManager
                   .GetString(id.ToString(), CultureInfo.GetCultureInfo("cs"))
               ?? id.ToString();
    }
}
