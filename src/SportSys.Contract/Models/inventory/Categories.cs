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
    /// Povolené velikosti jako textový vstup (jeden řádek = jedna velikost).
    /// Při čtení z DB se JSON převede na řádky; při ukládání se řádky převedou zpět na JSON.
    /// </summary>
    [Display(Name = "Povolené velikosti")]
    public string? AvailableSizesText { get; set; }
}

public class CategoryListItem
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public int? ParentCategoryId { get; set; }
    public string? ParentCategoryName { get; set; }
    public int SortOrder { get; set; }
    public bool IsActive { get; set; }
    public bool HasSizes { get; set; }
}

public class CategoryTreeNode
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public int? ParentCategoryId { get; set; }
    public int SortOrder { get; set; }
    public bool IsActive { get; set; }
    public bool HasSizes { get; set; }
    public List<CategoryTreeNode> Children { get; set; } = [];
}

public class CategorySelectItem
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string[] AvailableSizes { get; set; } = [];
}
