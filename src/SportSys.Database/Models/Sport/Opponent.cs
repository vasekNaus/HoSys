using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SportSys.Database.Models.Sport;

[Table("Opponent", Schema = "sport")]
public class Opponent
{
  [Key]
  public int Id { get; set; }

  [Required]
  [MaxLength(100)]
  public string Name { get; set; } = string.Empty;

  [Required]
  [MaxLength(200)]
  public string Address { get; set; } = string.Empty;

  [Required]
  [MaxLength(100)]
  public string City { get; set; } = string.Empty;

  [Column("HomeIceRink_Id")]
  public int? HomeIceRinkId { get; set; }
}
