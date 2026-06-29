using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using SportSys.Contract.Models.inventory;
using SportSys.Database.Context;
using SportSys.Database.Models.inventory;

namespace SportSys.Contract.Services;

public class CategoryService
{
    private readonly SportSysDbContext _db;

    public CategoryService(SportSysDbContext db)
    {
        _db = db;
    }

    public async Task<List<CategoryListItem>> GetAllAsync(string? nameFilter = null, CancellationToken ct = default)
    {
        var query = _db.InventoryCategories.AsQueryable();

        if (!string.IsNullOrWhiteSpace(nameFilter))
            query = query.Where(c => c.Name.Contains(nameFilter));

        return await query
            .OrderBy(c => c.ParentCategory == null ? "" : c.ParentCategory.Name)
            .ThenBy(c => c.SortOrder)
            .ThenBy(c => c.Name)
            .Select(c => new CategoryListItem
            {
                Id = c.Id,
                Name = c.Name,
                ParentCategoryId = c.ParentCategoryId,
                ParentCategoryName = c.ParentCategory != null ? c.ParentCategory.Name : null,
                SortOrder = c.SortOrder,
                IsActive = c.IsActive,
                HasSizes = c.AvailableSizesJson != null && c.AvailableSizesJson != "[]",
            })
            .ToListAsync(ct);
    }

    /// <summary>
    /// Vrátí všechny kategorie jako strom. Kořenové uzly (bez rodiče) jsou seřazeny dle SortOrder.
    /// </summary>
    public async Task<List<CategoryTreeNode>> GetTreeAsync(CancellationToken ct = default)
    {
        var all = await _db.InventoryCategories
            .OrderBy(c => c.SortOrder)
            .ThenBy(c => c.Name)
            .Select(c => new CategoryTreeNode
            {
                Id = c.Id,
                Name = c.Name,
                ParentCategoryId = c.ParentCategoryId,
                SortOrder = c.SortOrder,
                IsActive = c.IsActive,
                HasSizes = c.AvailableSizesJson != null && c.AvailableSizesJson != "[]",
            })
            .ToListAsync(ct);

        // Sestavení stromu in-memory
        var lookup = all.ToDictionary(n => n.Id);
        var roots = new List<CategoryTreeNode>();

        foreach (var node in all)
        {
            if (node.ParentCategoryId is null)
                roots.Add(node);
            else if (lookup.TryGetValue(node.ParentCategoryId.Value, out var parent))
                parent.Children.Add(node);
        }

        return roots;
    }

    public async Task<CategoryModel?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var cat = await _db.InventoryCategories
            .Where(c => c.Id == id)
            .Select(c => new { c.Id, c.Name, c.ParentCategoryId, c.SortOrder, c.IsActive, c.AvailableSizesJson })
            .FirstOrDefaultAsync(ct);

        if (cat is null) return null;

        return new CategoryModel
        {
            Id = cat.Id,
            Name = cat.Name,
            ParentCategoryId = cat.ParentCategoryId,
            SortOrder = cat.SortOrder,
            IsActive = cat.IsActive,
            AvailableSizesText = JsonToText(cat.AvailableSizesJson),
        };
    }

    public async Task<List<CategorySelectItem>> GetSelectListAsync(int? excludeId = null, CancellationToken ct = default)
    {
        return await _db.InventoryCategories
            .Where(c => c.IsActive && (excludeId == null || c.Id != excludeId))
            .OrderBy(c => c.SortOrder)
            .ThenBy(c => c.Name)
            .Select(c => new CategorySelectItem
            {
                Id = c.Id,
                Name = c.Name,
                AvailableSizes = c.AvailableSizesJson != null
                    ? JsonSerializer.Deserialize<string[]>(c.AvailableSizesJson)!
                    : Array.Empty<string>(),
            })
            .ToListAsync(ct);
    }

    public async Task<CategoryModel> CreateAsync(CategoryModel model, CancellationToken ct = default)
    {
        var entity = new Category
        {
            Name = model.Name!,
            ParentCategoryId = model.ParentCategoryId,
            SortOrder = model.SortOrder,
            IsActive = model.IsActive,
            AvailableSizesJson = TextToJson(model.AvailableSizesText),
        };
        _db.InventoryCategories.Add(entity);
        await _db.SaveChangesAsync(ct);
        model.Id = entity.Id;
        return model;
    }

    public async Task UpdateAsync(CategoryModel model, CancellationToken ct = default)
    {
        var entity = await _db.InventoryCategories.FindAsync([model.Id], ct)
            ?? throw new InvalidOperationException($"Kategorie s ID {model.Id} nebyla nalezena.");

        entity.Name = model.Name!;
        entity.ParentCategoryId = model.ParentCategoryId;
        entity.SortOrder = model.SortOrder;
        entity.IsActive = model.IsActive;
        entity.AvailableSizesJson = TextToJson(model.AvailableSizesText);

        await _db.SaveChangesAsync(ct);
    }

    /// <summary>Převede víceřádkový text (jeden řádek = jedna velikost) na JSON pole.</summary>
    private static string? TextToJson(string? text)
    {
        if (string.IsNullOrWhiteSpace(text)) return null;

        var sizes = text
            .Split('\n', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Where(s => !string.IsNullOrWhiteSpace(s))
            .ToArray();

        return sizes.Length == 0 ? null : JsonSerializer.Serialize(sizes);
    }

    /// <summary>Převede JSON pole velikostí na víceřádkový text pro editaci.</summary>
    private static string? JsonToText(string? json)
    {
        if (string.IsNullOrWhiteSpace(json)) return null;
        try
        {
            var sizes = JsonSerializer.Deserialize<string[]>(json);
            return sizes is { Length: > 0 } ? string.Join("\n", sizes) : null;
        }
        catch
        {
            return null;
        }
    }
}
