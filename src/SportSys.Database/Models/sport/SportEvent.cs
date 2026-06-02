#nullable enable
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using SportSys.Database.Models.sportSchema;

namespace SportSys.Database.Models.sportSchema;

public abstract class SportEvent
{
    [Key]
    public int Id { get; set; }

    public int SeasonId { get; set; }

    [StringLength(10)]
    [Unicode(false)]
    public required string SeasonCategoryName { get; set; }

    public DateOnly Date { get; set; }

    [Precision(0)]
    public TimeOnly TimeFrom { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public required string Note { get; set; }

    [ForeignKey(nameof(SeasonId))]
    [DeleteBehavior(DeleteBehavior.ClientSetNull)]
    public virtual Season Season { get; set; } = null!;

    [ForeignKey(nameof(SeasonId) + ", " + nameof(SeasonCategoryName))]
    [DeleteBehavior(DeleteBehavior.ClientSetNull)]
    public virtual SeasonCategory SeasonCategory { get; set; } = null!;
}
