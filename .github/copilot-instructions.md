# Copilot Instructions – SportSys

SportSys je informační systém pro hokejový klub postavený na .NET 10. Jde o nadstavbu nad externím rezervačním systémem sportoviště a zajišťuje kontrolu fakturace ledového času, evidenci tréninků a zápasů a automatizaci plateb.

## Build & spuštění

```bash
# Sestavení celé solution
dotnet build SportSys.slnx

# Spuštění webové aplikace
dotnet run --project src/SportSys.Razor

# Spuštění konzolové aplikace (import dat)
dotnet run --project src/SportSys.ConsoleApp

# EF Core migrace
dotnet ef migrations add <NazevMigrace> --project src/SportSys.Database
dotnet ef database update --project src/SportSys.Database
dotnet ef migrations list --project src/SportSys.Database
```

CSS se kompiluje ze SCSS pomocí npm (sass):

```bash
cd src/SportSys.Razor
npm run build:css   # jednorázová kompilace
npm run watch:css   # sledování změn
```

Konfigurační soubory (`appsettings.json`) nejsou commitovány – connection stringy se nastavují lokálně nebo přes user secrets:

```bash
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=.;Database=SportSys;..."  --project src/SportSys.Razor
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=.;Database=SportSys;..."  --project src/SportSys.ConsoleApp
```

## Architektura projektu

Solution používá nový formát `.slnx`. Vrstvová závislost: **Razor → Contract → Database → SQL Server**

| Projekt | Typ | Role |
|---|---|---|
| `SportSys.Database` | Class Library | EF Core modely, DbContext, migrace, konfigurace |
| `SportSys.Model` | Class Library | Doménové objekty a DTO sdílené napříč vrstvami |
| `SportSys.Contract` | Class Library | Aplikační servisy; závisí na Database, vrací Model objekty |
| `SportSys.Razor` | ASP.NET Core Web App | Razor Pages; závisí **výhradně** na Contract |
| `SportSys.ConsoleApp` | Console App | Import dat z Excelu (trenéři, zápasy) do SQL Serveru |
| `src/Apollo/` | Git submodul | Interní sdílená knihovna (HttpService, ModelBuilder extensions, atd.) |

**Klíčové pravidlo:** `SportSys.Razor` nesmí přímo referencovat `SportSys.Database`. Veškerý přístup k databázi jde přes injektované servisy z `SportSys.Contract`.

Registrace servisů v `Program.cs` (Razor):
```csharp
builder.Services.AddSportSysServices(builder.Configuration);
```
`AddSportSysServices` je extension metoda v `SportSys.Contract/ServiceCollectionExtensions.cs` – registruje `SportSysDbContext`, ASP.NET Core Identity, `EntraClaimsTransformation`, authorization policies a všechny Contract servisy. **Nikdy neregistrovat tyto věci přímo v Razor projektu.**

## Databázový model

### SportSysDbContext

Jediný DbContext – ručně psaný. Konfigurační soubory patří do `Configurations/dbo/`, `Configurations/sport/`, `Configurations/identity/` a `Configurations/inventory/`. Modely jsou v `Models/dbo/`, `Models/sport/`, `Models/identity/`, `Models/inventory/` s namespace `dbo` / `sportSchema` / `identity` / `inventory`.

### Apollo model builder konvence

`SportSysDbContext.OnModelCreating` volá dvě Apollo extension metody:

- `modelBuilder.IdConvention()` – přejmenuje FK sloupce na konvenci `{Entita}Id`. Přeskočí **zděděné** FK z TPC bázové třídy — pro sdílené FK vlastnosti (`SeasonId`, `SeasonCategoryName`) nastav `HasColumnName` explicitně v `SportEventConfiguration`.
- `modelBuilder.InitDatetime2()` – nastaví výchozí mapování `DateTime` → `datetime2`.

