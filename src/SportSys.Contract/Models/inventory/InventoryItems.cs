using System.ComponentModel.DataAnnotations;

namespace SportSys.Contract.Models.inventory;

/// <summary>Řádek v přehledu výstroje (Equipment).</summary>
public class EquipmentListItem
{
    public int Id { get; set; }
    public string InventoryNumber { get; set; } = "";
    public string Name { get; set; } = "";
    public string CategoryName { get; set; } = "";
    public string? ManufacturerName { get; set; }
    public string? ItemKindName { get; set; }
    public string? Size { get; set; }
    public int ItemStatus { get; set; }
    public string StatusName { get; set; } = "";
    public bool IsActive { get; set; }
}

/// <summary>Řádek v přehledu majetku (Asset).</summary>
public class AssetListItem
{
    public int Id { get; set; }
    public string InventoryNumber { get; set; } = "";
    public string Name { get; set; } = "";
    public string CategoryName { get; set; } = "";
    public string? ManufacturerName { get; set; }
    public string? SerialNumber { get; set; }
    public DateOnly? WarrantyUntil { get; set; }
    public string? ExternalId { get; set; }
    public int ItemStatus { get; set; }
    public string StatusName { get; set; } = "";
    public bool IsActive { get; set; }
}

/// <summary>Filtr na přehledu jednoho konkrétního typu položky (bez výběru ItemType).</summary>
public class InventoryTypeFilter
{
    [Display(Name = "Hledat")]
    public string? NameFilter { get; set; }

    [Display(Name = "Kategorie")]
    public int? CategoryId { get; set; }

    [Display(Name = "Druh")]
    public int? ItemKindId { get; set; }

    [Display(Name = "Stav")]
    public int? StatusFilter { get; set; }

    [Display(Name = "Pouze aktivní")]
    public bool ActiveOnly { get; set; } = true;
}

/// <summary>Řádek v přehledu položek skladu.</summary>
public class InventoryItemListItem
{
    public int Id { get; set; }
    /// <summary>"Asset" nebo "Equipment"</summary>
    public string ItemType { get; set; } = "";
    public string ItemTypeName { get; set; } = "";
    public string InventoryNumber { get; set; } = "";
    public string Name { get; set; } = "";
    public string CategoryName { get; set; } = "";
    public string? ManufacturerName { get; set; }
    /// <summary>Druh výstroje (pouze pro Equipment; null pro Asset).</summary>
    public string? ItemKindName { get; set; }
    public int ItemStatus { get; set; }
    public string StatusName { get; set; } = "";
    public bool IsActive { get; set; }
}

/// <summary>Filtr na přehledu položek skladu.</summary>
public class InventoryItemFilter
{
    [Display(Name = "Hledat")]
    public string? NameFilter { get; set; }

    [Display(Name = "Typ")]
    public string? ItemType { get; set; }

    [Display(Name = "Kategorie")]
    public int? CategoryId { get; set; }

    [Display(Name = "Stav")]
    public int? StatusFilter { get; set; }

    [Display(Name = "Pouze aktivní")]
    public bool ActiveOnly { get; set; } = true;
}

/// <summary>Formulářový model pro vytvoření i editaci položky skladu (Asset i Equipment).</summary>
public class InventoryItemForm
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Typ položky je povinný.")]
    [Display(Name = "Typ")]
    public string ItemType { get; set; } = "Equipment";

    // ── Společné pole ──────────────────────────────────────────────────────────

    [Required(ErrorMessage = "Inventární číslo je povinné.")]
    [StringLength(20, ErrorMessage = "Inventární číslo nesmí přesáhnout 20 znaků.")]
    [Display(Name = "Inventární číslo")]
    public string? InventoryNumber { get; set; }

    [Required(ErrorMessage = "Název je povinný.")]
    [StringLength(200, ErrorMessage = "Název nesmí přesáhnout 200 znaků.")]
    [Display(Name = "Název")]
    public string? Name { get; set; }

    [Display(Name = "Popis")]
    public string? Description { get; set; }

    [Required(ErrorMessage = "Kategorie je povinná.")]
    [Display(Name = "Kategorie")]
    public int CategoryId { get; set; }

    [Display(Name = "Výrobce")]
    public int? ManufacturerId { get; set; }

    [Display(Name = "Přiřazené umístění")]
    public int? AssignedLocationId { get; set; }

    [Display(Name = "Aktuální umístění")]
    public int? CurrentLocationId { get; set; }

    [Display(Name = "Stav")]
    public int ItemStatus { get; set; } = 1;

    [Display(Name = "Datum pořízení")]
    public DateOnly? AcquisitionDate { get; set; }

    [Display(Name = "Pořizovací cena (Kč)")]
    [Range(0, 9999999.99, ErrorMessage = "Cena musí být kladné číslo.")]
    public decimal? AcquisitionPrice { get; set; }

    [Display(Name = "Aktivní")]
    public bool IsActive { get; set; } = true;

    // ── Asset — specifické pole ─────────────────────────────────────────────────

    [StringLength(100, ErrorMessage = "Sériové číslo nesmí přesáhnout 100 znaků.")]
    [Display(Name = "Sériové číslo")]
    public string? SerialNumber { get; set; }

    [Display(Name = "Záruka do")]
    public DateOnly? WarrantyUntil { get; set; }

    [StringLength(100, ErrorMessage = "Externí ID nesmí přesáhnout 100 znaků.")]
    [Display(Name = "Externí ID")]
    public string? ExternalId { get; set; }

    // ── Equipment — specifické pole ─────────────────────────────────────────────

    [Display(Name = "Druh")]
    public int? ItemKindId { get; set; }

    [StringLength(50, ErrorMessage = "Velikost nesmí přesáhnout 50 znaků.")]
    [Display(Name = "Velikost")]
    public string? Size { get; set; }
}
