#nullable enable
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using SportSys.Database.Models.dbo;



namespace SportSys.Database.Models.inventory;

public abstract class InventoryItem
{
    [Key]
    public int Id { get; set; }

    [StringLength(20)]
    [Unicode(false)]
    public required string InventoryNumber { get; set; }

    [StringLength(200)]
    public required string Name { get; set; }

    public string? Description { get; set; }

    public int CategoryId { get; set; }

    public int? ManufacturerId { get; set; }

    public int? AssignedLocationId { get; set; }

    public int? CurrentLocationId { get; set; }

    // Hodnota EItemStatus uložena jako int
    public int ItemStatus { get; set; }

    public DateOnly? AcquisitionDate { get; set; }

    [Column(TypeName = "decimal(10,2)")]
    public decimal? AcquisitionPrice { get; set; }

    [StringLength(500)]
    [Unicode(false)]
    public string? QRCodeValue { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public int? CreatedByUserId { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public int? ModifiedByUserId { get; set; }

    [ForeignKey(nameof(CategoryId))]
    [DeleteBehavior(DeleteBehavior.ClientSetNull)]
    public virtual Category Category { get; set; } = null!;

    [ForeignKey(nameof(ManufacturerId))]
    [DeleteBehavior(DeleteBehavior.ClientSetNull)]
    public virtual Manufacturer? Manufacturer { get; set; }

    [ForeignKey(nameof(AssignedLocationId))]
    [DeleteBehavior(DeleteBehavior.ClientSetNull)]
    [InverseProperty(nameof(Location.AssignedInventoryItems))]
    public virtual Location? AssignedLocation { get; set; }

    [ForeignKey(nameof(CurrentLocationId))]
    [DeleteBehavior(DeleteBehavior.ClientSetNull)]
    [InverseProperty(nameof(Location.CurrentInventoryItems))]
    public virtual Location? CurrentLocation { get; set; }
}
