# SportSys

Informační systém pro sportovní organizaci. Slouží jako nadstavba nad existujícími systémy (rezervační systém sportoviště, účetnictví) a poskytuje funkce, které tyto systémy samostatně nepokrývají – zejména kontrolu fakturace, evidenci sportovních událostí a automatizaci plateb.

## Technologie

| Vrstva | Technologie |
|---|---|
| Backend | ASP.NET Core (.NET 10), Razor Pages + Blazor Server |
| Databáze | Microsoft SQL Server 2019+, Entity Framework Core 10 |
| ORM / migrace | EF Core 10 – TPC dědičnost, sdílené DB sekvence, `dotnet ef` |
| Frontend | HTML, CSS (vlastní, bez frameworků), Vanilla JS |
| API | ASP.NET Core Minimal API (selektivně) |
| Import dat | ExcelDataReader (`.xlsx` → SQL Server) |

## Předpoklady

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- Microsoft SQL Server 2019+ (nebo SQL Server Express / LocalDB)
- Visual Studio 2022+ nebo VS Code s rozšířením C#
- EF Core CLI: `dotnet tool install --global dotnet-ef`

## Rychlý start

```bash
# 1. Klonovat repozitář
git clone <url>

# 2. Nastavit connection string v appsettings.json
# "ConnectionStrings": { "DefaultConnection": "Server=.;Database=SportSys;Trusted_Connection=True;" }

# 3. Aplikovat databázové migrace (EF Core CLI)
dotnet ef database update --project src/SportSys.Database

# 4. Spustit aplikaci
dotnet run --project src/SportSys.Web
```

> Migrace automaticky vytvoří schéma včetně sekvence `SportEventSeq` sdílené mezi `Training` a `Match` (TPC vzor).

## Struktura projektu

```
src/
  SportSys.Database/      # EF Core modely, DbContext, migrace
    Models/
      Emr/                # Read-only modely napojené na ext. rezervační systém
    Context/              # SportSysDbContext
    Migrations/
  SportSys.Web/           # ASP.NET Core – Razor Pages / Blazor Server (připravováno)
  SportSys.ConsoleApp/    # Pomocné CLI nástroje (import dat z Excelu)
  DB Model/               # SQL DDL skripty (záložní zdroj schématu)
docs/                     # Projektová dokumentace
```

## Dokumentace

| Dokument | Obsah |
|---|---|
| [docs/overview.md](docs/overview.md) | Účel systému, kontext, integrace |
| [docs/architecture.md](docs/architecture.md) | Technická architektura, DB design, vrstvy |
| [docs/features.md](docs/features.md) | Přehled funkcí dle modulů |
| [docs/use-cases.md](docs/use-cases.md) | Scénáře použití |

## Licence

Interní projekt – není určen k veřejnému šíření.
