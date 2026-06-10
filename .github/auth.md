# GitHub Copilot Instructions – ASP.NET Core Razor Pages + Entra ID

## Cíl

Implementovat ASP.NET Core Razor Pages aplikaci s:

- primární autentizací přes Microsoft Entra ID
- fallback lokálními účty pro externí uživatele
- jednotným identity modelem přes ASP.NET Core Identity
- business autorizací spravovanou v aplikaci
- policy-based authorization
- auditní vazbou na uživatele

---

## Autentizace

### Interní uživatelé
Používat:
- Microsoft Entra ID
- OpenID Connect
- SSO + MFA

### Externí/fallback uživatelé
Používat:
- lokální ASP.NET Core Identity účty

---

## ASP.NET Core Identity

Použít standardní ASP.NET Core Identity.

Nepoužívat vlastní identity systém.

Používat:
- UserManager
- SignInManager
- Identity cookies
- external login support

---

## ApplicationUser

```csharp
public class ApplicationUser : IdentityUser
{
    public string? EntraOid { get; set; }

    public string? EntraTenantId { get; set; }

    public string? DisplayName { get; set; }

    public bool IsLocalAccount { get; set; }

    public DateTime? LastLoginUtc { get; set; }
}
```

---

## Identifikace uživatelů

Nikdy nepoužívat:
- Email
- UserPrincipalName

Pro Entra ID používat:
- oid
- tid

z claims tokenu.

---

## Vazba na business data

Všechny business entity referencovat přes:

```csharp
ApplicationUser.Id
```

Příklad:

```csharp
public string CreatedByUserId { get; set; } = default!;

public ApplicationUser CreatedByUser { get; set; } = default!;
```

---

## Identity tabulky

Použít standardní Identity tabulky:

- AspNetUsers
- AspNetRoles
- AspNetUserRoles
- AspNetUserClaims
- AspNetUserLogins
- AspNetUserTokens

---

## Authorization

### DŮLEŽITÉ

Business oprávnění NEukládat do:
- AspNetRoles
- AspNetUserClaims

Identity role používat pouze pro:
- SystemAdmin
- Support
- InternalUser

---

## Business authorization model

### Permissions

```text
Id
Code
Name
Description
```

### BusinessRoles

```text
Id
Code
Name
```

### BusinessRolePermissions

```text
BusinessRoleId
PermissionId
```

### UserBusinessRoles

```text
UserId
BusinessRoleId
```

---

## Entra ID role/skupiny

Entra ID řeší:
- vstup do aplikace
- enterprise governance
- high-level role

Preferovat:
- App Roles

Aplikace řeší:
- detailní business permissions

---

## Login flow

### Entra login

1. získat claims:
   - oid
   - tid
   - roles/groups
2. najít uživatele podle:
   - EntraOid
   - EntraTenantId
3. pokud neexistuje:
   - automaticky vytvořit ApplicationUser
4. synchronizovat:
   - DisplayName
   - Email
   - LastLoginUtc
5. načíst business permissions z DB
6. doplnit claims

---

## Claims transformation

Použít:

```csharp
IClaimsTransformation
```

pro dynamické doplnění:
- permissions
- business roles
- feature flags

---

## Authorization policies

Používat:
- policy-based authorization
- claims-based authorization

Příklad:

```csharp
options.AddPolicy("invoice.approve",
    policy => policy.RequireClaim("permission", "invoice.approve"));
```

---

## NuGet balíčky

```text
Microsoft.Identity.Web
Microsoft.AspNetCore.Identity.EntityFrameworkCore
Microsoft.EntityFrameworkCore.SqlServer
```

---

## Co nedělat

Nepoužívat:
- email jako identity klíč
- UPN jako identity klíč
- business permissions v AspNetRoles
- vlastní autentizační systém

Nekopírovat kompletní Entra identity do databáze.

---

## Rozdělení odpovědností

### Entra ID
Řeší:
- autentizaci
- MFA
- SSO
- enterprise governance

### ASP.NET Core Identity
Řeší:
- user store
- hybrid login model
- sessions
- local/external login support

### Aplikace
Řeší:
- business authorization
- permissions
- ownership
- audit
- doménová pravidla