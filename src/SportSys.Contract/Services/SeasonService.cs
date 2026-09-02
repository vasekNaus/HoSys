using Microsoft.EntityFrameworkCore;
using SportSys.Contract.Models;
using SportSys.Database.Context;
using DbSeason = SportSys.Database.Models.sport.Season;

namespace SportSys.Contract.Services;

public class SeasonService
{
    private readonly SportSysDbContext _db;

    public SeasonService(SportSysDbContext db)
    {
        _db = db;
    }

    public async Task<List<SeasonListItem>> GetAllAsync(
        string? search = null,
        bool? isActive = true,
        CancellationToken ct = default)
    {
        var query = _db.Seasons.AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
            query = query.Where(s => s.Name.Contains(search));

        if (isActive.HasValue)
            query = query.Where(s => s.IsActive == isActive.Value);

        return await query
            .OrderByDescending(s => s.From)
            .Select(s => new SeasonListItem
            {
                Id = s.Id,
                Name = s.Name,
                From = s.From,
                To = s.To,
                IsActive = s.IsActive,
            })
            .ToListAsync(ct);
    }

    public async Task<SeasonEditDto?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        return await _db.Seasons
            .Where(s => s.Id == id)
            .Select(s => new SeasonEditDto
            {
                Id = s.Id,
                Name = s.Name,
                From = s.From,
                To = s.To,
                IsActive = s.IsActive,
            })
            .FirstOrDefaultAsync(ct);
    }

    public async Task<List<LookupSelectItem>> GetSelectListAsync(
        int? includeId = null,
        CancellationToken ct = default)
    {
        return await _db.Seasons
            .Where(s => s.IsActive || s.Id == includeId)
            .OrderByDescending(s => s.From)
            .Select(s => new LookupSelectItem
            {
                Id = s.Id,
                Name = s.Name,
            })
            .ToListAsync(ct);
    }

    public async Task<SeasonEditDto> CreateAsync(SeasonEditDto dto, CancellationToken ct = default)
    {
        var entity = new DbSeason
        {
            Name = dto.Name!,
            From = dto.From,
            To = dto.To,
            IsActive = dto.IsActive,
        };

        _db.Seasons.Add(entity);
        await _db.SaveChangesAsync(ct);
        dto.Id = entity.Id;
        return dto;
    }

    public async Task UpdateAsync(SeasonEditDto dto, CancellationToken ct = default)
    {
        var entity = await _db.Seasons.FindAsync([dto.Id], ct)
            ?? throw new InvalidOperationException($"Sezóna s ID {dto.Id} nebyla nalezena.");

        entity.Name = dto.Name!;
        entity.From = dto.From;
        entity.To = dto.To;
        entity.IsActive = dto.IsActive;

        await _db.SaveChangesAsync(ct);
    }

    public async Task SetActiveAsync(int id, bool isActive, CancellationToken ct = default)
    {
        var entity = await _db.Seasons.FindAsync([id], ct)
            ?? throw new InvalidOperationException($"Sezóna s ID {id} nebyla nalezena.");

        entity.IsActive = isActive;
        await _db.SaveChangesAsync(ct);
    }
}
