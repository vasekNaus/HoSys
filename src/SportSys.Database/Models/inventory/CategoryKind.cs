namespace SportSys.Database.Models.inventory;

/// <summary>
/// Value object serializovaný do JSON sloupce Category.CategoryKindJson.
/// Definuje povolené velikosti pro konkrétní druh výstroje v dané kategorii.
/// </summary>
public class CategoryKind
{
    public EItemKind ItemKind { get; set; }
    public string[] Sizes { get; set; } = [];
}
