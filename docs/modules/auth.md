# Autentizace a autorizace — SportSys

## Architektura identity

| Vrstva | Technologie | Role |
|---|---|---|
| Primární autentizace | Microsoft Entra ID (OIDC, SSO, MFA) | Interní uživatelé |
| Fallback | Lokální ASP.NET Core Identity účty | Přímý přístup bez Entra |
| User store | ASP.NET Core Identity (`IdentityUser<int>`) | Jednotný model |
| Business autorizace | Policy-based + Claims transformation | Detailní oprávnění |

---

## Model uživatele (`User`)

Třída `User` v `Models/identity/` rozšiřuje `IdentityUser<int>`:

```csharp
public class User : IdentityUser<int>
{
    public string? EntraOid { get; set; }         // OID z Entra ID tokenu
    public string? EntraTenantId { get; set; }    // TID z Entra ID tokenu
    public string? DisplayName { get; set; }
    public bool IsLocalAccount { get; set; }
    public DateTime? LastLoginUtc { get; set; }
}
```

> ❌ Nikdy nepoužívat email ani UPN jako identity klíč pro Entra uživatele — použít `EntraOid` + `EntraTenantId`.

---

## Identity tabulky

Identity tabulky jsou ve schématu `identity` (ne výchozí `dbo`), **bez** `AspNet` prefixu:

| Tabulka | Mapuje na |
|---|---|
| `identity.User` | `IdentityUser<int>` |
| `identity.Role` | `IdentityRole<int>` |
| `identity.UserRole` | `IdentityUserRole<int>` |
| `identity.UserClaim` | `IdentityUserClaim<int>` |
| `identity.UserLogin` | `IdentityUserLogin<int>` |
| `identity.UserToken` | `IdentityUserToken<int>` |
| `identity.RoleClaim` | `IdentityRoleClaim<int>` |

---

## Registrace servisů

Veškerá registrace probíhá výhradně přes `AddSportSysServices()` v `SportSys.Contract/ServiceCollectionExtensions.cs`.

> ❌ `AddIdentity<T>()` NESMÍ být použito — nahrazuje OIDC jako výchozí autentizační schéma, přeruší přihlašování přes Entra ID.

> ✅ Vždy `AddIdentityCore<User>()` + `.AddSignInManager()`.

---

## FrameworkReference (ne NuGet)

`SportSys.Database` a `SportSys.Contract` referencují ASP.NET Core Identity přes:

```xml
<FrameworkReference Include="Microsoft.AspNetCore.App" />
```

❌ Ne NuGet balíček `Microsoft.AspNetCore.Identity` — na .NET 10 by kolidoval.

---

## Authorization

Identity role pouze pro: `SystemAdmin`, `Support`, `InternalUser`.

> ❌ Business oprávnění NESMÍ být ukládána do Identity rolí ani claims — narušuje oddělení odpovědností a ztěžuje správu.

Business autorizace je implementována přes `EntraClaimsTransformation` (implementuje `IClaimsTransformation`) — dynamicky doplňuje claims z databáze po přihlášení.

```csharp
// Policy-based authorization (příklad)
options.AddPolicy("invoice.approve",
    policy => policy.RequireClaim("permission", "invoice.approve"));
```

---

## Login flow (Entra ID)

1. OIDC callback → claims obsahují `oid` (EntraOid) a `tid` (EntraTenantId)
2. Vyhledat `User` podle `EntraOid` + `EntraTenantId`
3. Pokud neexistuje → automaticky vytvořit
4. Synchronizovat `DisplayName`, `Email`, `LastLoginUtc`
5. `EntraClaimsTransformation.TransformAsync` → načíst business oprávnění z DB → doplnit claims

---

## Scaffolding Identity stránek — povinný cleanup

Po `dotnet aspnet-codegenerator identity` scaffolder vloží do `Program.cs` 3 řádky, které **musí být okamžitě smazány** (způsobí `"Scheme already exists: Identity.Application"`):

```csharp
// ❌ Smazat:
var connectionString = builder.Configuration.GetConnectionString("SportSysDbContext");
builder.Services.AddDbContext<SportSys.Razor.Data.SportSysDbContext>(...);
builder.Services.AddDefaultIdentity<SportSys.Razor.Data.ApplicationUser>(...);
```

Scaffoldované stránky v `Areas/Identity/Pages/` — přejmenovat `using SportSys.Razor.Data` na `using SportSys.Database.Models.identity`.

Viz `.github/skills/identity-scaffold-cleanup/SKILL.md` pro krok-za-krokem postup.

---

## Reference

- `src/SportSys.Contract/ServiceCollectionExtensions.cs` — jediné místo registrace
- `src/SportSys.Database/Models/identity/` — identity modely
- `.github/skills/identity-scaffold-cleanup/SKILL.md` — postup po scaffoldingu
