using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SportSys.Database.Models.dbo;

[Table(nameof(Permission), Schema = Schemas.Dbo)]
[Index(nameof(Code), IsUnique = true)]
public class Permission
{
  [Key]
  public int Id { get; set; }

  [StringLength(50)]
  [Unicode(false)]
  public required string Code { get; set; }

  [StringLength(100)]
  public required string Name { get; set; }

  [StringLength(500)]
  public string? Description { get; set; }
}