Odstraněné konvence v DbContext:
- `ForeignKeyIndexConvention` – EF Core **nevytváří automatické indexy na FK**; přidávej indexy jen explicitně.
- `TableNameFromDbSetConvention` – název tabulky se bere z `[Table]` atributu, ne z `DbSet<T>` property. **Každý model musí mít `[Table(nameof(X), Schema = Schemas.Y)]`** — bez atributu EF Core tabulku nenajde.

### TPC dědičnost a sdílená sekvence

`Training` a `Match` jsou betonové tabulky bez společného rodiče v DB. Sdílejí sekvenci `sport.SportEventSeq`:

```csharp
modelBuilder.HasSequence<int>("SportEventSeq", Schemas.Sport).StartsAt(1).IncrementsBy(1);
```

EF Core TPC **nepodporuje** pojmenované DEFAULT constraints (`HasDefaultValueSql` se dvěma parametry) ani `OwnsOne...ToJson()` v celé TPC hierarchii. Místo JSON owned entities použij value converter.

### Computed columns

`DurationMinutes` je persisted computed column (`DATEDIFF(minute, TimeFrom, TimeTo)`) v tabulkách `Training`, `Match` i `TrainingPlan`. **Nikdy jej počítat v C# kódu.**

### Modul skladového hospodářství (Inventory)

Schéma `inventory`. Sdílené entity (`Manufacturer`, `Location`) jsou v `dbo`.

**TPC hierarchie** (analogie k `SportEvent → Training / Match`):

```csharp
modelBuilder.HasSequence<int>("InventoryItemSeq", Schemas.Inventory).StartsAt(1).IncrementsBy(1);
// InventoryItem (abstract) → Equipment + Asset
```

**Namespace konvence:**
- Všechny inventory typy (TPC hierarchie i lookup tabulky): `SportSys.Database.Models.inventory`
- Sdílené dbo entity (`Manufacturer`, `Location`): `SportSys.Database.Models.dbo`

**Zděděné FK z TPC bázové třídy:** Apollo `IdConvention()` zpracuje pojmenování FK sloupců zděděných z `InventoryItem` — `HasColumnName` není potřeba.

**TPC FK constraint:** Entity `Loan`, `InventoryTransaction`, `InventoryItemPurchase`, `ItemLocationHistory`, `InventoryCheck` mají `InventoryItemId` bez DB-level FK constraint (TPC omezení). Integrita se vynucuje v Contract servisech.

**Dvě FK na Location:** `InventoryItem` odkazuje na `Location` dvěma FK (`AssignedLocationId`, `CurrentLocationId`). Vyžaduje `[InverseProperty]` na navigacích a odpovídající kolekce v `Location` — viz pravidlo o více vztazích.

**Lookup enumerace modulu:**
- `EItemStatus` – stav položky (int sloupec, ne lookup tabulka)
- `ETransactionType` – seeduje `inventory.TransactionType`
- `ECategoryType` – typ kategorie (int sloupec `CategoryType` v `inventory.Category`)

> Inventory enumerace jsou umístěny v `Models/inventory/` s namespace `SportSys.Database.Models.inventory`, **nikoli** v globálním `Enums/` folderu — jsou součástí modulu.

Podrobnosti: [docs/inventory.md](../docs/inventory.md) · Implementační plán: [.github/tasks/inventory-data-layer.md](tasks/inventory-data-layer.md)

### Schéma `plan.*` (read-only)

Modely `Block` a `Task` v namespace `Emr` jsou namapovány na externí databázi rezervačního systému. Do těchto tabulek se **nikdy nezapisuje**.

### VIEW `sport.SportEvent`

SQL view spojující `Training UNION ALL Match` s rozlišovacím sloupcem `EventType`. Slouží pro unifikovaný přehled kalendáře.

## Konvence kódu

### Nullable a ImplicitUsings

Všechny projekty mají `<Nullable>enable</Nullable>` a `<ImplicitUsings>enable</ImplicitUsings>`. Dodržovat nullable anotace; vyhýbat se `null!` bez komentáře.

### Konvence modelů a EF Core konfigurace

