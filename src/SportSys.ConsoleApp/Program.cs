using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using SportSys.Database.Context;

partial class Program
{
  static async Task Main(string[] args)
  {
    using var host = Host.CreateDefaultBuilder(args)
        .ConfigureAppConfiguration((context, config) =>
        {
          config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
          if (context.HostingEnvironment.IsDevelopment())
            config.AddUserSecrets<Program>(optional: true);
        })
        .ConfigureServices((context, services) =>
        {
          // Načtení connection stringu
          var connectionString = context.Configuration.GetConnectionString("DefaultConnection");

          // Registrace DbContextu
          services.AddDbContext<SportSysDbContext>(options =>
            options.UseSqlServer(connectionString, x => x.UseNetTopologySuite()));
          //services.AddHttpClient();
          // Registrace konfigurace MapyCom a HttpService jako singleton
          services.Configure<SportSys.Contract.Config.MapyCom>(context.Configuration.GetSection("MapyCom"));
          services.AddSingleton<SportSys.Contract.Services.HttpService>();
          services.AddScoped<SportSys.Contract.Services.CsvMatchImportService>();

        })
        .Build();

    //await host.Services.GetRequiredService<App>().RunAsync();

    Console.WriteLine("Hello, World!");

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

    //foreach (var file in Directory.EnumerateFiles(folderPath, "*.xlsx"))
    //{
    //  //if (file.Contains("Krsová") || file.Contains("Kuchař"))
    //  await ImportRun.ImportAsync(file, connStr);
    //}

    // Import zápasů z games.xlsx

    // Kontrola a aplikace čekajících EF Core migrací
    await using (var migrationScope = host.Services.CreateAsyncScope())
    {
      var db = migrationScope.ServiceProvider.GetRequiredService<SportSysDbContext>();
      var logger = migrationScope.ServiceProvider.GetRequiredService<ILogger<Program>>();

      var pending = (await db.Database.GetPendingMigrationsAsync()).ToList();
      if (pending.Count > 0)
      {
        logger.LogInformation("Aplikuji {Count} čekající migrace: {Migrations}",
          pending.Count, string.Join(", ", pending));
        await db.Database.MigrateAsync();
        logger.LogInformation("Migrace úspěšně aplikovány.");
      }
      else
      {
        logger.LogInformation("Databáze je aktuální, žádné migrace nečekají.");
      }
    }

    using var scope = host.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<SportSysDbContext>();
    var coaches = dbContext.Coaches.ToList();
    var matches = dbContext.Matches.ToList();
    var trainings = dbContext.Training.ToList();
    //var http = host.Services.GetRequiredService<IHttpClientFactory>().CreateClient();
    //var gamesFile = @"e:\Data\vasek.naus@outlook.cz\OneDrive\Hokej\Výbor\Dokumenty\Source\SportSys\src\SportSys.ConsoleApp\games.xlsx";
    //await MatchImportRun.ImportAsync(gamesFile, db, http);
    //*/
    var httpService = host.Services.GetRequiredService<SportSys.Contract.Services.HttpService>();
   //var result = await httpService.Search("Zimní station, Domažlice");

    /*
    // Import zápasů z hokejového svazu (CSV)
    // SeasonCategory záznamy musí mít nastavený CompetitionCode odpovídající hodnotám sloupce Soutěž v CSV.
    var importService = scope.ServiceProvider.GetRequiredService<SportSys.Contract.Services.CsvMatchImportService>();
    var csvFile = @"e:\Data\vasek.naus@outlook.cz\OneDrive\Hokej\Výbor\Dokumenty\Source\SportSys\src\Data\Zapasy.csv";
    const int seasonId = 1;   // TODO: nastavit Id sezóny
    var importResult = await importService.ImportAsync(csvFile, seasonId);
    Console.WriteLine($"Import dokončen: vloženo {importResult.Inserted}, přeskočeno {importResult.Skipped}");
    foreach (var warning in importResult.Warnings)
        Console.WriteLine($"  [varování] {warning}");
    */


    Console.WriteLine("Done");
    Console.ReadLine();
  }
}






