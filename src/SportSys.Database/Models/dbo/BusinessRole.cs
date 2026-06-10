using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SportSys.Database.Models.dbo;

[Table(nameof(BusinessRole), Schema = Schemas.Dbo)]
[Index(nameof(Code), IsUnique = true)]
public class BusinessRole
{
  [Key]
  public int Id { get; set; }

  [StringLength(50)]
  [Unicode(false)]
  public required string Code { get; set; }

  [StringLength(100)]
  public required string Name { get; set; }

  public ICollection<BusinessRolePermission> Permissions { get; set; } = [];

  public ICollection<UserBusinessRole> Users { get; set; } = [];
}
