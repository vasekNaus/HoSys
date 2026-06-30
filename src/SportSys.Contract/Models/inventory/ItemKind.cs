using SportSys.Database.Models.inventory;

namespace SportSys.Contract.Models.inventory;

/// <summary>Druh výstroje s definicí povolených velikostí pro danou kategorii.</summary>
public class CategoryKind
{
    public EItemKind ItemKind { get; set; }
    public string[] Sizes { get; set; } = [];
}

/// <summary>Formulářový vstup pro jeden řádek kind→velikosti v editaci kategorie.</summary>
public class CategoryKindInput
{
    public EItemKind ItemKind { get; set; }
    /// <summary>Velikosti jako víceřádkový text (jeden řádek = jedna velikost).</summary>
    public string? SizesText { get; set; }
}

/// <summary>Položka číselníku druhů pro select dropdown.</summary>
public class ItemKindListItem
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public EItemKind Kind { get; set; }
}
