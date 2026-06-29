# Modul: Skladové hospodářství (Inventory)

## Účel

Modul eviduje majetek a výstroj hokejového klubu. Zajišťuje úplnou dohledatelnost životního cyklu každé položky – od pořízení přes pohyby, zápůjčky a opravy až po vyřazení. Slouží také jako podpora pro pravidelné inventury.

## Rozsah evidence

- Hokejová výstroj (dresy, helmy, rukavice, hokejky, brusle, …)
- Sportovní oblečení a doplňky
- Dlouhodobý majetek (notebooky, monitory, tiskárny, …)
- Vybavení tělocvičny (spinningová kola, posilovací stroje, …)
- Elektronika a kancelářské vybavení

## Klíčové funkce

- Evidence jednotlivých kusů majetku s unikátním inventárním číslem
- QR kódy pro rychlou identifikaci při inventurách
- Sledování stavů položek (Ve skladu / Přidělena / Zapůjčena / V servisu / Ztracena / Vyřazena)
- Evidence zápůjček členům klubu s historií vydání a vrácení
- Automatický audit trail – každá operace nad položkou vytváří záznam pohybu
- Podpora periodických inventur s evidencí výsledků kontrol
- Evidence nákupních dokladů a financování pořízení
- Historie umístění každé položky
- Stromová kategorizace výstroje a majetku

---

## Databázová architektura

### Schémata

| Schéma | Obsah |
|---|---|
| `inventory` | Entity skladového hospodářství |
| `dbo` | Sdílené entity: `Manufacturer`, `Location` |

### Namespace

| Typ entit | Namespace |
|---|---|
| Abstraktní základ TPC + konkrétní typy + lookup tabulky | `SportSys.Database.Models.inventory` |
| Sdílené entity dbo (Manufacturer, Location) | `SportSys.Database.Models.dbo` |

### Konfigurace EF Core

Konfigurační soubory patří do `Configurations/inventory/`. Vznikají **pouze** pro entity, kde je nutné Fluent API (TPC, sekvence, seed data, value convertory). Pojmenování FK sloupců zajišťuje Apollo `IdConvention()` automaticky.

---

## TPC dědičnost

Modul používá strategii **TPC (Table Per Concrete Type)** identicky s hierarchií `SportEvent → Training / Match`. Fyzická tabulka `InventoryItem` v databázi neexistuje. ID je unikátní napříč oběma tabulkami díky sdílené sekvenci `inventory.InventoryItemSeq`.

```
InventoryItemSeq  (SEQUENCE – sdílené ID pro Equipment i Asset)
       │
       ├── inventory.Equipment   (výstroj – dresy, helmy, brusle, …)
       │
       └── inventory.Asset       (majetek – notebooky, stroje, …)
```

> **Pozor na zděděné FK v TPC:** Fyzická tabulka `InventoryItem` neexistuje. Sekvence `inventory.InventoryItemSeq` zajišťuje unikátní ID napříč `Equipment` i `Asset`. Apollo `IdConvention()` zpracuje pojmenování FK sloupců zděděných z bázové třídy – není potřeba ruční `HasColumnName`.

---

## Entity

### InventoryItem (abstraktní základ – TPC)

Společné vlastnosti sdílené oběma konkrétními typy. Fyzická tabulka neexistuje.

| Vlastnost | SQL typ | Popis |
|---|---|---|
| `Id` | `int` | PK – hodnota z sekvence `inventory.InventoryItemSeq` |
| `InventoryNumber` | `varchar(20)` | Unikátní inventární číslo (formát `INV-YYYY-NNNNNN`) – po vytvoření neměnné |
| `Name` | `nvarchar(200)` | Název položky |
| `Description` | `nvarchar(max)?` | Volná poznámka |
| `CategoryId` | `int` | FK → `inventory.Category` |
| `ManufacturerId` | `int?` | FK → `dbo.Manufacturer` |
| `AssignedLocationId` | `int?` | FK → `dbo.Location` (organizační příslušnost) |
| `CurrentLocationId` | `int?` | FK → `dbo.Location` (skutečné aktuální umístění) |
| `ItemStatus` | `int` | Stav položky dle `EItemStatus` |
| `AcquisitionDate` | `date?` | Datum pořízení |
| `AcquisitionPrice` | `decimal(10,2)?` | Pořizovací cena |
| `QRCodeValue` | `varchar(500)?` | Hodnota QR kódu (inventární číslo nebo URL) |
| `IsActive` | `bit` | Aktivní / archivováno |
| `CreatedAt` | `datetime2` | Datum a čas vytvoření záznamu |
| `CreatedByUserId` | `int?` | FK → `dbo.User` (kdo vytvořil) |
| `ModifiedAt` | `datetime2?` | Datum a čas poslední změny |
| `ModifiedByUserId` | `int?` | FK → `dbo.User` (kdo naposledy upravil) |

