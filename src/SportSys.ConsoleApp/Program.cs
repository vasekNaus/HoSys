using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using SportSys.Database.Context;

partial class Program
{
  static async Task Main(string[] args)
  {
    using var host = Host.CreateDefaultBuilder(args)
        .ConfigureAppConfiguration(config =>
        {
          config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
        })
        .ConfigureServices((context, services) =>
        {
          // Načtení connection stringu
          var connectionString = context.Configuration.GetConnectionString("DefaultConnection");

          // Registrace DbContextu
          services.AddDbContext<SportSysDbContext>(options => options.UseSqlServer(connectionString));

        })
        .Build();

    //await host.Services.GetRequiredService<App>().RunAsync();

    Console.WriteLine("Hello, World!");


    //vytáhnu si službu databáze
    //var dbContext = host.Services.GetRequiredService<SportSysDbContext>();

    //DateTime now = DateTime.Now;
    //var timeOnlyNow = TimeOnly.FromDateTime(now);

    //var taskts = dbContext.Tasks.Where(x => true).Select(x => new
    //{
    //  a = DateTime.Now,
    //  b = x.Block!.Date.ToDateTime(TimeOnly.MinValue),
    //  diff = EF.Functions.DateDiffMinute(x.TimeFrom, x.TimeTo),  
    //  diff2 = EF.Functions.DateDiffMinute(now, x.Block!.Date.ToDateTime(TimeOnly.MinValue)),  //x.Block!.Date.ToDateTime(TimeOnly.MinValue)
    //  diff3 = EF.Functions.DateDiffMinute(timeOnlyNow, x.TimeFrom)  //x.Block!.Date.ToDateTime(TimeOnly.MinValue)
    //}).ToList();

    var folderPath = @"e:\Data\vasek.naus@outlook.cz\OneDrive\Hokej\Výbor\Dokumenty\PowerBI\202603";
    var connStr = "Server=.\\SQL2022;Database=Hockey;Trusted_Connection=True;TrustServerCertificate=True;";

    foreach (var file in Directory.EnumerateFiles(folderPath, "*.xlsx"))
    {
      //if (file.Contains("Krsová") || file.Contains("Kuchař"))
      await ImportRun.ImportAsync(file, connStr);
    }
    Console.WriteLine("Done");
    Console.ReadLine();
  }
}






