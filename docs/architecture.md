# Architektura systému

## Přehled

SportSys je vícevrstvá webová aplikace postavená na .NET 10. Skládá se z několika projektů se striktně oddělenými odpovědnostmi.

```
┌─────────────────────────────────────────────────────┐
│  SportSys.Web  (Razor Pages / Blazor Server)        │
│  – UI, API endpointy                                │
└──────────────────────┬──────────────────────────────┘
                       │ závisí na
┌──────────────────────▼──────────────────────────────┐
│  SportSys.Database                                  │
│  – EF Core modely, DbContext, migrace               │
│  – Models.Emr  (read-only napojení na ext. systém)  │
└──────────────────────┬──────────────────────────────┘
                       │ MSSQL
┌──────────────────────▼──────────────────────────────┐
│  SQL Server                                         │
│  – databáze SportSys  (vlastní schéma dbo)          │
│  – databáze externího systému  (schéma plan)        │
└─────────────────────────────────────────────────────┘
```

## Projekty

| Projekt | Typ | Popis |
|---|---|---|
| `SportSys.Database` | Class Library | EF Core modely, `SportSysDbContext`, migrace |
| `SportSys.Web` | ASP.NET Core Web App | Razor Pages / Blazor Server, REST API (připravováno) |
| `SportSys.ConsoleApp` | Console App | Pomocné CLI nástroje – import dat z Excelu |

## Databázový model

### Vlastní schéma (`dbo`)

Systém používá vzor **TPC (Table Per Concrete type)** pro abstraktní entitu `SportEvent`. Fyzická tabulka `SportEvent` neexistuje; místo ní jsou konkrétní tabulky `Training` a `Match`, jejichž identifikátory sdílejí **společnou sekvenci** `dbo.SportEventSeq`. Výsledkem jsou ID jedinečná napříč oběma entitami.

```
SportEventSeq  (SEQUENCE – sdílené ID pro Training i Match)
       │
       ├── Training
       │     Season_Id, SeasonCategory_Name
       │     TrainingType_Id, TrainingPhase_Id, TrainingState_Id
       │     IceRink_Id, Date, TimeFrom, TimeTo, DurationMinutes (persisted)
       │
       └── Match
             Season_Id, SeasonCategory_Name
             IceRink_Id, Opponent_Id, MatchState_Id
             Date, TimeFrom, TimeTo, DurationMinutes (persisted)
             IsHome, GoalsScored, GoalsConceded, MatchCode

VIEW dbo.SportEvent  →  UNION ALL Training + Match  (EventType: 'Training' / 'Match')

IceRink      (zimní stadiony / sportoviště)
Opponent     (soupeři; HomeIceRink_Id → IceRink)
```

`DurationMinutes` je **persisted computed column** (`DATEDIFF(minute, TimeFrom, TimeTo)`), není třeba jej počítat v aplikační vrstvě.

### Integrační modely – ext. rezervační systém (namespace `Emr`)

Modely v `SportSys.Database/Models/Emr/` jsou mapovány na schéma `plan` externího systému. Jde o **read-only** přístup – SportSys do těchto tabulek nezapisuje.

| Model | Tabulka | Popis |
|---|---|---|
| `Block` | `plan.Block` | Blok rezervovaného ledového času (místnost, čas, omezení rezervací) |
| `Task` | `plan.Task` | Konkrétní rezervace/obsazení v rámci bloku |

## Frontendová vrstva

- **Razor Pages / Blazor Server** – serverem renderované stránky, žádný SPA framework.
- **CSS** – vlastní styly, bez Bootstrapu ani jiných CSS frameworků. Využívány CSS custom properties a flexbox/grid.
- **JavaScript** – vanilla JS (bez jQuery). Interaktivita tam, kde ji nelze vyřešit na serveru; jinak preferovat Blazor komponenty nebo server-side logiku.

## API vrstva

REST API se implementuje selektivně – pouze tam, kde je to odůvodněné (např. asynchronní operace v UI, budoucí integrace). Používá **ASP.NET Core Minimal API** nebo Controller-based API dle složitosti endpointu.

Konvence:
- Endpointy v cestě `/api/v1/...`
- JSON request/response
- HTTP status kódy dle sémantiky (200, 201, 400, 404, 409...)
- Autentizace řešena na úrovni ASP.NET Core middleware (cookie auth / JWT – TBD)

## Konfigurace

Connection stringy a citlivé hodnoty jsou uloženy v `appsettings.json` (lokálně) a nesmí být commitovány do repozitáře. Pro produkci se použijí environment variables nebo Azure Key Vault.

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.;Database=SportSys;Trusted_Connection=True;",
    "ExternalSystem":    "Server=.;Database=ExternalDb;Trusted_Connection=True;"
  }
}
```

## Databázové migrace

Migrace jsou spravovány přes EF Core CLI (`dotnet ef`). DDL skripty ve složce `src/DB Model/` slouží jako záloha a referenční zdroj pro přehled schématu.

```bash
# Přidat novou migraci
dotnet ef migrations add <NazevMigrace> --project src/SportSys.Database

# Aplikovat migrace
dotnet ef database update --project src/SportSys.Database
```
