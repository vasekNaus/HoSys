#nullable enable
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SportSys.Database.Models.inventory;

[Table(nameof(PurchaseDocument), Schema = Schemas.Inventory)]
public partial class PurchaseDocument
{
    [Key]
    public int Id { get; set; }

    [StringLength(100)]
    public required string DocumentNumber { get; set; }

    [StringLength(200)]
    public required string SupplierName { get; set; }

    public DateOnly PurchaseDate { get; set; }

    [Column(TypeName = "decimal(10,2)")]
    public decimal TotalAmount { get; set; }

    [StringLength(500)]
    public string? Note { get; set; }

    public virtual ICollection<InventoryItemPurchase> InventoryItemPurchases { get; set; } = new List<InventoryItemPurchase>();
}
