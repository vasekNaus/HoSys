
#nullable enable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;
using SportSys.Database.Models.dboSchema;
using SportSys.Database.Models.sportSchema;


namespace SportSys.Database.Models.sportSchema;

[Table(nameof(IceRink), Schema = Schemas.Sport)]
public partial class IceRink
{
  [Key]
  public int Id { get; set; }

  [StringLength(100)]
  public required string Name { get; set; }

  [StringLength(200)]
  public required string Street { get; set; }

  [StringLength(100)]
  public required string City { get; set; }

  [StringLength(100)]
  public required string ZipCode { get; set; }

  public Geometry? Location { get; set; }

  public virtual ICollection<Match> Matches { get; set; } = new List<Match>();

  public virtual ICollection<Team> Teams { get; set; } = new List<Team>();
}