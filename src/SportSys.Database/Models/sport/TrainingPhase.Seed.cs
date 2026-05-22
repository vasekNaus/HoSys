using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using SportSys.Database.Enums;

namespace SportSys.Database.Models.sportSchema;

public partial class TrainingPhase
{
    private TrainingPhase() { Name = null!; }

    [SetsRequiredMembers]
    public TrainingPhase(ETrainingPhase id)
    {
        Id   = (int)id;
        Name = Resources.ETrainingPhase.ResourceManager
                   .GetString(id.ToString(), CultureInfo.GetCultureInfo("cs"))
               ?? id.ToString();
    }
}
