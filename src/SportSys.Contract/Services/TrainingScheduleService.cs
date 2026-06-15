using Microsoft.EntityFrameworkCore;
using SportSys.Contract.Models;
using SportSys.Database.Context;

namespace SportSys.Contract.Services;

public class TrainingScheduleService
{
    private readonly SportSysDbContext _db;

    public TrainingScheduleService(SportSysDbContext db)
    {
        _db = db;
    }

    public async Task<List<SeasonDto>> GetSeasonsAsync(CancellationToken ct = default)
    {
        return await _db.Seasons
            .OrderByDescending(s => s.From)
            .Select(s => new SeasonDto { Id = s.Id, Name = s.Name })
            .ToListAsync(ct);
    }

    public async Task<List<SeasonCategoryDto>> GetCategoriesAsync(int seasonId, CancellationToken ct = default)
    {
        return await _db.SeasonCategories
            .Where(c => c.SeasonId == seasonId)
            .OrderBy(c => c.Order)
            .Select(c => new SeasonCategoryDto
            {
                SeasonId = c.SeasonId,
                Name = c.Name,
                Order = c.Order,
            })
            .ToListAsync(ct);
    }

    public async Task<List<TrainingScheduleItemDto>> GetTrainingsAsync(
        int seasonId,
        IReadOnlyCollection<string> categoryNames,
        DateOnly dateFrom,
        DateOnly dateTo,
        CancellationToken ct = default)
    {
        return await _db.Training
            .Where(t => t.SeasonId == seasonId
                && categoryNames.Contains(t.SeasonCategoryName)
                && t.Date >= dateFrom
                && t.Date <= dateTo)
            .OrderBy(t => t.Date)
            .ThenBy(t => t.TimeFrom)
            .Select(t => new TrainingScheduleItemDto
            {
                Id = t.Id,
                Date = t.Date,
                TimeFrom = t.TimeFrom,
                TimeTo = t.TimeTo,
                DurationMinutes = t.DurationMinutes,
                SeasonCategoryName = t.SeasonCategoryName,
                Location = t.Location,
                TrainingTypeName = t.TrainingType.Name,
                Note = t.Note,
            })
            .ToListAsync(ct);
    }
}
