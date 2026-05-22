using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using SportSys.Database.Enums;

namespace SportSys.Database.Models.sportSchema;

public partial class TrainingType
{
    private TrainingType() { Name = null!; }

    [SetsRequiredMembers]
    public TrainingType(ETrainingType id)
    {
        Id   = (int)id;
        Name = Resources.ETrainingType.ResourceManager
                   .GetString(id.ToString(), CultureInfo.GetCultureInfo("cs"))
               ?? id.ToString();
    }
}