### Equipment (výstroj)

Fyzická tabulka `inventory.Equipment`. Obsahuje všechny sloupce z `InventoryItem` plus:

| Vlastnost | SQL typ | Popis |
|---|---|---|
| `Size` | `nvarchar(50)?` | Velikost položky – textová hodnota (např. „M", „42", „13") |

Konkrétní hodnoty velikostí nejsou ověřovány databázově. Pokud má kategorie definovány `AvailableSizesJson`, UI nabídne dropdown s povolenými hodnotami; jinak zobrazí volné textové pole.

**Příklady:** dres, helma, rukavice, kalhoty, hokejka, brusle, tepláková souprava.

### Asset (majetek)

Fyzická tabulka `inventory.Asset`. Obsahuje všechny sloupce z `InventoryItem` plus:

| Vlastnost | SQL typ | Popis |
|---|---|---|
| `SerialNumber` | `nvarchar(100)?` | Výrobní / sériové číslo |
| `WarrantyUntil` | `date?` | Datum konce záruky |
| `ExternalId` | `nvarchar(100)?` | Externí označení (inventární číslo jiného systému) |

**Příklady:** notebook, PC, monitor, tiskárna, spinningové kolo, posilovací stroj.

---

## Kategorie

### Category

`inventory.Category` – stromová struktura kategorií (self-referencing). Příslušnost k typu položky (Equipment / Asset) je určena pozicí ve stromu – kořenové kategorie **Sportovní vybavení** a **Majetek** jsou seedována při migraci.

| Vlastnost | SQL typ | Popis |
|---|---|---|
| `Id` | `int` | PK |
| `ParentCategoryId` | `int?` | FK → `inventory.Category` (rodičovská kategorie) |
| `Name` | `nvarchar(100)` | Název |
| `AvailableSizesJson` | `nvarchar(max)?` | JSON pole povolených velikostí, např. `["XS","S","M","L","XL"]` |
| `SortOrder` | `int` | Pořadí řazení |
| `IsActive` | `bit` | Aktivní |

#### Jak funguje stromová příslušnost

- `Equipment` používá kategorie z podstromu **Sportovní vybavení**
- `Asset` používá kategorie z podstromu **Majetek**

Tím se eliminuje riziko nekonzistence (žádný enum `CategoryType` v DB).

#### Povolené velikosti

Pokud kategorie povolené velikosti využívá, je `AvailableSizesJson` vyplněno jako JSON pole:

```json
["XS", "S", "M", "L", "XL", "XXL"]
```

Jinak je `null`. V administraci kategorie se velikosti zadávají jako víceřádkový text (jeden řádek = jedna velikost) a aplikace je automaticky převede na JSON.

**Výchozí stromová struktura (seed data):**

```
Sportovní vybavení
├── Výstroj
│   ├── Dres
│   ├── Brusle
│   ├── Helma
│   ├── Hokejka
│   ├── Vesta
│   ├── Holeně
│   ├── Suspenzor
│   ├── Kalhoty
│   ├── Lokty
│   └── Nákrčník
├── Oblečení
│   ├── Tričko
│   ├── Mikina
│   └── Bunda
└── Tréninková pomůcka
    ├── Puk
    ├── Kužel
    └── Překážka

Majetek
├── IT
│   ├── Notebook
│   ├── PC
│   ├── Monitor
│   └── Tiskárna
├── Spotřebiče
│   └── Vysoušeč
├── Posilovna
│   ├── Spinningové kolo
│   └── Posilovací stroj
└── Kancelář
    └── Nábytek
```

---

## Výrobci

### Manufacturer

`dbo.Manufacturer` – sdílená entita, může být využita i dalšími moduly.

| Vlastnost | SQL typ | Popis |
|---|---|---|
| `Id` | `int` | PK |
| `Name` | `nvarchar(200)` | Název výrobce |
| `Website` | `nvarchar(500)?` | URL webu |
| `IsActive` | `bit` | Aktivní |

---

## Umístění

### Location

`dbo.Location` – stromová struktura umístění (self-referencing). Sdílená entita, může být využita i dalšími moduly.

| Vlastnost | SQL typ | Popis |
|---|---|---|
| `Id` | `int` | PK |
| `ParentLocationId` | `int?` | FK → `dbo.Location` (rodičovské umístění) |
| `Name` | `nvarchar(200)` | Název |
| `Description` | `nvarchar(500)?` | Popis |
| `IsActive` | `bit` | Aktivní |

Každá položka skladu má dvě vazby na `Location`:

| Vazba | Popis | Příklady |
|---|---|---|
| `AssignedLocation` | Kam položka organizačně patří | Hlavní sklad, Kabina A, Posilovna, Kancelář |
| `CurrentLocation` | Kde se skutečně nachází | Servis, Turnaj, Autobus, Hlavní sklad |

> Protože `InventoryItem` odkazuje na `Location` dvěma různými FK, jsou pro navigační vlastnosti v `Location` nutné atributy `[InverseProperty]`.

---

## Zápůjčky

### Loan

`inventory.Loan` – evidence zapůjčení položky členu klubu.

| Vlastnost | SQL typ | Popis |
|---|---|---|
| `Id` | `int` | PK |
| `InventoryItemId` | `int` | Odkaz na zapůjčenou položku (Equipment nebo Asset) |
| `MemberId` | `int` | FK → `dbo.User` (člen, jemuž je zapůjčeno) |
| `LoanDate` | `date` | Datum vydání |
| `ExpectedReturnDate` | `date?` | Plánované datum vrácení |
| `ReturnedDate` | `date?` | Skutečné datum vrácení |
| `Note` | `nvarchar(500)?` | Poznámka |
| `IsClosed` | `bit` | Zápůjčka uzavřena (vráceno nebo odepsáno) |

> **Poznámka TPC:** `InventoryItemId` odkazuje na abstraktní typ bez fyzické tabulky. FK constraint na úrovni databáze nelze vynutit. Integrita je zajišťována aplikační vrstvou. Totéž platí pro `InventoryTransaction`, `ItemLocationHistory`, `InventoryItemPurchase` a `InventoryCheck`.

---

## Pohyby skladu

### InventoryTransaction

`inventory.InventoryTransaction` – klíčová auditní entita. Každá operace nad skladovou položkou **musí** vytvořit záznam pohybu.

| Vlastnost | SQL typ | Popis |
|---|---|---|
| `Id` | `int` | PK |
| `InventoryItemId` | `int` | Odkaz na položku |
| `TransactionTypeId` | `int` | FK → `inventory.TransactionType` |
| `TransactionDate` | `datetime2` | Datum a čas operace |
| `Quantity` | `int` | Počet kusů (standardně 1) |
| `UserId` | `int?` | FK → `dbo.User` (kdo provedl operaci) |
| `Note` | `nvarchar(500)?` | Poznámka |

### TransactionType

`inventory.TransactionType` – lookup tabulka typů pohybů. Seedována z `ETransactionType`.

| Hodnota enumu | Popis |
|---|---|
| `Purchase` | Nákup / zařazení do evidence |
| `Loan` | Zapůjčení členu |
| `Return` | Vrácení zápůjčky |
| `Transfer` | Přesun na jiné umístění |
| `RepairStart` | Zahájení opravy / odeslání do servisu |
| `RepairEnd` | Ukončení opravy / převzetí ze servisu |
| `InventoryCheck` | Inventurní kontrola |
| `Lost` | Ztráta položky |
| `Dispose` | Vyřazení z evidence |

---

## Nákupy a financování

### PurchaseDocument

`inventory.PurchaseDocument` – faktura nebo jiný nákupní doklad.

| Vlastnost | SQL typ | Popis |
|---|---|---|
| `Id` | `int` | PK |
| `DocumentNumber` | `nvarchar(100)` | Číslo dokladu / faktury |
| `SupplierName` | `nvarchar(200)` | Název dodavatele |
| `PurchaseDate` | `date` | Datum nákupu |
| `TotalAmount` | `decimal(10,2)` | Celková částka dokladu |
| `Note` | `nvarchar(500)?` | Poznámka |

### InventoryItemPurchase

`inventory.InventoryItemPurchase` – vazba konkrétní položky na nákupní doklad.

| Vlastnost | SQL typ | Popis |
|---|---|---|
| `Id` | `int` | PK |
| `InventoryItemId` | `int` | Odkaz na položku |
| `PurchaseDocumentId` | `int` | FK → `inventory.PurchaseDocument` |
| `PurchasePrice` | `decimal(10,2)` | Pořizovací cena dané položky z tohoto dokladu |

---

## Historie umístění

### ItemLocationHistory

`inventory.ItemLocationHistory` – automaticky plněná evidence každé změny umístění.

| Vlastnost | SQL typ | Popis |
|---|---|---|
| `Id` | `int` | PK |
| `InventoryItemId` | `int` | Odkaz na položku |
| `PreviousLocationId` | `int?` | FK → `dbo.Location` (odkud) |
| `NewLocationId` | `int` | FK → `dbo.Location` (kam) |
| `ChangedAt` | `datetime2` | Čas změny |
| `ChangedByUserId` | `int?` | FK → `dbo.User` |
| `Note` | `nvarchar(500)?` | Poznámka |

---

## Inventury

### InventorySession

`inventory.InventorySession` – jeden inventurní běh (prováděný minimálně jednou ročně).

| Vlastnost | SQL typ | Popis |
|---|---|---|
| `Id` | `int` | PK |
| `Name` | `nvarchar(200)` | Název inventury (např. „Inventura 2026") |
| `StartedAt` | `datetime2` | Datum a čas zahájení |
| `FinishedAt` | `datetime2?` | Datum a čas ukončení |
| `IsClosed` | `bit` | Inventura uzavřena |

### InventoryCheck

`inventory.InventoryCheck` – výsledek kontroly konkrétní položky v rámci inventury.

| Vlastnost | SQL typ | Popis |
|---|---|---|
| `Id` | `int` | PK |
| `InventorySessionId` | `int` | FK → `inventory.InventorySession` |
| `InventoryItemId` | `int` | Odkaz na kontrolovanou položku |
| `CheckedAt` | `datetime2` | Čas provedení kontroly |
| `CheckedByUserId` | `int?` | FK → `dbo.User` |
| `Found` | `bit` | Položka nalezena (`true`) / nenalezena (`false`) |
| `ActualLocationId` | `int?` | FK → `dbo.Location` (skutečné umístění při kontrole) |
| `Note` | `nvarchar(500)?` | Poznámka |

---

## Inventární číslo

Každá položka má unikátní inventární číslo generované při prvním uložení. Po vytvoření je **neměnné**.

**Formát:** `INV-{ROK}-{NNNNNN}` (rok vytvoření, šestimístné pořadové číslo s nulami)

```
INV-2026-000001
INV-2026-000002
INV-2026-000003
```

Inventární číslo je přirozeným obsahem QR kódu a slouží jako lidsky čitelný identifikátor při inventurách.

---

## QR kódy

QR kód slouží jako primární identifikační mechanismus při skenování mobilním zařízením. Do databáze (`QRCodeValue`) se ukládá pouze textová hodnota; obrázek QR kódu se generuje aplikačně (na vyžádání, nikdy perzistovaně).

**Doporučený obsah:**
```
INV-2026-000001
```
nebo přímá URL:
```
https://app.domain.cz/inventory/item/12345
```

---

## Enumerace

### EItemStatus – stav položky

| Hodnota | int | Popis |
|---|---|---|
| `InStock` | 1 | Ve skladu |
| `Assigned` | 2 | Přidělena na konkrétní umístění |
| `Borrowed` | 3 | Zapůjčena členu klubu |
| `InRepair` | 4 | V servisu / opravě |
| `Lost` | 5 | Ztracena |
| `Disposed` | 6 | Vyřazena z evidence |

### ETransactionType – typ pohybu skladu

Seeduje tabulku `inventory.TransactionType`. Hodnoty viz sekce [TransactionType](#transactiontype) výše.

---

## Audit

Všechny entity modulu nesou auditní sloupce:

| Sloupec | Typ | Popis |
|---|---|---|
| `CreatedAt` | `datetime2` | Čas vytvoření záznamu |
| `CreatedByUserId` | `int?` | FK → `dbo.User` – kdo vytvořil |
| `ModifiedAt` | `datetime2?` | Čas poslední úpravy |
| `ModifiedByUserId` | `int?` | FK → `dbo.User` – kdo naposledy upravil |

Hodnoty se plní v aplikační vrstvě (Contract servisy). `CreatedAt` se nastaví při vložení, `ModifiedAt` při každé aktualizaci.

---

## Budoucí rozšíření

Datový model je navržen s výhledem na tato rozšíření (dosud neimplementována):

- Sezónní přidělení výstroje konkrétnímu hráči
- Vratné zálohy za vydané vybavení
- Schvalování výdeje majetku (workflow schválení)
- Evidence servisních zásahů a plán údržby
- Fotodokumentace položek (přílohy)
- Elektronické podpisy při fyzickém převzetí
- Mobilní inventura s QR / RFID skenerem
- Podpora více skladů s odděleným přístupem
- Automatické generování inventurních štítků (PDF)
- Hromadný import z Excelu
- Export inventurních sestav

---

## UI vrstva

### URL struktura

Všechny stránky modulu jsou umístěny pod cestou `/Inventory` v Areas:

```
/Inventory/Manufacturers          – Správa výrobců
/Inventory/Manufacturers/Edit     – Nový / editace výrobce
/Inventory/Locations              – Správa umístění
/Inventory/Locations/Edit         – Nové / editace umístění
/Inventory/Categories             – Správa kategorií
/Inventory/Categories/Edit        – Nová / editace kategorie
/Inventory/Loans                  – Přehled výpůjček
/Inventory/Loans/Create           – Nová výpůjčka (QR skener)
/Inventory/Loans/Edit/{id}        – Detail + vrácení položek
```

### Navigace

Navigační sekce modulu:

```
Sklad
├─ Výrobci
├─ Umístění
├─ Kategorie
└─ Výpůjčky
```

### Aplikační servisy (SportSys.Contract)

| Servis | Popis |
|---|---|
| `ManufacturerService` | CRUD výrobců; filtrování podle názvu |
| `LocationService` | CRUD umístění; stromová struktura; dropdown pro formuláře |
| `CategoryService` | CRUD kategorií; správa `AvailableSizesJson`; stromová struktura |
| `LoanService` | Správa výpůjček; vrácení položek; vyhledávání položek dle inventárního čísla |

Registrace: `AddSportSysServices()` v `ServiceCollectionExtensions.cs`.

### Modely (SportSys.Contract/Models/inventory/)

Modely jsou odděleny od databázových entit a patří do namespace `SportSys.Contract.Models.inventory`. Žádný model nemá suffix `Dto`.

| Model | Použití |
|---|---|
| `Manufacturer` | Seznam i formulář – obsahuje validační atributy |
| `Location` | Formulář (nové/editace) |
| `LocationListItem` | Řádek v seznamu umístění |
| `LocationSelectItem` | Položka dropdownu nadřazeného umístění |
| `CategoryModel` | Formulář (nové/editace kategorie) – obsahuje `AvailableSizesText` pro textarea |
| `CategoryListItem` | Řádek v přehledu kategorií – `HasSizes` indikuje přítomnost velikostí |
| `CategorySelectItem` | Položka dropdownu + `AvailableSizes[]` pro dynamický dropdown v editaci Equipment |
| `LoanListItem` | Řádek v přehledu výpůjček |
| `LoanDetail` | Hlavička detailu výpůjčky (jen čtení) |
| `LoanDetailItem` | Řádek v tabulce položek výpůjčky |
| `CreateLoan` | Vstup pro vytvoření výpůjčky |
| `InventoryItemLookup` | Výsledek vyhledání položky dle inventárního čísla (QR sken) |
| `MemberSelectItem` | Položka výběru člena ve formuláři výpůjčky |

### Výpůjčky – datový model UI vs. datová vrstva

Entita `Loan` eviduje **jedno zapůjčení jedné položky**. UI prezentuje výpůjčky jako skupiny – jeden hráč, jedno datum vydání = jedna logická výpůjčka (batch).

Skupinová identita = `(MemberId, LoanDate)`. Při vytvoření nové výpůjčky se vytvoří N záznamů `Loan` ve stejné transakci se stejným `LoanDate = DateOnly.FromDateTime(DateTime.Today)`.

Vypočítaný stav výpůjčky:

| Stav | Podmínka |
|---|---|
| Aktivní | Žádná položka v grupě nemá `ReturnedDate` |
| Částečně vráceno | Část položek má `ReturnedDate` |
| Uzavřeno | Všechny položky mají `ReturnedDate` nebo `IsClosed = true` |

Číslo výpůjčky = nejnižší `Loan.Id` ve skupině formátované jako `V-{Id:D5}`.

### Stránky (Razor Pages)

**Categories:**
- `Index` – tabulka s filtrem (Name); sloupce Název, Nadřazená, Pořadí, Velikosti (✓/—), Aktivní
- `Edit` – formulář: Název, Nadřazená kategorie (dropdown), Pořadí, Aktivní, Povolené velikosti (textarea – jeden řádek = jedna velikost)

**Manufactures:**
- `Index` – tabulka s filtrem (Name, tlačítka Hledat / Vymazat filtr), řazení dle názvu
- `Edit` – sdílený formulář pro Create i Edit; `IsNew` určuje nadpis

**Locations:**
- `Index` – tabulka s filtrem (Name); sloupce Název, Nadřazené umístění, Aktivní
- `Edit` – formulář s dropdownem nadřazeného umístění (pouze aktivní lokace)

**Loans:**
- `Index` – tabulka s filtrem (Člen, Aktivní výpůjčka, Datum od/do); zobrazuje skupiny
- `Create` – vícekrokový formulář: (1) výběr člena ze selectu, (2) přidávání položek přes inventární číslo / QR skener s inline validací pomocí vanilla JS fetch, (3) tlačítko Vytvořit výpůjčku
- `Edit` – detail skupiny (jen čtení: hlavička) + tabulka položek s tlačítky Potvrdit vrácení + hromadné Vrátit vše

---

## Diagram vztahů (zjednodušený)

```
dbo.Manufacturer ──────────────────┐
dbo.Location (×2) ─────────────────┤
dbo.User ──────────────────────────┤
                                   │
inventory.Category ────────────────┤
                                   ▼
              ┌─── inventory.Equipment (TPC)
InventoryItem │
              └─── inventory.Asset    (TPC)
                         │
          ┌──────────────┼──────────────┐
          ▼              ▼              ▼
    inventory.Loan  inventory.         inventory.
                    InventoryTrans-    InventoryItem-
                    action             Purchase
          │              │              │
          │         inventory.          inventory.
          │         TransactionType    PurchaseDocument
          │
    inventory.ItemLocationHistory
    inventory.InventoryCheck ──── inventory.InventorySession
```

---

## Související dokumenty

- [features.md](features.md) – přehled všech funkcí systému
- [architecture.md](architecture.md) – technická architektura
- [.github/tasks/inventory-data-layer.md](../.github/tasks/inventory-data-layer.md) – implementační plán datové vrstvy
- [.github/tasks/inventory-ui-layer-impl.md](../.github/tasks/inventory-ui-layer-impl.md) – implementační plán UI vrstvy
