#nullable enable
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SportSys.Database.Models.inventory;

[Table(nameof(Location), Schema = Schemas.Inventory)]
public partial class Location
{
    [Key]
    public int Id { get; set; }

    [StringLength(200)]
    public required string Name { get; set; }

    [StringLength(500)]
    public string? Description { get; set; }

    public bool IsActive { get; set; }

    [InverseProperty(nameof(InventoryItem.AssignedLocation))]
    public virtual ICollection<InventoryItem> AssignedInventoryItems { get; set; } = new List<InventoryItem>();

    [InverseProperty(nameof(InventoryItem.CurrentLocation))]
    public virtual ICollection<InventoryItem> CurrentInventoryItems { get; set; } = new List<InventoryItem>();
}
