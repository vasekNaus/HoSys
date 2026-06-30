using Microsoft.EntityFrameworkCore;
using SportSys.Contract.Models.inventory;
using SportSys.Database.Context;
using SportSys.Database.Models.inventory;

namespace SportSys.Contract.Services;

public class ItemKindService
{
    private readonly SportSysDbContext _db;

    public ItemKindService(SportSysDbContext db)
    {
        _db = db;
    }

    public async Task<List<ItemKindListItem>> GetAllAsync(CancellationToken ct = default)
    {
        return await _db.InventoryItemKinds
            .OrderBy(k => k.Id)
            .Select(k => new ItemKindListItem
            {
                Id = k.Id,
                Name = k.Name,
                Kind = (EItemKind)k.Id,
            })
            .ToListAsync(ct);
    }
}
