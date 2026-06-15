namespace SportSys.Contract.Models;

public class SeasonDto
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
}

public class SeasonCategoryDto
{
    public int SeasonId { get; set; }
    public string Name { get; set; } = "";
    public int Order { get; set; }
}

public class TrainingScheduleItemDto
{
    public int Id { get; set; }
    public DateOnly Date { get; set; }
    public TimeOnly TimeFrom { get; set; }
    public TimeOnly TimeTo { get; set; }
    public int? DurationMinutes { get; set; }
    public string SeasonCategoryName { get; set; } = "";
    public string Location { get; set; } = "";
    public string TrainingTypeName { get; set; } = "";
    public string Note { get; set; } = "";
}
