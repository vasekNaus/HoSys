using System.ComponentModel.DataAnnotations;

namespace SportSys.Database.Enums;

public enum ECoachRole
{
    [Display(Name = "Head", ResourceType = typeof(SportSys.Database.Resources.ECoachRole))]
    Head = 1,

    [Display(Name = "Assistant", ResourceType = typeof(SportSys.Database.Resources.ECoachRole))]
    Assistant = 2,

    [Display(Name = "Defenders", ResourceType = typeof(SportSys.Database.Resources.ECoachRole))]
    Defenders = 3,

    [Display(Name = "Goalkeepers", ResourceType = typeof(SportSys.Database.Resources.ECoachRole))]
    Goalkeepers = 4,

    [Display(Name = "Physiotherapist", ResourceType = typeof(SportSys.Database.Resources.ECoachRole))]
    Physiotherapist = 5,

    [Display(Name = "Conditioning", ResourceType = typeof(SportSys.Database.Resources.ECoachRole))]
    Conditioning = 6
}
