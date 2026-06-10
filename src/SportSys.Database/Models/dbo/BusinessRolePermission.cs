using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SportSys.Database.Models.dbo;

[Table(nameof(BusinessRolePermission), Schema = Schemas.Dbo)]
[PrimaryKey(nameof(BusinessRoleId), nameof(PermissionId))]
public class BusinessRolePermission
{
  public int BusinessRoleId { get; set; }

  public int PermissionId { get; set; }

  [ForeignKey(nameof(BusinessRoleId))]
  [DeleteBehavior(DeleteBehavior.Cascade)]
  public BusinessRole BusinessRole { get; set; } = default!;

  [ForeignKey(nameof(PermissionId))]
  [DeleteBehavior(DeleteBehavior.Cascade)]
  public Permission Permission { get; set; } = default!;
}
