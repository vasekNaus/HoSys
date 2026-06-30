# Frontend — SportSys

## Přehled

Žádný CSS framework (Bootstrap, Tailwind apod.). Vlastní SCSS kompilovaný do `wwwroot/css/site.css`.

| Vrstva | Technologie |
|---|---|
| Šablonování | Razor Pages (CSHTML) |
| CSS | Vlastní SCSS → `wwwroot/css/site.css` |
| JavaScript | Vanilla JS (minimální, bez frameworků) |
| Ikony | Font Awesome 6 (CDN) |
| Build nástroj | npm + sass |

---

## SCSS struktura

```
src/SportSys.Razor/Styles/
├── site.scss              ← entry point (importuje vše)
├── _vars.scss             ← design tokeny (barvy, fonty, breakpointy)
├── _layout.scss           ← html/body/header/nav/footer
├── _forms.scss            ← formuláře, buttony, validace
├── _grid.scss             ← tabulkové komponenty
└── ...
```

> ⚠️ `wwwroot/css/site.css` je **kompilovaný výstup** — ❌ nikdy neupravovat ručně. Změny patří do `Styles/*.scss`.

Kompilace:
```bash
cd src/SportSys.Razor
npm run build:css    # jednorázová kompilace
npm run watch:css    # sledování změn
```

---

## Design tokeny (`_vars.scss`)

Design tokeny jsou definovány jako **CSS custom properties** v `_vars.scss`. ❌ Nikdy používat přímé hex hodnoty v komponentových souborech.

```scss
// Primitivní tokeny (barvy HC Klatovy — nedotýkej se)
--sport-red-500: #d8232a;
--sport-navy-700: #0d1b3e;
--sport-gold-400: #c9a227;

// Sémantické tokeny (používat v komponentách)
--color-brand-primary:        var(--sport-red-500);
--color-brand-primary-active: #b01c22;   // tmavší — výhradně :hover/:active
--color-brand-secondary:      var(--sport-navy-700);
--color-brand-accent:         var(--sport-gold-400);
```

**Pravidlo barev:**
- `--color-brand-primary` (`#d8232a`) — tlačítka, navigace, hlavní akcenty
- `--color-brand-primary-active` — **výhradně** pro `:hover`/`:active` stavy, ❌ nikoli jako alternativní sekce
- Sémantické tokeny (`--color-*`) mají přednost před primitivními (`--sport-red-500`)

Barevné schéma HC Klatovy definováno v `.github/barevna-schemata/hc-klatovy/`:
- `palette.md` — kompletní paleta
- `tokens.md` — CSS custom properties
- `usage.md` — pravidla použití

---

## Tlačítka

```html
<!-- Primární akce -->
<button type="submit" class="button">Uložit</button>

<!-- Sekundární akce (zrušit, zpět) -->
<a class="button secondary" asp-page="Index">Zpět</a>

<!-- Nebezpečná akce (smazat) -->
<button type="submit" class="button tertiary">Smazat</button>

<!-- Ikonové tlačítko v gridu (bez textu) -->
<a class="button secondary icon-btn" asp-page="Edit" asp-route-id="@item.Id" title="Upravit">
    <i class="fa-solid fa-pen fa-fw"></i>
</a>
```

---

## Ikony (Font Awesome 6)

Každá ikona musí mít třídu `fa-fw` (pevná šířka).

**Akční tlačítka v řádcích gridu** — vždy pouze ikona s `title`:

```html
@* Detail *@
<a class="button secondary icon-btn" asp-page="Detail" asp-route-id="@item.Id" title="Detail">
    <i class="fa-solid fa-eye fa-fw"></i>
</a>

@* Editace *@
<a class="button secondary icon-btn" asp-page="Edit" asp-route-id="@item.Id" title="Upravit">
    <i class="fa-solid fa-pen fa-fw"></i>
</a>

@* Smazání *@
<button type="submit" class="button tertiary icon-btn" title="Smazat">
    <i class="fa-solid fa-trash fa-fw"></i>
</button>
```

**Standardní ikonový slovník:**

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

Tlačítka mimo gridy (nadpisové akce, formulářová tlačítka) mohou mít text vedle ikony. Tlačítka v kategoriích stromu a podobných strukturách také používají `icon-btn`.

---

## Standardní stránka se seznamem

```cshtml
<table class="grid">
    <thead>
        <tr>
            <th>Název</th>
            <th class="buttons"></th>
        </tr>
    </thead>
    <tbody>
        @foreach (var item in Model.Items)
        {
            <tr>
                <td>@item.Name</td>
                <td class="buttons">
                    <a class="button secondary icon-btn" asp-page="Detail"
                       asp-route-id="@item.Id" title="Detail">
                        <i class="fa-solid fa-eye fa-fw"></i>
                    </a>
                    <a class="button secondary icon-btn" asp-page="Edit"
                       asp-route-id="@item.Id" title="Upravit">
                        <i class="fa-solid fa-pen fa-fw"></i>
                    </a>
                </td>
            </tr>
        }
    </tbody>
</table>
```

---

## Standardní formulářová stránka

```cshtml
<form method="post">
    <div class="field">
        <label asp-for="Input.Name"></label>
        <input asp-for="Input.Name" />
        <span asp-validation-for="Input.Name"></span>
    </div>

    <footer>
        <div asp-validation-summary="ModelOnly"></div>
        <button type="submit" class="button">
            <i class="fa-solid fa-floppy-disk fa-fw"></i> Uložit
        </button>
        <a class="button secondary" asp-page="Index">
            <i class="fa-solid fa-arrow-left fa-fw"></i> Zpět
        </a>
    </footer>
</form>
```

---

## JavaScript

Vanilla JS bez frameworků. JS pouze tam, kde server-side logiku nelze použít.

---

## Reference

- `.github/barevna-schemata/hc-klatovy/` — kompletní barevné schéma HC Klatovy
- `docs/research/html-css-reference.md` — rešerše přístupu projektu ReP (inspirace)
