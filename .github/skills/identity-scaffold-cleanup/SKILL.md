---
name: identity-scaffold-cleanup
description: >
  Provede povinný cleanup po scaffoldingu ASP.NET Core Identity stránek.
  Použij tento skill bezprostředně po spuštění příkazu
  `dotnet aspnet-codegenerator identity` — jinak aplikace spadne
  s chybou "Scheme already exists: Identity.Application".
user-invocable: true
---

# Cleanup po scaffoldingu Identity stránek

## Kdy použít

Bezprostředně po spuštění:
```bash
dotnet aspnet-codegenerator identity -dc SportSys.Database.SportSysDbContext --files "Account.Login;Account.Register;..."
```

## Proč

Scaffolder automaticky vloží do `Program.cs` kód, který:
1. Duplikuje registraci `DbContext` (již registrována v `AddSportSysServices`)
2. Volá `AddDefaultIdentity` → koliduje s `AddIdentityCore` v `AddSportSysServices`
3. Způsobí runtime výjimku: `"Scheme already exists: Identity.Application"`

## Postup

### 1. Smazat 3 řádky z `Program.cs`

Scaffolder vloží přibližně tyto řádky — smazat **všechny tři**:

```csharp
// ❌ Smazat — špatný connection string key
var connectionString = builder.Configuration.GetConnectionString("SportSysDbContext")
    ?? throw new InvalidOperationException("...");

// ❌ Smazat — duplikát registrace DbContext
builder.Services.AddDbContext<SportSys.Razor.Data.SportSysDbContext>(options =>
    options.UseSqlServer(connectionString));

// ❌ Smazat — koliduje s AddSportSysServices
builder.Services.AddDefaultIdentity<SportSys.Razor.Data.ApplicationUser>(options =>
    options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<SportSys.Razor.Data.SportSysDbContext>();
```

Tyto věci jsou již registrovány v `AddSportSysServices()` — ponechat pouze:
```csharp
builder.Services.AddSportSysServices(builder.Configuration);
```

### 2. Opravit using direktivy ve scaffoldovaných stránkách

V každém scaffoldovaném souboru v `Areas/Identity/Pages/Account/`:

```csharp
// ❌ Špatně (scaffolded)
using SportSys.Razor.Data;

// ✅ Správně
using SportSys.Database.Models.identity;
```

Nahradit i všechny výskyty třídy `ApplicationUser` za `User`:
```csharp
// ❌ Špatně
private readonly UserManager<ApplicationUser> _userManager;

// ✅ Správně
private readonly UserManager<User> _userManager;
```

### 3. Smazat vygenerované pomocné soubory (pokud existují)

Scaffolder může vytvořit `Areas/Identity/Data/` se soubory `ApplicationUser.cs` a `SportSysDbContext.cs` — **smazat celou složku**:
```
Areas/Identity/Data/   ← smazat
```

### 4. Ověřit sestavení

```bash
dotnet build SportSys.slnx
```

Aplikace nesmí vykazovat chybu `"Scheme already exists"` ani kompilační chyby.

### 5. Spustit aplikaci a ověřit login flow

```bash
dotnet run --project src/SportSys.Razor
```

Ověřit:
- Přihlášení přes Entra ID funguje (OIDC redirect)
- Scaffoldované stránky jsou dostupné
- Žádná výjimka `InvalidOperationException`

## Omezení

- ❌ Nikdy volat `AddIdentity<T>()` — pouze `AddIdentityCore<User>()` + `.AddSignInManager()`
- ❌ Nikdy přidávat druhé `AddDbContext` do Razor projektu
- Jediné místo registrace: `AddSportSysServices()` v `SportSys.Contract/ServiceCollectionExtensions.cs`

## Checklist

- [ ] 3 scaffoldované řádky z `Program.cs` smazány
- [ ] `using SportSys.Razor.Data` → `using SportSys.Database.Models.identity` ve všech scaffoldovaných stránkách
- [ ] `ApplicationUser` → `User` ve všech scaffoldovaných stránkách
- [ ] `Areas/Identity/Data/` smazána (pokud existuje)
- [ ] `dotnet build` proběhne bez chyb
- [ ] Login flow ověřen

## Reference

- `docs/modules/auth.md` — architektura autentizace
- `src/SportSys.Contract/ServiceCollectionExtensions.cs` — jediné místo registrace
