#nullable enable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using SportSys.Database.Models.dboSchema;
using SportSys.Database.Models.sportSchema;

namespace SportSys.Database.Models.sportSchema;

[Table(nameof(Match), Schema = Schemas.Sport)]
public partial class Match
{
    [Key]
    public int Id { get; set; }

    public int SeasonId { get; set; }

    [StringLength(10)]
    [Unicode(false)]
    public required string SeasonCategoryName { get; set; }

    public int IceRinkId { get; set; }

    public DateOnly Date { get; set; }

    [Precision(0)]
    public TimeOnly TimeFrom { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public required string Note { get; set; }

    [StringLength(10)]
    [Unicode(false)]
    public string? MatchCode { get; set; }

    public int HomeTeamId { get; set; }

    public int AwayTeamId { get; set; }

    public MatchResult? Result { get; set; }

    public int MatchTypeId { get; set; }

    [DeleteBehavior(DeleteBehavior.ClientSetNull)]
    public virtual IceRink IceRink { get; set; } = null!;

    [ForeignKey(nameof(HomeTeamId))]
    [DeleteBehavior(DeleteBehavior.ClientSetNull)]
    public virtual Team HomeTeam { get; set; } = null!;

    [ForeignKey(nameof(AwayTeamId))]
    [DeleteBehavior(DeleteBehavior.ClientSetNull)]
    public virtual Team AwayTeam { get; set; } = null!;

    [DeleteBehavior(DeleteBehavior.ClientSetNull)]
    public virtual MatchType MatchType { get; set; } = null!;

    [ForeignKey(nameof(SeasonId))]
    [DeleteBehavior(DeleteBehavior.ClientSetNull)]
    public virtual Season Season { get; set; } = null!;

    [ForeignKey(nameof(SeasonId) + ", " + nameof(SeasonCategoryName))]
    [DeleteBehavior(DeleteBehavior.ClientSetNull)]
    public virtual SeasonCategory SeasonCategory { get; set; } = null!;
}
