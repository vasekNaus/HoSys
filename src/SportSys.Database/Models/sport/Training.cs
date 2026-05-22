
#nullable enable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using SportSys.Database.Models.dboSchema;
using SportSys.Database.Models.sportSchema;


namespace SportSys.Database.Models.sportSchema;

[Table(nameof(Training), Schema = Schemas.Sport)]
public partial class Training
{
  [Key]
  public int Id { get; set; }

  public int SeasonId { get; set; }

  [StringLength(10)]
  [Unicode(false)]
  public required string SeasonCategoryName { get; set; }

  public int TrainingTypeId { get; set; }

  public int TrainingPhaseId { get; set; }

  public int TrainingStateId { get; set; }

  public int? TrainingPlanId { get; set; }

  public int IceRinkId { get; set; }

  [StringLength(100)]
  [Unicode(false)]
  public required string Location { get; set; }

  [Precision(0)]
  public TimeOnly TimeFrom { get; set; }

  [Precision(0)]
  public TimeOnly TimeTo { get; set; }

  public DateOnly Date { get; set; }

  public int? DurationMinutes { get; set; }

  [StringLength(50)]
  [Unicode(false)]
  public required string Note { get; set; }

  [ForeignKey(nameof(SeasonId))]
  [DeleteBehavior(DeleteBehavior.ClientSetNull)]
  public virtual Season Season { get; set; } = null!;

  [ForeignKey(nameof(SeasonId) + ", " + nameof(SeasonCategoryName))]
  [DeleteBehavior(DeleteBehavior.ClientSetNull)]
  public virtual SeasonCategory SeasonCategory { get; set; } = null!;

  [DeleteBehavior(DeleteBehavior.ClientSetNull)]
  public virtual IceRink IceRink { get; set; } = null!;

  [DeleteBehavior(DeleteBehavior.ClientSetNull)]
  public virtual TrainingPhase TrainingPhase { get; set; } = null!;

  public virtual TrainingPlan? TrainingPlan { get; set; }

  [DeleteBehavior(DeleteBehavior.ClientSetNull)]
  public virtual TrainingState TrainingState { get; set; } = null!;

  [DeleteBehavior(DeleteBehavior.ClientSetNull)]
  public virtual TrainingType TrainingType { get; set; } = null!;

  public virtual ICollection<CoachTraining> CoachTrainings { get; set; } = new List<CoachTraining>();
}