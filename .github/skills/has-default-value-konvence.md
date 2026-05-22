# Konvence: HasDefaultValue s názvem constraint

Při nastavování výchozí hodnoty sloupce v `IEntityTypeConfiguration<T>` vždy použij **dvouparametrovou** přetíženou verzi `HasDefaultValue`, kde druhý parametr je název DB constraint.

```csharp
builder.Property(e => e.ZipCode)
       .HasDefaultValue("", "DF_IceRink_ZipCode");
```

## Vzor pojmenování constraint

```
DF_{TabulkaBezSchématu}_{SloupecNázev}
```

Příklady:
- `DF_IceRink_ZipCode`
- `DF_SeasonCategory_BirthYears`

## Proč

EF Core bez explicitního názvu generuje náhodný název constraint (např. `DF__IceRink__ZipCode__3A4CA8FD`).  
Pojmenovaný constraint je předvídatelný, snáze se na něj odkazuje v migracích a diff skriptech.

## Reference

- `Configurations/sport/SeasonCategoryConfiguration.cs`
- `Configurations/sport/IceRinkConfiguration.cs`
