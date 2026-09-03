using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SportSys.Database.Models.sport;

[Table(nameof(TrainingGroup), Schema = Schemas.Sport)]
[PrimaryKey(nameof(GroupId), nameof(TrainingId))]
[Index(nameof(TrainingId), IsUnique = true, Name = "UX_TrainingGroup_TrainingId")]
public class TrainingGroup
{
    public Guid GroupId { get; set; }

    public int TrainingId { get; set; }

    [ForeignKey(nameof(TrainingId))]
    [DeleteBehavior(DeleteBehavior.Cascade)]
    public virtual Training Training { get; set; } = null!;
}
