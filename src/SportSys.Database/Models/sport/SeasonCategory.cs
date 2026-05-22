
#nullable enable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using SportSys.Database.Models.dboSchema;
using SportSys.Database.Models.sportSchema;


namespace SportSys.Database.Models.sportSchema;

[PrimaryKey("SeasonId", "Name")]
[Table(nameof(SeasonCategory), Schema = Schemas.Sport)]
public partial class SeasonCategory
{
    [Key]
    public int SeasonId { get; set; }

    [Key]
    [StringLength(10)]
    [Unicode(false)]
    public required string Name { get; set; }

    public int Order { get; set; }

    [StringLength(4000)]
    public required string BirthYears { get; set; }

    public virtual ICollection<Match> Matches { get; set; } = new List<Match>();

    [ForeignKey(nameof(SeasonId))]
    [DeleteBehavior(DeleteBehavior.ClientSetNull)]
    public virtual Season Season { get; set; } = null!;

    public virtual ICollection<Training> Training { get; set; } = new List<Training>();

    public virtual ICollection<TrainingEntitlement> TrainingEntitlements { get; set; } = new List<TrainingEntitlement>();

    public virtual ICollection<TrainingPlan> TrainingPlans { get; set; } = new List<TrainingPlan>();
}