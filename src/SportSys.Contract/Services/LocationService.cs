using Microsoft.EntityFrameworkCore;
using SportSys.Contract.Models.inventory;
using SportSys.Database.Context;
using DbLocation = SportSys.Database.Models.dbo.Location;

namespace SportSys.Contract.Services;

public class LocationService
{
    private readonly SportSysDbContext _db;

    public LocationService(SportSysDbContext db)
    {
        _db = db;
    }

    public async Task<List<LocationListItem>> GetAllAsync(string? nameFilter = null, CancellationToken ct = default)
    {
        var query = _db.Locations.AsQueryable();

        if (!string.IsNullOrWhiteSpace(nameFilter))
            query = query.Where(l => l.Name.Contains(nameFilter));

        return await query
            .OrderBy(l => l.ParentLocation == null ? "" : l.ParentLocation.Name)
            .ThenBy(l => l.Name)
            .Select(l => new LocationListItem
            {
                Id = l.Id,
                Name = l.Name,
                ParentLocationName = l.ParentLocation != null ? l.ParentLocation.Name : null,
                IsActive = l.IsActive,
            })
            .ToListAsync(ct);
    }

    public async Task<Location?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        return await _db.Locations
            .Where(l => l.Id == id)
            .Select(l => new Location
            {
                Id = l.Id,
                Name = l.Name,
                Description = l.Description,
                ParentLocationId = l.ParentLocationId,
                IsActive = l.IsActive,
            })
            .FirstOrDefaultAsync(ct);
    }

    public async Task<List<LocationSelectItem>> GetSelectListAsync(int? excludeId = null, CancellationToken ct = default)
    {
        return await _db.Locations
            .Where(l => l.IsActive && (excludeId == null || l.Id != excludeId))
            .OrderBy(l => l.Name)
            .Select(l => new LocationSelectItem
            {
                Id = l.Id,
                Name = l.Name,
            })
            .ToListAsync(ct);
    }

    public async Task<Location> CreateAsync(Location model, CancellationToken ct = default)
    {
        var entity = new DbLocation
        {
            Name = model.Name!,
            Description = model.Description,
            ParentLocationId = model.ParentLocationId,
            IsActive = model.IsActive,
        };
        _db.Locations.Add(entity);
        await _db.SaveChangesAsync(ct);
        model.Id = entity.Id;
        return model;
    }

    public async Task UpdateAsync(Location model, CancellationToken ct = default)
    {
        var entity = await _db.Locations.FindAsync([model.Id], ct)
            ?? throw new InvalidOperationException($"Umístění s ID {model.Id} nebylo nalezeno.");

        entity.Name = model.Name!;
        entity.Description = model.Description;
        entity.ParentLocationId = model.ParentLocationId;
        entity.IsActive = model.IsActive;

        await _db.SaveChangesAsync(ct);
    }
}