**Data atributy mají přednost před Fluent API.** Veškerá konfigurace, kterou lze vyjádřit pomocí data atributů, patří přímo na modely v `Models/`. Fluent API v `Configurations/` se používá **výhradně** pro věci, které data atributy neumožňují:

| Lze atributem | Patří do modelu |
|---|---|
| Tabulka, schéma | `[Table(nameof(Training), Schema = Schemas.Sport)]` |
| Primární klíč | `[Key]` / `[PrimaryKey(nameof(A), nameof(B))]` |
| FK + navigace | `[ForeignKey(nameof(PropertyId))]` na nav. vlastnosti |
| Chování při mazání | `[DeleteBehavior(DeleteBehavior.Cascade)]` na nav. vlastnosti |
| Délka řetězce | `[StringLength(50)]` |
| Unicode / VARCHAR | `[Unicode(false)]` |
| Přesnost | `[Precision(0)]` |
| Název sloupce | `[Column("sloupec")]` |
| Typ sloupce | `[Column(TypeName = "decimal(5,2)")]` |
| Unikátní index | `[Index(nameof(Prop), IsUnique = true)]` (na třídě) |
| Složený index | `[Index(nameof(A), nameof(B), Name = "IX_...")]` (na třídě) |
| Inverzní navigace | `[InverseProperty(nameof(Other.Nav))]` |

**Výlučně Fluent API** (atribut neexistuje nebo ho EF nedokáže vyhodnotit):
- `HasComputedColumnSql(...)` – persisted computed sloupce (DurationMinutes, FullName)
- `HasDefaultValueSql(...)` / `HasDefaultValue(...)` s pojmenovaným constraintem
- `UseTpcMappingStrategy()` – TPC dědičnost
- `HasConversion(...)` – value convertory (např. JSON pro MatchResult)
- `HasData(...)` – seed data (lookup tabulky)
- `HasSequence(...)` – databázové sekvence

Konfigurační soubor v `Configurations/` se vytváří **jen tehdy**, když je pro entitu skutečně něco k vyjádření pomocí Fluent API.

### Schémata ve statické třídě `Schemas`

Názvy DB schémat jsou `const string` v `Models/Schemas.cs`. V atributech **vždy** použít `Schemas.Sport` / `Schemas.Dbo` / `Schemas.Identity` / `Schemas.Inventory` – nikdy string literál.

### `nameof` místo string literálů v atributech

Všude, kde atribut přijímá název C# symbolu, používat `nameof`. Sloučení konstant je v atributech platné:

```csharp
// složený FK
[ForeignKey(nameof(Season_Id) + ", " + nameof(SeasonCategory_Name))]
```

### `[ForeignKey]` – použít pouze když konvence nestačí

EF Core odvozuje FK z konvence `{NázevNavigace}_Id`. Atribut přidávat jen pro: složený FK, sdílený sloupec ve dvou FK téže entity, nebo PK+FK.

### `[InverseProperty]` – použít pouze při více vztazích

Nutné jen při více vztazích mezi stejnou dvojicí entit.

### `HasDefaultValue` – vždy pojmenovat constraint

```csharp
builder.Property(e => e.ZipCode).HasDefaultValue("", "DF_IceRink_ZipCode");
// vzor: DF_{TabulkaBezSchématu}_{Sloupec}
```

### Lookup (číselníkové) tabulky

Lookup tabulky: `TrainingType`, `TrainingState`, `TrainingPhase`, `ParticipationType`, `MatchType`, `CoachRole`; z modulu Inventory: `TransactionType`. Vzor: `int Id` (PK) + `required string Name`.

**Enum pro každou lookup tabulku** v `src/SportSys.Database/Enums/`:
- Pojmenování: `E{Název}` (prefixový vzor, nikoli suffix `Enum`), např. `ETrainingType`, `EMatchType`
- Členy anglicky (neutrální klíč), PascalCase, int hodnoty od 1, nikdy nula
- Nikdy neměnit existující int hodnoty; nemazat hodnotu pokud na ní existují FK záznamy
- Každý enum člen má `[Display(Name = "Člen", ResourceType = typeof(SportSys.Database.Resources.E{Enum}))]`

