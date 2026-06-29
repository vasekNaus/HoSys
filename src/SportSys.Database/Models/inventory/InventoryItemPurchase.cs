#nullable enable
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SportSys.Database.Models.inventory;

[Table(nameof(InventoryItemPurchase), Schema = Schemas.Inventory)]
[Index(nameof(InventoryItemId))]
[Index(nameof(PurchaseDocumentId))]
public partial class InventoryItemPurchase
{
    [Key]
    public int Id { get; set; }

    // TPC omezení – DB FK constraint nelze vynutit
    public int InventoryItemId { get; set; }

    public int PurchaseDocumentId { get; set; }

    [Column(TypeName = "decimal(10,2)")]
    public decimal PurchasePrice { get; set; }

    [ForeignKey(nameof(PurchaseDocumentId))]
    [DeleteBehavior(DeleteBehavior.Restrict)]
    public virtual PurchaseDocument PurchaseDocument { get; set; } = null!;
}
