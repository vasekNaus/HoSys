using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SportSys.Contract.Models;
using SportSys.Contract.Services;
using System.Globalization;

namespace SportSys.Razor.Areas.sport.Pages.Schedule;

public class IndexModel : PageModel
{
    private static readonly string[] CategoryColors =
    [
        "#3b82f6", // modrá
        "#22c55e", // zelená
        "#f59e0b", // žlutá
        "#ef4444", // červená
        "#8b5cf6", // fialová
        "#06b6d4", // azurová
        "#ec4899", // růžová
        "#84cc16", // limetková
    ];

    public static readonly TimeOnly TimelineStart = new(6, 0);
    public static readonly TimeOnly TimelineEnd   = new(22, 0);

    private static readonly double TotalTimelineMinutes =
        (TimelineEnd.ToTimeSpan() - TimelineStart.ToTimeSpan()).TotalMinutes;

    private readonly TrainingScheduleService _service;

    public IndexModel(TrainingScheduleService service)
    {
        _service = service;
    }

    // ── Data pro filtry ──────────────────────────────────────────
    public List<SeasonDto> Seasons { get; set; } = [];
    public List<SeasonCategoryDto> SeasonCategories { get; set; } = [];

    // ── Filtr (GET parametry) ────────────────────────────────────
    [BindProperty(SupportsGet = true)]
    public int? SeasonId { get; set; }

    [BindProperty(SupportsGet = true)]
    public List<string> SelectedCategories { get; set; } = [];

    [BindProperty(SupportsGet = true)]
    public DateOnly? DateFrom { get; set; }

    [BindProperty(SupportsGet = true)]
    public DateOnly? DateTo { get; set; }

    // ── Výsledná data ────────────────────────────────────────────
    public bool HasResults { get; private set; }
    public List<DateOnly> DateRange { get; private set; } = [];
    public Dictionary<DateOnly, List<TrainingScheduleItemDto>> Schedule { get; private set; } = [];
    public Dictionary<string, string> CategoryColorMap { get; private set; } = [];

    // ── Pomocné markery časové osy ───────────────────────────────
    public IEnumerable<(string Label, double LeftPct)> TimelineMarkers()
    {
        for (var h = TimelineStart.Hour; h <= TimelineEnd.Hour; h += 2)
        {
            var t = new TimeOnly(h, 0);
            yield return ($"{h}:00", GetLeft(t));
        }
    }

    public async Task OnGetAsync(CancellationToken ct)
    {
        Seasons = await _service.GetSeasonsAsync(ct);

        if (SeasonId.HasValue)
        {
            SeasonCategories = await _service.GetCategoriesAsync(SeasonId.Value, ct);
        }

        if (SeasonId.HasValue && SelectedCategories.Count > 0
            && DateFrom.HasValue && DateTo.HasValue
            && DateFrom <= DateTo)
        {
            var trainings = await _service.GetTrainingsAsync(
                SeasonId.Value, SelectedCategories, DateFrom.Value, DateTo.Value, ct);

            // Barvy kategorií zachovají pořadí z filtru
            for (var i = 0; i < SelectedCategories.Count; i++)
                CategoryColorMap[SelectedCategories[i]] = CategoryColors[i % CategoryColors.Length];

            // Vyplň celý rozsah dat (i dny bez tréninků)
            for (var d = DateFrom.Value; d <= DateTo.Value; d = d.AddDays(1))
                DateRange.Add(d);

            Schedule = trainings.GroupBy(t => t.Date)
                                .ToDictionary(g => g.Key, g => g.ToList());
            HasResults = true;
        }
    }

    // ── Helpery pro timeline ─────────────────────────────────────
    public double GetLeft(TimeOnly time)
    {
        var offset = (time.ToTimeSpan() - TimelineStart.ToTimeSpan()).TotalMinutes;
        return Math.Clamp(offset / TotalTimelineMinutes * 100, 0, 100);
    }

    public double GetWidth(TimeOnly timeFrom, TimeOnly timeTo)
    {
        var duration = (timeTo.ToTimeSpan() - timeFrom.ToTimeSpan()).TotalMinutes;
        return Math.Clamp(duration / TotalTimelineMinutes * 100, 0.5, 100);
    }

    public string GetCategoryColor(string categoryName)
        => CategoryColorMap.TryGetValue(categoryName, out var c) ? c : "#6c757d";

    public static string FormatDate(DateOnly d)
        => d.ToString("d.M.", CultureInfo.InvariantCulture);

    public static string FormatDow(DateOnly d)
        => d.DayOfWeek switch
        {
            DayOfWeek.Monday    => "Po",
            DayOfWeek.Tuesday   => "Út",
            DayOfWeek.Wednesday => "St",
            DayOfWeek.Thursday  => "Čt",
            DayOfWeek.Friday    => "Pá",
            DayOfWeek.Saturday  => "So",
            DayOfWeek.Sunday    => "Ne",
            _ => "",
        };

    public static bool IsWeekend(DateOnly d)
        => d.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday;
}
