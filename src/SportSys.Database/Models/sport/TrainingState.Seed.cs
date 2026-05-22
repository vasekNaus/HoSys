using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using SportSys.Database.Enums;

namespace SportSys.Database.Models.sportSchema;

public partial class TrainingState
{
    private TrainingState() { Name = null!; }

    [SetsRequiredMembers]
    public TrainingState(ETrainingState id)
    {
        Id   = (int)id;
        Name = Resources.ETrainingState.ResourceManager
                   .GetString(id.ToString(), CultureInfo.GetCultureInfo("cs"))
               ?? id.ToString();
    }
}
