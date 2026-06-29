#nullable enable
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using SportSys.Database.Models.dbo;
using SportSys.Database.Models.identity;

namespace SportSys.Database.Models.inventory;

[Table(nameof(ItemLocationHistory), Schema = Schemas.Inventory)]
[Index(nameof(InventoryItemId), nameof(ChangedAt), Name = "IX_ItemLocationHistory_ItemDate")]
public partial class ItemLocationHistory
{
    [Key]
    public int Id { get; set; }

    // TPC omezení – DB FK constraint nelze vynutit
    public int InventoryItemId { get; set; }

    public int? PreviousLocationId { get; set; }

    public int NewLocationId { get; set; }

    public DateTime ChangedAt { get; set; }

    public int? ChangedByUserId { get; set; }

    [StringLength(500)]
    public string? Note { get; set; }

    [ForeignKey(nameof(PreviousLocationId))]
    [DeleteBehavior(DeleteBehavior.ClientSetNull)]
    public virtual Location? PreviousLocation { get; set; }

    [ForeignKey(nameof(NewLocationId))]
    [DeleteBehavior(DeleteBehavior.ClientSetNull)]
    public virtual Location NewLocation { get; set; } = null!;

    [ForeignKey(nameof(ChangedByUserId))]
    [DeleteBehavior(DeleteBehavior.ClientSetNull)]
    public virtual User? ChangedByUser { get; set; }
}
