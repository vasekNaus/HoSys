#nullable enable
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using SportSys.Database.Models.dbo;
using SportSys.Database.Models.identity;

namespace SportSys.Database.Models.inventory;

[Table(nameof(InventoryCheck), Schema = Schemas.Inventory)]
[Index(nameof(InventorySessionId), nameof(InventoryItemId), IsUnique = true, Name = "UX_InventoryCheck_SessionItem")]
public partial class InventoryCheck
{
    [Key]
    public int Id { get; set; }

    public int InventorySessionId { get; set; }

    // TPC omezení – DB FK constraint nelze vynutit
    public int InventoryItemId { get; set; }

    public DateTime CheckedAt { get; set; }

    public int? CheckedByUserId { get; set; }

    public bool Found { get; set; }

    public int? ActualLocationId { get; set; }

    [StringLength(500)]
    public string? Note { get; set; }

    [ForeignKey(nameof(InventorySessionId))]
    [DeleteBehavior(DeleteBehavior.Cascade)]
    public virtual InventorySession InventorySession { get; set; } = null!;

    [ForeignKey(nameof(ActualLocationId))]
    [DeleteBehavior(DeleteBehavior.ClientSetNull)]
    public virtual Location? ActualLocation { get; set; }

    [ForeignKey(nameof(CheckedByUserId))]
    [DeleteBehavior(DeleteBehavior.ClientSetNull)]
    public virtual User? CheckedByUser { get; set; }
}
