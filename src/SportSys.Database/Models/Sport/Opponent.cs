
#nullable enable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using SportSys.Database.Models.dboSchema;
using SportSys.Database.Models.sportSchema;


namespace SportSys.Database.Models.sportSchema;

[Table(nameof(Opponent), Schema = Schemas.Sport)]
public partial class Opponent
{
    [Key]
    public int Id { get; set; }

    [StringLength(100)]
    public required string Name { get; set; }

    [StringLength(200)]
    public required string Address { get; set; }

    [StringLength(100)]
    public required string City { get; set; }

    public int? HomeIceRinkId { get; set; }

    public virtual IceRink? HomeIceRink { get; set; }

    public virtual ICollection<Match> Matches { get; set; } = new List<Match>();
}