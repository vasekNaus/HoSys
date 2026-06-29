#nullable enable
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using SportSys.Database.Models.inventory;

namespace SportSys.Database.Models.dbo;

[Table(nameof(Location), Schema = Schemas.Dbo)]
public partial class Location
{
    [Key]
    public int Id { get; set; }

    public int? ParentLocationId { get; set; }

    [StringLength(200)]
    public required string Name { get; set; }

    [StringLength(500)]
    public string? Description { get; set; }

    public bool IsActive { get; set; }

    [ForeignKey(nameof(ParentLocationId))]
    [DeleteBehavior(DeleteBehavior.ClientSetNull)]
    public virtual Location? ParentLocation { get; set; }

    [InverseProperty(nameof(ParentLocation))]
    public virtual ICollection<Location> ChildLocations { get; set; } = new List<Location>();

    [InverseProperty(nameof(InventoryItem.AssignedLocation))]
    public virtual ICollection<InventoryItem> AssignedInventoryItems { get; set; } = new List<InventoryItem>();

    [InverseProperty(nameof(InventoryItem.CurrentLocation))]
    public virtual ICollection<InventoryItem> CurrentInventoryItems { get; set; } = new List<InventoryItem>();
}
