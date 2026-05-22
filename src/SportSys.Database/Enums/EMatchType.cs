using System.ComponentModel.DataAnnotations;

namespace SportSys.Database.Enums;

public enum EMatchType
{
    [Display(Name = "League", ResourceType = typeof(SportSys.Database.Resources.EMatchType))]
    League = 1,

    [Display(Name = "Friendly", ResourceType = typeof(SportSys.Database.Resources.EMatchType))]
    Friendly = 2,

    [Display(Name = "Tournament", ResourceType = typeof(SportSys.Database.Resources.EMatchType))]
    Tournament = 3
}
