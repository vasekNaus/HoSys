---
name: has-default-value
description: >
  Přidá pojmenovaný DEFAULT constraint na sloupec v EF Core modelu.
  Použij tento skill když nastavuješ výchozí hodnotu sloupce pomocí HasDefaultValue
  nebo HasDefaultValueSql a chceš předvídatelný název DB constraintu.
user-invocable: true
---

# Pojmenovaný DEFAULT constraint (HasDefaultValue)

## Kdy použít

- Nastavuješ výchozí hodnotu sloupce pomocí Fluent API
- Potřebuješ předvídatelný název DB constraintu (pro diff skripty, CI a uživatelem vytvořené migrace)
- EF Core by jinak vygeneroval náhodný hash v názvu (např. `DF__IceRink__ZipCode__3A4CA8FD`)

## Postup

1. V konfiguračním souboru `Configurations/{schema}/{Entity}Configuration.cs` použij **dvouparametrovou** přetíženou verzi `HasDefaultValue`:

   ```csharp
   builder.Property(e => e.ZipCode)
          .HasDefaultValue("", "DF_IceRink_ZipCode");
   ```

   Nebo pro SQL výraz:
   ```csharp
   builder.Property(e => e.CreatedAt)
          .HasDefaultValueSql("GETUTCDATE()", "DF_Training_CreatedAt");
   ```

2. Pojmenuj constraint podle vzoru:
   ```
   DF_{TabulkaBezSchématu}_{NázevSloupce}
   ```

   Příklady:
   - `DF_IceRink_ZipCode`
   - `DF_SeasonCategory_BirthYears`
   - `DF_Training_CreatedAt`

3. Migraci nevytvářej ani neupravuj. Vytvoření migrace a kontrolu výsledného
   `AddColumn` nebo `AlterColumn` provádí výhradně uživatel.

## Omezení

- Skill se týká výhradně Fluent API v `IEntityTypeConfiguration<T>` — ne datových atributů.
- Pojmenovaný constraint nelze nastavit přes data atributy — vždy vyžaduje Fluent API.
- Agent nesmí vytvářet ani upravovat EF Core migrace.

## Checklist

- [ ] `HasDefaultValue` nebo `HasDefaultValueSql` má druhý parametr s názvem constraintu
- [ ] Název odpovídá vzoru `DF_{TabulkaBezSchématu}_{Sloupec}`
- [ ] Změna je připravena pro uživatelem vytvořenou migraci
- [ ] Nebyla vytvořena ani upravena žádná migrace

## Reference

- `src/SportSys.Database/Configurations/sport/IceRinkConfiguration.cs`
- `src/SportSys.Database/Configurations/sport/SeasonCategoryConfiguration.cs`
- `docs/conventions.md` — sekce HasDefaultValue
