using System.ComponentModel.DataAnnotations;

namespace SportSys.Database.Enums;

public enum ETrainingState
{
    [Display(Name = "Plan", ResourceType = typeof(SportSys.Database.Resources.ETrainingState))]
    Plan = 1,

    [Display(Name = "ConfirmedKis", ResourceType = typeof(SportSys.Database.Resources.ETrainingState))]
    ConfirmedKis = 2,

    [Display(Name = "KisOnly", ResourceType = typeof(SportSys.Database.Resources.ETrainingState))]
    KisOnly = 3,

    [Display(Name = "TimeChanged", ResourceType = typeof(SportSys.Database.Resources.ETrainingState))]
    TimeChanged = 4,

    [Display(Name = "Cancelled", ResourceType = typeof(SportSys.Database.Resources.ETrainingState))]
    Cancelled = 5,

    [Display(Name = "DateChanged", ResourceType = typeof(SportSys.Database.Resources.ETrainingState))]
    DateChanged = 6,

    [Display(Name = "ArenaFault", ResourceType = typeof(SportSys.Database.Resources.ETrainingState))]
    ArenaFault = 7
}
