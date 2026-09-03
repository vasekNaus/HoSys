using SportSys.Contract.Models;
using System.Globalization;

namespace SportSys.Razor.Models.TrainingSchedule;

public class TrainingScheduleComponentModel
{
    private readonly double _totalTimelineMinutes;

    private TrainingScheduleComponentModel(ITrainingScheduleViewModel source)
    {
        CategoryColors = source.CategoryColors;
        TimelineStart = source.TimelineStart;
        TimelineEnd = source.TimelineEnd;
        _totalTimelineMinutes =
            (TimelineEnd.ToTimeSpan() - TimelineStart.ToTimeSpan()).TotalMinutes;

        Markers = CreateMarkers();
        Rows = source.Rows
            .Select(row => new TrainingScheduleComponentRow
            {
                PrimaryLabel = row.PrimaryLabel,
                SecondaryLabel = row.SecondaryLabel,
                Parity = row.Parity,
                IsWeekend = row.IsWeekend,
                Lanes = CreateLanes(row.Items),
            })
            .ToList();
    }

    public IReadOnlyDictionary<string, string> CategoryColors { get; }
    public TimeOnly TimelineStart { get; }
    public TimeOnly TimelineEnd { get; }
    public IReadOnlyList<TrainingScheduleMarker> Markers { get; }
    public IReadOnlyList<TrainingScheduleComponentRow> Rows { get; }

    public static TrainingScheduleComponentModel Create(ITrainingScheduleViewModel source)
        => new(source);

    private IReadOnlyList<TrainingScheduleMarker> CreateMarkers()
    {
        var markers = new List<TrainingScheduleMarker>();
        var startHour = TimelineStart.Hour;
        var endMinutes = TimelineEnd.ToTimeSpan().TotalMinutes;

        for (var hour = startHour; hour * 60 <= endMinutes; hour += 2)
        {
            var time = new TimeOnly(hour, 0);
            markers.Add(new TrainingScheduleMarker
            {
                Label = $"{hour}:00",
                Left = GetLeft(time),
            });
        }

        return markers;
    }

    private IReadOnlyList<IReadOnlyList<TrainingScheduleBlock>> CreateLanes(
        IReadOnlyList<ITrainingScheduleItem> items)
    {
        var lanes = new List<List<TrainingScheduleBlock>>();

        foreach (var item in items.OrderBy(i => i.TimeFrom).ThenBy(i => i.TimeTo))
        {
            var block = new TrainingScheduleBlock
            {
                Item = item,
                Left = GetLeft(item.TimeFrom),
                Width = GetWidth(item.TimeFrom, item.TimeTo),
                Color = CategoryColors.TryGetValue(item.SeasonCategoryName, out var color)
                    ? color
                    : "var(--color-text-muted)",
                Tooltip = CreateTooltip(item),
            };

            var lane = lanes.FirstOrDefault(existing =>
                existing.Count == 0 || existing[^1].Item.TimeTo <= item.TimeFrom);

            if (lane is null)
            {
                lane = [];
                lanes.Add(lane);
            }

            lane.Add(block);
        }

        return lanes;
    }

    private double GetLeft(TimeOnly time)
    {
        var offset = (time.ToTimeSpan() - TimelineStart.ToTimeSpan()).TotalMinutes;
        return Math.Clamp(offset / _totalTimelineMinutes * 100, 0, 100);
    }

    private double GetWidth(TimeOnly timeFrom, TimeOnly timeTo)
    {
        var duration = (timeTo.ToTimeSpan() - timeFrom.ToTimeSpan()).TotalMinutes;
        return Math.Clamp(duration / _totalTimelineMinutes * 100, 0.5, 100);
    }

    private static string CreateTooltip(ITrainingScheduleItem item)
    {
        var parts = new List<string>
        {
            item.SeasonCategoryName,
            item.TrainingTypeName,
            item.TrainingPhaseName,
            item.Location,
        };

        if (item is TrainingPlanScheduleItemDto plan &&
            item is not TrainingScheduleItemDto)
        {
            parts.Add(
                $"Platnost {plan.From.ToString("d. M. yyyy", CultureInfo.CurrentCulture)}–" +
                plan.To.ToString("d. M. yyyy", CultureInfo.CurrentCulture));
        }

        if (!string.IsNullOrWhiteSpace(item.Note))
            parts.Add(item.Note);

        return string.Join(" · ", parts);
    }
}

public class TrainingScheduleComponentRow
{
    public required string PrimaryLabel { get; init; }
    public string? SecondaryLabel { get; init; }
    public required TrainingScheduleRowParity Parity { get; init; }
    public bool IsWeekend { get; init; }
    public IReadOnlyList<IReadOnlyList<TrainingScheduleBlock>> Lanes { get; init; } = [];
}

public class TrainingScheduleBlock
{
    public required ITrainingScheduleItem Item { get; init; }
    public required string Color { get; init; }
    public required string Tooltip { get; init; }
    public double Left { get; init; }
    public double Width { get; init; }
}

public class TrainingScheduleMarker
{
    public required string Label { get; init; }
    public double Left { get; init; }
}
