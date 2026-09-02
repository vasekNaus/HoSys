# Modul Sport

## Účel

Modul Sport spravuje sportovní číselníky a zobrazuje rozpisy tréninků. Administrační
stránky jsou v Razor Area `sport` a přistupují k databázi výhradně přes služby
projektu `SportSys.Contract`.

## Rozvrhy tréninků

| Stránka | Route | Zdroj dat |
|---|---|---|
| Reálný rozvrh | `/sport/Training/Schedule` | `sport.Training` |
| Obecný týdenní plán | `/sport/Training/Plan` | `sport.TrainingPlan` |

Původní route `/sport/Schedule` není zachována.

### Společný datový kontrakt

`ITrainingScheduleItem` definuje vlastnosti potřebné pro vykreslení bloku na
časové ose. `TrainingPlanScheduleItemDto` interface implementuje a obsahuje navíc
období `From–To` a `DayName`.

`TrainingScheduleItemDto` přímo dědí z `TrainingPlanScheduleItemDto` a přidává
konkrétní `Date`. U reálného tréninku jsou zděděné hodnoty nastaveny jako plán
platný právě v den tréninku.

### Sdílená ViewComponent

Obě stránky předávají data přes `ITrainingScheduleViewModel` komponentě:

- třída: `src/SportSys.Razor/ViewComponents/TrainingScheduleViewComponent.cs`,
- view: `src/SportSys.Razor/Pages/Shared/Components/TrainingSchedule/Default.cshtml`,
- prezentační modely: `src/SportSys.Razor/Models/TrainingSchedule/`.

Komponenta pouze vykresluje předaná data. Zajišťuje časové markery, dynamický
rozsah osy, rozdělení překryvů do lanes, barvy kategorií a bezpečně HTML
enkódované tooltipy. Data načítají PageModely přes `TrainingScheduleService`.

### Filtry Schedule

- aktivní sezóna,
- jedna nebo více aktivních kategorií,
- datum od a do.

Řádky odpovídají konkrétním datům z vybraného intervalu, včetně dnů bez tréninku.

### Filtry Plan

- aktivní sezóna,
- jedna nebo více aktivních kategorií,
- jeden typ tréninku,
- jedna fáze tréninku.

Plan vždy vykreslí pondělí až neděli včetně prázdných dnů. Zobrazuje všechny
odpovídající záznamy bez omezení podle `From–To`; překrývající se záznamy a plány
s různými obdobími platnosti jsou rozděleny do samostatných lanes. Platnost je
uvedena v tooltipu.

`TrainingPlan.DayName` musí obsahovat přesnou anglickou hodnotu `Monday` až
`Sunday`. Neplatná hodnota vyvolá explicitní chybu a není tiše přeskočena.

## Spravované číselníky

| Číselník | Databázová entita | Administrační stránky |
|---|---|---|
| Zimní stadiony | `sport.IceRink` | `Areas/sport/Pages/IceRink/` |
| Týmy | `sport.Team` | `Areas/sport/Pages/Team/` |
| Sezóny | `sport.Season` | `Areas/sport/Pages/Season/` |
| Kategorie sezón | `sport.SeasonCategory` | `Areas/sport/Pages/SeasonCategory/` |

Enumové lookup tabulky `TrainingType`, `TrainingState`, `TrainingPhase`,
`ParticipationType` a `MatchType` se přes administrační UI nespravují. Jejich
identifikátory jsou svázané s C# enumy a seed konfigurací.

## Administrační vzor

Každý číselník používá dvojici stránek:

- `Index` obsahuje textový filtr, filtr stavu, tabulku a řádkové akce.
- `Edit` je jediný formulář pro vytvoření i úpravu záznamu.

Formuláře používají `@Html.EditorFor` a šablony v
`src/SportSys.Razor/Pages/Shared/EditorTemplates/`. Validační a zobrazovací
metadata jsou definována pomocí DataAnnotations na DTO v `SportSys.Contract`.

## Aktivita místo mazání

Entity `IceRink`, `Team`, `Season` a `SeasonCategory` mají příznak `IsActive`.
Fyzické mazání se v administraci nepoužívá.

- Výchozí hodnota `IsActive` je `true`.
- Každý DEFAULT constraint má název `DF_{Entity}_IsActive`.
- Index standardně zobrazuje pouze aktivní záznamy.
- Filtr umožňuje zobrazit aktivní, neaktivní nebo všechny záznamy.
- Záznam lze zneaktivnit z Indexu i z editačního formuláře.
- Neaktivní záznam lze znovu aktivovat.
- Zneaktivnění se nekaskáduje na související entity.

## Specifika entit

### IceRink

Administrace spravuje `Name`, `Street`, `City`, `ZipCode` a `IsActive`.
Geografické pole `Location` není součástí formuláře.

### Team

Administrace spravuje `Code`, `Name`, `Address`, `City`, `HomeIceRinkId` a
`IsActive`. Výběr domácího stadionu nabízí aktivní stadiony a při editaci zachová
i aktuálně přiřazený neaktivní stadion.

### Season

Administrace spravuje `Name`, `From`, `To` a `IsActive`. Platí invariant
`From <= To`.

### SeasonCategory

Entita má složený primární klíč `SeasonId + Name`. Při vytvoření jsou obě části
klíče povinné; při editaci jsou neměnné. Formulář dále spravuje `Order`,
`CompetitionCode`, `CompetitionTeamName`, `BirthYears` a `IsActive`.

## Contract služby

| Služba | Odpovědnost |
|---|---|
| `IceRinkService` | CRUD bez fyzického mazání, filtrování, seznam stadionů |
| `TeamService` | CRUD bez fyzického mazání a filtrování týmů |
| `SeasonService` | CRUD bez fyzického mazání, filtrování, seznam sezón |
| `SeasonCategoryService` | CRUD bez změny složeného klíče a filtrování kategorií |

Změna aktivity se provádí explicitní metodou `SetActiveAsync`. Služby nikdy
nevracejí databázové entity do Razor vrstvy.

## Databázové změny

Agent upravuje modely a EF Core konfigurace, ale nikdy nevytváří ani neupravuje
migrace nebo model snapshot. Vytvoření a aplikaci migrace provádí výhradně
uživatel.
