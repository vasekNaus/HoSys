using System.ComponentModel.DataAnnotations;

namespace SportSys.Database.Models.inventory;

public enum ETransactionType
{
    [Display(Name = "Purchase", ResourceType = typeof(SportSys.Database.Resources.ETransactionType))]
    Purchase = 1,

    [Display(Name = "Loan", ResourceType = typeof(SportSys.Database.Resources.ETransactionType))]
    Loan = 2,

    [Display(Name = "Return", ResourceType = typeof(SportSys.Database.Resources.ETransactionType))]
    Return = 3,

    [Display(Name = "Transfer", ResourceType = typeof(SportSys.Database.Resources.ETransactionType))]
    Transfer = 4,

    [Display(Name = "RepairStart", ResourceType = typeof(SportSys.Database.Resources.ETransactionType))]
    RepairStart = 5,

    [Display(Name = "RepairEnd", ResourceType = typeof(SportSys.Database.Resources.ETransactionType))]
    RepairEnd = 6,

    [Display(Name = "InventoryCheck", ResourceType = typeof(SportSys.Database.Resources.ETransactionType))]
    InventoryCheck = 7,

    [Display(Name = "Lost", ResourceType = typeof(SportSys.Database.Resources.ETransactionType))]
    Lost = 8,

    [Display(Name = "Dispose", ResourceType = typeof(SportSys.Database.Resources.ETransactionType))]
    Dispose = 9
}
