#nullable enable
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SportSys.Database.Models.inventory;

[Table(nameof(ItemKind), Schema = Schemas.Inventory)]
public partial class ItemKind
{
    [Key]
    public int Id { get; set; }

    [StringLength(50)]
    public required string Name { get; set; }
}
