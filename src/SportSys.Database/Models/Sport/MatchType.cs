
#nullable enable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using SportSys.Database.Models.dboSchema;
using SportSys.Database.Models.sportSchema;


namespace SportSys.Database.Models.sportSchema;

[Table(nameof(MatchType), Schema = Schemas.Sport)]
public partial class MatchType
{
    [Key]
    public int Id { get; set; }

    [StringLength(100)]
    public required string Name { get; set; }

    public virtual ICollection<Match> Matches { get; set; } = new List<Match>();
}