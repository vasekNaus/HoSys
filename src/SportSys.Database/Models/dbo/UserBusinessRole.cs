using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using SportSys.Database.Models.identity;

namespace SportSys.Database.Models.dbo;

[Table(nameof(UserBusinessRole), Schema = Schemas.Dbo)]
[PrimaryKey(nameof(UserId), nameof(BusinessRoleId))]
public class UserBusinessRole
{
  public string UserId { get; set; } = default!;

  public int BusinessRoleId { get; set; }

  [ForeignKey(nameof(UserId))]
  [DeleteBehavior(DeleteBehavior.Cascade)]
  public User User { get; set; } = default!;

  [ForeignKey(nameof(BusinessRoleId))]
  [DeleteBehavior(DeleteBehavior.Cascade)]
  public BusinessRole BusinessRole { get; set; } = default!;
}
