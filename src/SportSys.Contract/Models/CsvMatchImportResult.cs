namespace SportSys.Contract.Models;

public class CsvMatchImportResult
{
    public int Inserted { get; set; }
    public int Skipped { get; set; }
    public List<string> Warnings { get; } = [];

    public void AddWarning(string message) => Warnings.Add(message);
}