**Seeding:** přes `HasData(Enum.GetValues<E{X}>().Select(e => new Entity(e)))` v konfiguraci.

**Konstruktory pro seeding** patří do `{Entity}.Seed.cs` (partial třída) – auto-generated modely se neupravují:

```csharp
public partial class TrainingType
{
    private TrainingType() { Name = null!; }

    [SetsRequiredMembers]
    public TrainingType(ETrainingType id) { Id = (int)id; Name = id.ToString(); }
}
```

**Lokalizace:** `Name` v DB je neutrální klíč (`enum.ToString()`), nikdy lokalizovaný text. Každý enum má trojici RESX v `src/SportSys.Database/Resources/`: `E{Enum}.cs` (ruční ResourceManager wrapper), `E{Enum}.resx` (anglický fallback), `E{Enum}.cs.resx` (české překlady). Pozor: wrapper třída a enum mají stejný název ale různé namespace – v enum souborech neimportovat Resources namespace, použij fully-qualified type v atributu.

**`MatchType` koliduje se `System.IO.MatchType`** – v konfiguračním souboru použij:
```csharp
using MatchType = SportSys.Database.Models.sportSchema.MatchType;
```

### Autentizace a autorizace

Interní uživatelé: Microsoft Entra ID (OIDC, SSO, MFA). Fallback: lokální ASP.NET Core Identity účty.

**`User`** (v `Models/identity/`) rozšiřuje `IdentityUser<int>` o:
- `EntraOid`, `EntraTenantId` – identita pro Entra uživatele (nikdy email/UPN jako klíč)
- `DisplayName`, `IsLocalAccount`, `LastLoginUtc`

Identity tabulky jsou ve schématu `identity` (ne výchozí `dbo`), názvy bez `AspNet` prefixu: `User`, `Role`, `UserRole`, atd.

Business permissions se **neukládají** do `AspNetRoles`/`AspNetUserClaims`. Identity role pouze pro: `SystemAdmin`, `Support`, `InternalUser`. Business authorization je připravena přes claims transformation (`EntraClaimsTransformation`) a policy-based authorization.

`SportSys.Database` a `SportSys.Contract` referencují ASP.NET Core Identity přes `<FrameworkReference Include="Microsoft.AspNetCore.App" />` (ne NuGet balíček).

> ⚠️ **`AddIdentity<T>()` nesmí být použito** — nahrazuje OIDC jako výchozí autentizační schéma, čímž přeruší přihlašování přes Entra ID. Vždy použít `AddIdentityCore<User>()` + `.AddSignInManager()`.

### Scaffolding Identity stránek — povinný cleanup

Po scaffoldingu Razor Identity stránek (`dotnet aspnet-codegenerator identity`) scaffolder vloží do `Program.cs` 3 řádky, které **musí být okamžitě smazány** — jinak app spadne s `"Scheme already exists: Identity.Application"`:

```csharp
// ❌ Smazat — scaffold vloží tyto řádky do Program.cs:
var connectionString = builder.Configuration.GetConnectionString("SportSysDbContext"); // špatný klíč
builder.Services.AddDbContext<SportSys.Razor.Data.SportSysDbContext>(...);             // duplikát
builder.Services.AddDefaultIdentity<SportSys.Razor.Data.ApplicationUser>(...);        // kolize s AddSportSysServices
```

Dále scaffoldované stránky v `Areas/Identity/Pages/` používají `using SportSys.Razor.Data` — přejmenovat na `using SportSys.Database.Models.identity`.

Jediné místo, kde se `DbContext`, Identity a auth registrují, je `AddSportSysServices()` v Contract.

### Frontend (CSS/SCSS)

Žádný CSS framework (Bootstrap, Tailwind apod.). Vlastní SCSS kompilovaný do `wwwroot/css/site.css` přes `npm run build:css`.

> ⚠️ `wwwroot/css/site.css` je **kompilovaný výstup** — nikdy jej neupravovat ručně. Změny se provádějí v `Styles/*.scss` a pak se spustí `npm run build:css`.

