using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SportSys.Database.Models.sport;

[Table(nameof(TrainingPlanGroup), Schema = Schemas.Sport)]
[PrimaryKey(nameof(GroupId), nameof(TrainingPlanId))]
[Index(nameof(TrainingPlanId), IsUnique = true, Name = "UX_TrainingPlanGroup_TrainingPlanId")]
public class TrainingPlanGroup
{
    public Guid GroupId { get; set; }

    public int TrainingPlanId { get; set; }

    [ForeignKey(nameof(TrainingPlanId))]
    [DeleteBehavior(DeleteBehavior.Cascade)]
    public virtual TrainingPlan TrainingPlan { get; set; } = null!;
}
