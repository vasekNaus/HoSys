
#nullable enable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using SportSys.Database.Models.sport;

namespace SportSys.Database.Models.dbo;

[Table(nameof(Coach), Schema = Schemas.Dbo)]
public partial class Coach
{
    [Key]
    public int Id { get; set; }

    [StringLength(50)]
    public required string FirstName { get; set; }

    [StringLength(50)]
    public required string LastName { get; set; }

    // Persisted computed column: (([FirstName]+N' ')+[LastName]) — hodnota pochází výhradně z DB.
    // Setter je private, aby se předešlo ručnímu nastavení hodnoty, která bude přepsána DB.
    // ⚠️ Při regeneraci EF Core Power Tools tuto změnu obnovte.
    [StringLength(101)]
    public string FullName { get; private set; } = null!;

    public virtual ICollection<CoachTrainingEntitlement> CoachTrainingEntitlementCoaches { get; set; } = new List<CoachTrainingEntitlement>();

    public virtual ICollection<CoachTrainingPlan> CoachTrainingPlans { get; set; } = new List<CoachTrainingPlan>();

    public virtual ICollection<CoachTraining> CoachTrainings { get; set; } = new List<CoachTraining>();
}