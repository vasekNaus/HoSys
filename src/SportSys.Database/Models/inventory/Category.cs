#nullable enable
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SportSys.Database.Models.inventory;

[Table(nameof(Category), Schema = Schemas.Inventory)]
public partial class Category
{
    [Key]
    public int Id { get; set; }

    public int? ParentCategoryId { get; set; }

    [StringLength(100)]
    public required string Name { get; set; }

    /// <summary>JSON pole povolených velikostí pro tuto kategorii, např. ["XS","S","M","L","XL"].</summary>
    public string? AvailableSizesJson { get; set; }

    public int SortOrder { get; set; }

    public bool IsActive { get; set; }

    [ForeignKey(nameof(ParentCategoryId))]
    [DeleteBehavior(DeleteBehavior.ClientSetNull)]
    public virtual Category? ParentCategory { get; set; }

    [InverseProperty(nameof(ParentCategory))]
    public virtual ICollection<Category> ChildCategories { get; set; } = new List<Category>();
}
