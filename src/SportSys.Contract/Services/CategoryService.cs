using Microsoft.EntityFrameworkCore;
using SportSys.Contract.Models.inventory;
using SportSys.Database.Context;
using SportSys.Database.Models.inventory;
using DbCategoryKind = SportSys.Database.Models.inventory.CategoryKind;
using ContractCategoryKind = SportSys.Contract.Models.inventory.CategoryKind;

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

        var rows = await query
            .OrderBy(c => c.ParentCategory == null ? "" : c.ParentCategory.Name)
            .ThenBy(c => c.SortOrder)
            .ThenBy(c => c.Name)
            .Select(c => new
            {
                c.Id, c.Name, c.ParentCategoryId,
                ParentCategoryName = c.ParentCategory != null ? c.ParentCategory.Name : null,
                c.SortOrder, c.IsActive, c.CategoryKinds,
            })
            .ToListAsync(ct);

        return rows.Select(c => new CategoryListItem
        {
            Id = c.Id,
            Name = c.Name,
            ParentCategoryId = c.ParentCategoryId,
            ParentCategoryName = c.ParentCategoryName,
            SortOrder = c.SortOrder,
            IsActive = c.IsActive,
            HasKinds = c.CategoryKinds is { Length: > 0 },
        }).ToList();
    }

    /// <summary>Vrátí všechny kategorie jako strom. Kořenové uzly jsou seřazeny dle SortOrder.</summary>
    public async Task<List<CategoryTreeNode>> GetTreeAsync(CancellationToken ct = default)
    {
        var rows = await _db.InventoryCategories
            .OrderBy(c => c.SortOrder)
            .ThenBy(c => c.Name)
            .Select(c => new { c.Id, c.Name, c.ParentCategoryId, c.SortOrder, c.IsActive, c.CategoryKinds })
            .ToListAsync(ct);

        var nodes = rows.Select(c => new CategoryTreeNode
        {
            Id = c.Id,
            Name = c.Name,
            ParentCategoryId = c.ParentCategoryId,
            SortOrder = c.SortOrder,
            IsActive = c.IsActive,
            HasKinds = c.CategoryKinds is { Length: > 0 },
        }).ToList();

        var lookup = nodes.ToDictionary(n => n.Id);
        var roots = new List<CategoryTreeNode>();

        foreach (var node in nodes)
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
            .Select(c => new { c.Id, c.Name, c.ParentCategoryId, c.SortOrder, c.IsActive, c.CategoryKinds })
            .FirstOrDefaultAsync(ct);

        if (cat is null) return null;

        return new CategoryModel
        {
            Id = cat.Id,
            Name = cat.Name,
            ParentCategoryId = cat.ParentCategoryId,
            SortOrder = cat.SortOrder,
            IsActive = cat.IsActive,
            CategoryKindInputs = cat.CategoryKinds?
                .Select(k => new CategoryKindInput
                {
                    ItemKind = k.ItemKind,
                    SizesText = k.Sizes.Length > 0 ? string.Join("\n", k.Sizes) : null,
                })
                .ToList() ?? [],
        };
    }

    public async Task<List<CategorySelectItem>> GetSelectListAsync(int? excludeId = null, CancellationToken ct = default)
    {
        var rows = await _db.InventoryCategories
            .Where(c => c.IsActive && (excludeId == null || c.Id != excludeId))
            .OrderBy(c => c.SortOrder)
            .ThenBy(c => c.Name)
            .Select(c => new { c.Id, c.Name, c.CategoryKinds })
            .ToListAsync(ct);

        return rows.Select(c => new CategorySelectItem
        {
            Id = c.Id,
            Name = c.Name,
            CategoryKinds = c.CategoryKinds?
                .Select(k => new ContractCategoryKind { ItemKind = k.ItemKind, Sizes = k.Sizes })
                .ToArray(),
        }).ToList();
    }

    public async Task<CategoryModel> CreateAsync(CategoryModel model, CancellationToken ct = default)
    {
        var entity = new Category
        {
            Name = model.Name!,
            ParentCategoryId = model.ParentCategoryId,
            SortOrder = model.SortOrder,
            IsActive = model.IsActive,
            CategoryKinds = MapToDbKinds(model.CategoryKindInputs),
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
        entity.CategoryKinds = MapToDbKinds(model.CategoryKindInputs);

        await _db.SaveChangesAsync(ct);
    }

    private static DbCategoryKind[]? MapToDbKinds(List<CategoryKindInput> inputs)
    {
        if (inputs.Count == 0) return null;

        var result = inputs
            .Select(k => new DbCategoryKind
            {
                ItemKind = k.ItemKind,
                Sizes = ParseSizesText(k.SizesText),
            })
            .ToArray();

        return result.Length == 0 ? null : result;
    }

    private static string[] ParseSizesText(string? text)
    {
        if (string.IsNullOrWhiteSpace(text)) return [];
        return text
            .Split('\n', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Where(s => !string.IsNullOrWhiteSpace(s))
            .ToArray();
    }
}

