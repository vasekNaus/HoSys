using System.ComponentModel.DataAnnotations;

namespace SportSys.Database.Enums;

public enum ETrainingType
{
    [Display(Name = "Dry", ResourceType = typeof(SportSys.Database.Resources.ETrainingType))]
    Dry = 1,

    [Display(Name = "Ice", ResourceType = typeof(SportSys.Database.Resources.ETrainingType))]
    Ice = 2
}
