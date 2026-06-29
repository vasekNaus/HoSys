#nullable enable
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using SportSys.Database.Models.identity;

namespace SportSys.Database.Models.inventory;

[Table(nameof(Loan), Schema = Schemas.Inventory)]
[Index(nameof(InventoryItemId))]
[Index(nameof(MemberId))]
public partial class Loan
{
    [Key]
    public int Id { get; set; }

    // Odkazuje na Equipment nebo Asset – DB FK constraint nelze vynutit (TPC omezení)
    public int InventoryItemId { get; set; }

    public int MemberId { get; set; }

    public DateOnly LoanDate { get; set; }

    public DateOnly? ExpectedReturnDate { get; set; }

    public DateOnly? ReturnedDate { get; set; }

    [StringLength(500)]
    public string? Note { get; set; }

    public bool IsClosed { get; set; }

    [ForeignKey(nameof(MemberId))]
    [DeleteBehavior(DeleteBehavior.ClientSetNull)]
    public virtual User Member { get; set; } = null!;
}