SCSS soubory jsou v `src/SportSys.Razor/Styles/`:
- `_vars.scss` – design tokeny (barvy, fonty, breakpointy) — vždy přes CSS custom properties, nikdy přímé hex hodnoty v komponent souborech
- Barevné schéma je definováno v `.github/barevna-schemata/hc-klatovy/` (HC Klatovy: červená + námořní modrá + zlatá)
- Sémantické tokeny (`--color-*`) mají přednost před primitivními tokeny (`--sport-red-500`)

### Ikony (Font Awesome)

Celá aplikace používá Font Awesome 6. Pravidla:

- Každá ikona musí mít třídu `fa-fw` (pevná šířka) pro zarovnání v textu i tlačítkách.
- **Akční tlačítka v řádcích gridu** jsou vždy ikonová (bez textu), třída `button secondary icon-btn` s atributem `title` pro přístupnost:

```html
@* Editace *@
<a class="button secondary icon-btn" asp-page="Edit" asp-route-id="@item.Id" title="Upravit">
    <i class="fa-solid fa-pen fa-fw"></i>
</a>

@* Smazání *@
<button type="submit" class="button tertiary icon-btn" title="Smazat">
    <i class="fa-solid fa-trash fa-fw"></i>
</button>

@* Zobrazení detailu *@
<a class="button secondary icon-btn" asp-page="Detail" asp-route-id="@item.Id" title="Detail">
    <i class="fa-solid fa-eye fa-fw"></i>
</a>
```

- **Standardní ikonový slovník** (používat konzistentně napříč celou aplikací):

| Akce | Ikona |
|---|---|
| Přidat / Nový | `fa-solid fa-plus` |
| Upravit | `fa-solid fa-pen` |
| Smazat | `fa-solid fa-trash` |
| Detail / Zobrazit | `fa-solid fa-eye` |
| Hledat | `fa-solid fa-magnifying-glass` |
| Vymazat filtr | `fa-solid fa-filter-circle-xmark` |
| Uložit | `fa-solid fa-floppy-disk` |
| Zpět | `fa-solid fa-arrow-left` |
| Potvrdit / OK | `fa-solid fa-circle-check` |
| Varování | `fa-solid fa-triangle-exclamation` |

- Tlačítka mimo gridy (nadpisové akce, formulářová tlačítka) mohou mít i text vedle ikony.
- Tlačítka v kategoriích stromu a podobných strukturách také používají `icon-btn`.

### Import z Excelu

`ImportRun` (tréninky) a `MatchImportRun` (zápasy) sdílejí stejný vzor parsování:
- `ExcelDataReader` pro čtení `.xlsx`
- `TryParseExcelDate` zvládá OA date (double), `DateTime` i string v `cs-CZ`
- `TryParseTime` zvládá zlomek dne (double < 1), `DateTime` i `TimeSpan`
- `Encoding.RegisterProvider(CodePagesEncodingProvider.Instance)` musí být zavoláno před `ExcelReaderFactory.CreateReader`

`ImportRun` volá stored procedure `[dbo].[procImportCoachTrainingKIS]` přímo přes ADO.NET (ne EF Core) – záměrně, kvůli detekci duplicit v SP.

`MatchImportRun.cs` je momentálně vyloučen z kompilace (`<Compile Remove="MatchImportRun.cs" />`).

### Apollo.HttpService (submodul)

`HttpService` v `ConsoleApp` používá interní knihovnu z `src/Apollo/MultiTarget/Apollo.HttpService`. Geokódování pro import IceRink a Team probíhá přes Nominatim API.

## Konfigurace

Connection stringy nesmí být commitovány. Citlivé hodnoty patří do `appsettings.json` (lokálně) nebo user secrets:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.;Database=SportSys;Trusted_Connection=True;TrustServerCertificate=True;",
    "ExternalSystem":    "Server=.;Database=ExternalDb;Trusted_Connection=True;"
  }
}
```