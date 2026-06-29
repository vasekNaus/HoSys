using System.ComponentModel.DataAnnotations;

namespace SportSys.Database.Models.inventory;

public enum EItemStatus
{
    [Display(Name = "InStock", ResourceType = typeof(SportSys.Database.Resources.EItemStatus))]
    InStock = 1,

    [Display(Name = "Assigned", ResourceType = typeof(SportSys.Database.Resources.EItemStatus))]
    Assigned = 2,

    [Display(Name = "Borrowed", ResourceType = typeof(SportSys.Database.Resources.EItemStatus))]
    Borrowed = 3,

    [Display(Name = "InRepair", ResourceType = typeof(SportSys.Database.Resources.EItemStatus))]
    InRepair = 4,

    [Display(Name = "Lost", ResourceType = typeof(SportSys.Database.Resources.EItemStatus))]
    Lost = 5,

    [Display(Name = "Disposed", ResourceType = typeof(SportSys.Database.Resources.EItemStatus))]
    Disposed = 6
}
