#nullable enable
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SportSys.Database.Models.inventory;

[Table(nameof(InventorySession), Schema = Schemas.Inventory)]
public partial class InventorySession
{
    [Key]
    public int Id { get; set; }

    [StringLength(200)]
    public required string Name { get; set; }

    public DateTime StartedAt { get; set; }

    public DateTime? FinishedAt { get; set; }

    public bool IsClosed { get; set; }

    public virtual ICollection<InventoryCheck> InventoryChecks { get; set; } = new List<InventoryCheck>();
}
