using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using SportSys.Database.Enums;

namespace SportSys.Database.Models.sportSchema;

public partial class ParticipationType
{
    private ParticipationType() { Name = null!; }

    [SetsRequiredMembers]
    public ParticipationType(EParticipationType id)
    {
        Id   = (int)id;
        Name = Resources.EParticipationType.ResourceManager
                   .GetString(id.ToString(), CultureInfo.GetCultureInfo("cs"))
               ?? id.ToString();
    }
}
