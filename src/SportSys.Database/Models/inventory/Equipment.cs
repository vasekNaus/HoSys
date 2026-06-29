#nullable enable
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SportSys.Database.Models.inventory;

[Table(nameof(Equipment), Schema = Schemas.Inventory)]
public partial class Equipment : InventoryItem
{
    [StringLength(50)]
    public string? Size { get; set; }
}
