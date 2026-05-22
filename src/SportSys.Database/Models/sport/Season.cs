
#nullable enable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using SportSys.Database.Models.dboSchema;
using SportSys.Database.Models.sportSchema;


namespace SportSys.Database.Models.sportSchema;

[Table(nameof(Season), Schema = Schemas.Sport)]
public partial class Season
{
    [Key]
    public int Id { get; set; }

    [StringLength(50)]
    public required string Name { get; set; }

    public DateOnly From { get; set; }

    public DateOnly To { get; set; }

    public virtual ICollection<Match> Matches { get; set; } = new List<Match>();

    public virtual ICollection<SeasonCategory> SeasonCategories { get; set; } = new List<SeasonCategory>();

    public virtual ICollection<Training> Training { get; set; } = new List<Training>();
}