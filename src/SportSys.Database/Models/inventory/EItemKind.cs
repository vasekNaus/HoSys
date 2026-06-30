using System.ComponentModel.DataAnnotations;

namespace SportSys.Database.Models.inventory;

public enum EItemKind
{
    [Display(Name = "Children", ResourceType = typeof(SportSys.Database.Resources.EItemKind))]
    Children = 1,

    [Display(Name = "Youth", ResourceType = typeof(SportSys.Database.Resources.EItemKind))]
    Youth = 2,

    [Display(Name = "Junior", ResourceType = typeof(SportSys.Database.Resources.EItemKind))]
    Junior = 3,

    [Display(Name = "Senior", ResourceType = typeof(SportSys.Database.Resources.EItemKind))]
    Senior = 4,

    [Display(Name = "Women", ResourceType = typeof(SportSys.Database.Resources.EItemKind))]
    Women = 5,

    [Display(Name = "Men", ResourceType = typeof(SportSys.Database.Resources.EItemKind))]
    Men = 6,

    [Display(Name = "Unisex", ResourceType = typeof(SportSys.Database.Resources.EItemKind))]
    Unisex = 7,
}
