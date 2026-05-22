using System.ComponentModel.DataAnnotations;

namespace SportSys.Database.Enums;

public enum ETrainingPhase
{
    [Display(Name = "Preparatory", ResourceType = typeof(SportSys.Database.Resources.ETrainingPhase))]
    Preparatory = 1,

    [Display(Name = "PreCompetition", ResourceType = typeof(SportSys.Database.Resources.ETrainingPhase))]
    PreCompetition = 2,

    [Display(Name = "Competition", ResourceType = typeof(SportSys.Database.Resources.ETrainingPhase))]
    Competition = 3,

    [Display(Name = "Transitional", ResourceType = typeof(SportSys.Database.Resources.ETrainingPhase))]
    Transitional = 4
}
