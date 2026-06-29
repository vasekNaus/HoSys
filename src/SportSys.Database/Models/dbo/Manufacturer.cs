#nullable enable
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SportSys.Database.Models.dbo;

[Table(nameof(Manufacturer), Schema = Schemas.Dbo)]
public partial class Manufacturer
{
    [Key]
    public int Id { get; set; }

    [StringLength(200)]
    public required string Name { get; set; }

    [StringLength(500)]
    public string? Website { get; set; }

    public bool IsActive { get; set; }
}
