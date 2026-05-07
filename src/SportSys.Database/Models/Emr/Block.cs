using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SportSys.Database.Models.Emr;

[Table("Block", Schema = "plan")]
public partial class Block
{
  [Key]
  public int Id { get; set; }

  [Column("Room_id")]
  public int RoomId { get; set; }

  [StringLength(100)]
  public string Name { get; set; } = null!;

  public DateOnly Date { get; set; }

  public TimeOnly TimeFrom { get; set; }

  public TimeOnly TimeTo { get; set; }

  [Column(TypeName = "datetime")]
  public DateTime? InsertDate { get; set; }

  public string Note { get; set; } = null!;

  public bool EnableReservation { get; set; }

  public bool IsActive { get; set; }

  public bool IsLocked { get; set; }

  [StringLength(100)]
  public string? LockReason { get; set; }

  [Column(TypeName = "datetime")]
  public DateTime? LockTime { get; set; }

  [Column(TypeName = "datetime")]
  public DateTime? UnlockTime { get; set; }

  public short? MaxReservations { get; set; }

  [Column(TypeName = "datetime")]
  public DateTime? ConfirmDate { get; set; }

  public bool? AllowExternalReservations { get; set; }

  public bool? AllowAnonymousReservations { get; set; }

  public bool? AllowExternalUserReservations { get; set; }

  public bool IsHiddenForSearch { get; set; }

  public bool IsShuttleOnly { get; set; }

  [StringLength(7)]
  [Unicode(false)]
  public string? NoteColor { get; set; }

  //  [InverseProperty("Block")]
  public virtual ICollection<Task> Tasks { get; set; } = new List<Task>();

}