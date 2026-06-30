#nullable enable
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SportSys.Database.Models.inventory;

[Table(nameof(Equipment), Schema = Schemas.Inventory)]
public partial class Equipment : InventoryItem
{
    [StringLength(50)]
    public string? Size { get; set; }

    [Column("ItemKindId")]
    public int? ItemKindId { get; set; }

    [ForeignKey(nameof(ItemKindId))]
    [DeleteBehavior(DeleteBehavior.ClientSetNull)]
    public virtual ItemKind? ItemKind { get; set; }
}

