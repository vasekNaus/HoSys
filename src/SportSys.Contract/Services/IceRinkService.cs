using Microsoft.EntityFrameworkCore;
using SportSys.Contract.Models;
using SportSys.Database.Context;
using SportSys.Database.Models.sport;

namespace SportSys.Contract.Services;

public class IceRinkService
{
    private readonly SportSysDbContext _db;

    public IceRinkService(SportSysDbContext db)
    {
        _db = db;
    }

    public async Task<List<IceRinkDto>> GetAllAsync(
        string? search = null,
        bool? isActive = true,
        CancellationToken ct = default)
    {
        var query = _db.IceRinks.AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
            query = query.Where(r => r.Name.Contains(search) || r.City.Contains(search));

        if (isActive.HasValue)
            query = query.Where(r => r.IsActive == isActive.Value);

        return await query
            .OrderBy(r => r.City)
            .ThenBy(r => r.Name)
            .Select(r => new IceRinkDto
            {
                Id = r.Id,
                Name = r.Name,
                Street = r.Street,
                City = r.City,
                ZipCode = r.ZipCode,
                IsActive = r.IsActive,
            })
            .ToListAsync(ct);
    }

    public async Task<IceRinkDto?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        return await _db.IceRinks
            .Where(r => r.Id == id)
            .Select(r => new IceRinkDto
            {
                Id = r.Id,
                Name = r.Name,
                Street = r.Street,
                City = r.City,
                ZipCode = r.ZipCode,
                IsActive = r.IsActive,
            })
            .FirstOrDefaultAsync(ct);
    }

    public async Task<List<LookupSelectItem>> GetSelectListAsync(
        int? includeId = null,
        CancellationToken ct = default)
    {
        return await _db.IceRinks
            .Where(r => r.IsActive || r.Id == includeId)
            .OrderBy(r => r.City)
            .ThenBy(r => r.Name)
            .Select(r => new LookupSelectItem
            {
                Id = r.Id,
                Name = r.City + " – " + r.Name,
            })
            .ToListAsync(ct);
    }

    public async Task<IceRinkDto> CreateAsync(IceRinkDto dto, CancellationToken ct = default)
    {
        var entity = new IceRink
        {
            Name = dto.Name!,
            Street = dto.Street!,
            City = dto.City!,
            ZipCode = dto.ZipCode!,
            IsActive = dto.IsActive,
        };
        _db.IceRinks.Add(entity);
        await _db.SaveChangesAsync(ct);
        dto.Id = entity.Id;
        return dto;
    }

    public async Task UpdateAsync(IceRinkDto dto, CancellationToken ct = default)
    {
        var entity = await _db.IceRinks.FindAsync([dto.Id], ct)
            ?? throw new InvalidOperationException($"Zimní stadion s ID {dto.Id} nebyl nalezen.");

        entity.Name = dto.Name!;
        entity.Street = dto.Street!;
        entity.City = dto.City!;
        entity.ZipCode = dto.ZipCode!;
        entity.IsActive = dto.IsActive;

        await _db.SaveChangesAsync(ct);
    }

    public async Task SetActiveAsync(int id, bool isActive, CancellationToken ct = default)
    {
        var entity = await _db.IceRinks.FindAsync([id], ct)
            ?? throw new InvalidOperationException($"Zimní stadion s ID {id} nebyl nalezen.");

        entity.IsActive = isActive;
        await _db.SaveChangesAsync(ct);
    }
}
