using System.ComponentModel.DataAnnotations;

namespace SportSys.Contract.Models.inventory;

public class CategoryModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Název je povinný.")]
    [StringLength(100, ErrorMessage = "Název nesmí přesáhnout 100 znaků.")]
    [Display(Name = "Název")]
    public string? Name { get; set; }

    [Display(Name = "Nadřazená kategorie")]
    public int? ParentCategoryId { get; set; }

    [Display(Name = "Pořadí")]
    public int SortOrder { get; set; }

    [Display(Name = "Aktivní")]
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Druhy výstroje a jejich povolené velikosti pro tuto kategorii.
    /// Prázdné pole = kategorie nemá druhy ani velikosti.
    /// </summary>
    [Display(Name = "Druhy a velikosti")]
    public List<CategoryKindInput> CategoryKindInputs { get; set; } = [];
}

public class CategoryListItem
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public int? ParentCategoryId { get; set; }
    public string? ParentCategoryName { get; set; }
    public int SortOrder { get; set; }
    public bool IsActive { get; set; }
    public bool HasKinds { get; set; }
}

public class CategoryTreeNode
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public int? ParentCategoryId { get; set; }
    public int SortOrder { get; set; }
    public bool IsActive { get; set; }
    public bool HasKinds { get; set; }
    public List<CategoryTreeNode> Children { get; set; } = [];
}

public class CategorySelectItem
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    /// <summary>Druhy výstroje a jejich velikosti; null = kategorie nemá druhy.</summary>
    public CategoryKind[]? CategoryKinds { get; set; }
}

