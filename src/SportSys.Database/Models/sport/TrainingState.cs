
#nullable enable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using SportSys.Database.Models.dboSchema;
using SportSys.Database.Models.sportSchema;


namespace SportSys.Database.Models.sportSchema;

[Table(nameof(TrainingState), Schema = Schemas.Sport)]
public partial class TrainingState
{
    [Key]
    public int Id { get; set; }

    [StringLength(50)]
    public required string Name { get; set; }

    public virtual ICollection<Training> Training { get; set; } = new List<Training>();
}