using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SportSys.Database.Models.Emr;

[Table("Task", Schema = "plan")]
public partial class Task
{
  [Key]
  public int Id { get; set; }

  [Column("Block_id")]
  public int? BlockId { get; set; }

  [Column("Patient_id")]
  public int PatientId { get; set; }

  public TimeOnly TimeFrom { get; set; }

  public TimeOnly TimeTo { get; set; }

  public TimeOnly? TimeArrival { get; set; }

  public string Note { get; set; } = null!;

  [Column(TypeName = "money")]
  public decimal Price { get; set; }

  [StringLength(50)]
  public string? PriceNote { get; set; }

  [Column(TypeName = "money")]
  public decimal PriceFull { get; set; }

  [StringLength(200)]
  public string? Reason { get; set; }

  [Column(TypeName = "datetime")]
  public DateTime InsertDate { get; set; }

  public bool IsActive { get; set; }

  public bool IsRearranged { get; set; }

  public bool IsEdited { get; set; }

  public short Part { get; set; }

  [Column(TypeName = "datetime")]
  public DateTime? FinalDate { get; set; }

  public bool IsExternalBonus { get; set; }

  [Column("JobDone_id")]
  public int? JobDoneId { get; set; }

  [Column(TypeName = "money")]
  public decimal? InsurancePrice { get; set; }

  public bool IsExternal { get; set; }

  [Column("InsuranceCompany_code")]
  [StringLength(3)]
  [Unicode(false)]
  public string? InsuranceCompanyCode { get; set; }

  [Column(TypeName = "decimal(12, 0)")]
  public decimal? AmbulatoryBookNumber { get; set; }

  public bool? IsIcBilled { get; set; }

  public bool? IsAnonymous { get; set; }

  [StringLength(1024)]
  public string? UrlOnlineMeeting { get; set; }

  public int? Urgency { get; set; }

  [ForeignKey("BlockId")]
  public virtual Block? Block { get; set; }

}