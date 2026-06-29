#nullable enable
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SportSys.Database.Models.inventory;

[Table(nameof(Asset), Schema = Schemas.Inventory)]
public partial class Asset : InventoryItem
{
    [StringLength(100)]
    public string? SerialNumber { get; set; }

    public DateOnly? WarrantyUntil { get; set; }

    [StringLength(100)]
    public string? ExternalId { get; set; }
}
