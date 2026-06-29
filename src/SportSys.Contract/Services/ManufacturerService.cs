using Microsoft.EntityFrameworkCore;
using SportSys.Contract.Models.inventory;
using SportSys.Database.Context;
using DbManufacturer = SportSys.Database.Models.dbo.Manufacturer;

namespace SportSys.Contract.Services;

public class ManufacturerService
{
    private readonly SportSysDbContext _db;

    public ManufacturerService(SportSysDbContext db)
    {
        _db = db;
    }

    public async Task<List<Manufacturer>> GetAllAsync(string? nameFilter = null, CancellationToken ct = default)
    {
        var query = _db.Manufacturers.AsQueryable();

        if (!string.IsNullOrWhiteSpace(nameFilter))
            query = query.Where(m => m.Name.Contains(nameFilter));

        return await query
            .OrderBy(m => m.Name)
            .Select(m => new Manufacturer
            {
                Id = m.Id,
                Name = m.Name,
                Website = m.Website,
                IsActive = m.IsActive,
            })
            .ToListAsync(ct);
    }

    public async Task<Manufacturer?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        return await _db.Manufacturers
            .Where(m => m.Id == id)
            .Select(m => new Manufacturer
            {
                Id = m.Id,
                Name = m.Name,
                Website = m.Website,
                IsActive = m.IsActive,
            })
            .FirstOrDefaultAsync(ct);
    }

    public async Task<Manufacturer> CreateAsync(Manufacturer model, CancellationToken ct = default)
    {
        var entity = new DbManufacturer
        {
            Name = model.Name!,
            Website = model.Website,
            IsActive = model.IsActive,
        };
        _db.Manufacturers.Add(entity);
        await _db.SaveChangesAsync(ct);
        model.Id = entity.Id;
        return model;
    }

    public async Task UpdateAsync(Manufacturer model, CancellationToken ct = default)
    {
        var entity = await _db.Manufacturers.FindAsync([model.Id], ct)
            ?? throw new InvalidOperationException($"Výrobce s ID {model.Id} nebyl nalezen.");

        entity.Name = model.Name!;
        entity.Website = model.Website;
        entity.IsActive = model.IsActive;

        await _db.SaveChangesAsync(ct);
    }
}
