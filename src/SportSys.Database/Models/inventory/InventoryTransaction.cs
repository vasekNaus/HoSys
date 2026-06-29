#nullable enable
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using SportSys.Database.Models.identity;


namespace SportSys.Database.Models.inventory;

[Table(nameof(InventoryTransaction), Schema = Schemas.Inventory)]
[Index(nameof(InventoryItemId), nameof(TransactionDate), Name = "IX_InventoryTransaction_ItemDate")]
public partial class InventoryTransaction
{
    [Key]
    public int Id { get; set; }

    // TPC omezení – DB FK constraint nelze vynutit
    public int InventoryItemId { get; set; }

    public int TransactionTypeId { get; set; }

    public DateTime TransactionDate { get; set; }

    public int Quantity { get; set; }

    public int? UserId { get; set; }

    [StringLength(500)]
    public string? Note { get; set; }

    [ForeignKey(nameof(TransactionTypeId))]
    [DeleteBehavior(DeleteBehavior.ClientSetNull)]
    public virtual TransactionType TransactionType { get; set; } = null!;

    [ForeignKey(nameof(UserId))]
    [DeleteBehavior(DeleteBehavior.ClientSetNull)]
    public virtual User? User { get; set; }
}
