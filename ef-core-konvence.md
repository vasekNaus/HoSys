# EF Core — konvence pro datové atributy a modely

## 1. `nameof` místo magických řetězců

Všude, kde atribut přijímá název C# symbolu (třída, vlastnost), používej `nameof` místo string literálu.

```csharp
// ❌ Špatně
[Table("Training", Schema = "sport")]
[ForeignKey("Season_Id")]

// ✅ Správně
[Table(nameof(Training), Schema = Schemas.Sport)]
[ForeignKey(nameof(Season_Id))]
```

### Složené FK (více vlastností)

`nameof` vrací compile-time konstantu — konkatenace konstant je v atributech platná:

```csharp
// ❌ Špatně
[ForeignKey("Season_Id, SeasonCategory_Name")]

// ✅ Správně
[ForeignKey(nameof(Season_Id) + ", " + nameof(SeasonCategory_Name))]
```

> **Poznámka:** Atributy vyžadují compile-time konstanty. `nameof` tuto podmínku splňuje.

---

## 2. Schémata ve statické třídě

Názvy DB schémat jsou uloženy jako `const string` v centrální statické třídě `Schemas`.
Umístění: `Models/Schemas.cs`.

```csharp
namespace SportSys.Database.Models;

public static class Schemas
{
    public const string Dbo = "dbo";
    public const string Sport = "sport";
}
```

> **Proč `const`, ne `static readonly`?**
> Atributy vyžadují compile-time konstanty. `static readonly` compile-time konstanta není — v atributech by způsobilo chybu překladu.

Použití:

```csharp
[Table(nameof(Training), Schema = Schemas.Sport)]
[Table(nameof(Coach), Schema = Schemas.Dbo)]
```

---

## 3. `[ForeignKey]` — použít pouze když je nutné

EF Core umí FK odvodit z konvence `{NázevNavigace}_Id` / `{NázevEntity}_Id`. Atribut `[ForeignKey]` přidávej jen tam, kde konvence nestačí:

| Situace | `[ForeignKey]` nutný? |
|---|---|
| FK splňuje konvenci `{Navigace}_Id`, jediný vztah | ❌ Ne |
| **Složený FK** (composite key) | ✅ Ano |
| **Sdílený sloupec** ve dvou FK téže entity | ✅ Ano |
| FK sloupec je zároveň součástí PK (shared PK/FK) | ✅ Ano |

```csharp
// ❌ Zbytečné — konvence IceRink_Id → IceRink stačí
[ForeignKey(nameof(IceRink_Id))]
public virtual IceRink IceRink { get; set; } = null!;

// ✅ Nutné — složený FK, nestandardní mapování
[ForeignKey(nameof(Season_Id) + ", " + nameof(SeasonCategory_Name))]
public virtual SeasonCategory SeasonCategory { get; set; } = null!;

// ✅ Nutné — Season_Id se zároveň použije ve složeném FK jiné navigace
[ForeignKey(nameof(Season_Id))]
public virtual Season Season { get; set; } = null!;
```

---

## 4. `[InverseProperty]` — použít pouze při více vztazích

EF Core páruje navigační vlastnosti automaticky, pokud mezi dvojicí entit existuje **jediný vztah**. `[InverseProperty]` je nutný jen při **více vztazích mezi stejnými entitami** (aby EF Core věděl, která navigace k které patří).

```csharp
// ❌ Zbytečné — jediný vztah mezi Training a IceRink
[InverseProperty("Trainings")]
public virtual IceRink IceRink { get; set; } = null!;

// ✅ Nutné — Employee má dvě navigace na Department (HomeDepart., ManagedDepart.)
[InverseProperty(nameof(Employee.HomeDepart))]
public virtual ICollection<Employee> HomeEmployees { get; set; } = [];
```

---

## Rychlý přehled

| Atribut | Kdy použít |
|---|---|
| `[Table(nameof(X), Schema = Schemas.Y)]` | Vždy — `nameof` místo string literálu |
| `[ForeignKey(nameof(Prop))]` | Jen když konvence nestačí (složený FK, sdílený sloupec, PK+FK) |
| `[InverseProperty]` | Jen při více vztazích mezi stejnou dvojicí entit |
