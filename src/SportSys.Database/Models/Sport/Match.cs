using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SportSys.Database.Models.Sport;

[Table("Match", Schema = "sport")]
public class Match
{
  [Key]
  [DatabaseGenerated(DatabaseGeneratedOption.None)] // Id je NEXT VALUE FOR SportEventSeq, ne IDENTITY
  public int Id { get; set; }

  [Column("Season_Id")]
  public int SeasonId { get; set; }

  [Required]
  [MaxLength(10)]
  [Column("SeasonCategory_Name")]
  public string SeasonCategoryName { get; set; } = string.Empty;

  [Column("IceRink_Id")]
  public int IceRinkId { get; set; }

  public DateOnly Date { get; set; }

  public TimeOnly TimeFrom { get; set; }

  public TimeOnly TimeTo { get; set; }

  [MaxLength(50)]
  public string Note { get; set; } = string.Empty;

  [MaxLength(10)]
  public string? MatchCode { get; set; }

  [Column("Opponent_Id")]
  public int OpponentId { get; set; }

  public bool IsHome { get; set; }

  public byte? GoalsScored { get; set; }

  public byte? GoalsConceded { get; set; }

  [Column("MatchType_Id")]
  public int MatchTypeId { get; set; }
}
