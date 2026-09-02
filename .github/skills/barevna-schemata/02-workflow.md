# Workflow — Přidání nového barevného schématu

Postup pro výzkum klubové identity a vytvoření nového barevného schématu.

---

## Struktura schématu

Každé schéma má vlastní podsložku s těmito soubory:

| Soubor | Obsah |
|--------|-------|
| `research.md` | Výzkum — analýza webu klubu, heraldika, klubové barvy |
| `palette.md` | Primitivní tokeny (surové hex hodnoty) + souhrn palety |
| `tokens.md` | Sémantické tokeny — light mode a dark mode |
| `usage.md` | Příklady použití v CSS, přepínání módů, confidence assessment |

Vzorová šablona struktury je ve složce [`hc-klatovy/`](hc-klatovy/).

---

## Postup výzkumu

### Krok 1 — Analýza webu klubu

1. Otevři CSS soubory webu klubu (DevTools → Sources nebo přímé URL `style.min.css`)
2. Identifikuj hex hodnoty:
   - Primární barva (tlačítka, CTA, zvýraznění)
   - Sekundární barva (navigace, nadpisy)
   - Pozadí stránky a karet
   - Barva textu
3. Zaznamenej do tabulky s poznámkou kde se barva používá

### Krok 2 — Heraldika města

1. Najdi SVG znaku města na [Wikimedia Commons](https://commons.wikimedia.org/)
   - Hledat: `{název-města}-znak.svg` nebo `Coat_of_arms_of_{city}.svg`
2. Otevři SVG soubor a odečti hodnoty atributů `fill`
3. Dokumentuj heraldické prvky:
   - Barva štítu (pole)
   - Barvy figur (zvíře, stavba, symbol)
   - Barvy doplňků (makovice, střechy)

### Krok 3 — Ověření klubových barev

1. Česká Wikipedie — hledej `{název klubu}` → sekce „Klubové barvy"
2. Oficiální web klubu — logo, dressy, merchandise
3. Porovnej s výsledky analýzy CSS (měly by souhlasit)

### Krok 4 — Psychologická analýza

Na základě barev z kroků 1–3 aplikuj principy z [`01-principles.md`](01-principles.md):
- Jaký pocit barvy evokují?
- Hodí se pro sportovní web?
- Jsou konzistentní s heraldickou identitou?

---

## Definice primitivních tokenů

Prefix schématu je `--{zkratka-klubu}-` (např. `--hc-klatovy-` → zkrátit na `--sport-` pro generické schéma).

```css
:root {
  /* Červená škála */
  --{prefix}-red-50:  #{nejsvětlejší tint};
  /* ... */
  --{prefix}-red-500: #{hlavní červená};   /* ← značková barva */
  /* ... */
  --{prefix}-red-900: #{nejtmavší};

  /* Stejná struktura pro každou hlavní barvu */
}
```

**Konvence pojmenování:** `{prefix}-{barva}-{číslo}` kde číslo 500 = základní, nižší = světlejší, vyšší = tmavší.

---

## Definice sémantických tokenů

Použij strukturu z `hc-klatovy/tokens.md` jako šablonu. Povinné kategorie:

```css
:root, [data-theme="light"] {
  --color-bg-base
  --color-bg-subtle
  --color-surface-1, -2, -3, -inverse
  --color-brand-primary (+ -hover, -active, -subtle)
  --color-on-brand-primary
  --color-brand-secondary (+ varianty)
  --color-text-primary, -secondary, -muted, -disabled, -inverse, -brand
  --color-border-subtle, -default, -strong, -focus, -brand
  --color-interactive-hover, -active, -selected
  --color-success, -warning, -error, -info (+ -text, -subtle varianty)
  --shadow-sm, -md, -lg, -brand
  --color-overlay
}

[data-theme="dark"], .dark {
  /* Stejné tokeny s přizpůsobenými hodnotami pro tmavé pozadí */
}
```

---

## Ověření WCAG 2.1 AA

Po definici tokenů ověř minimálně tyto kombinace:

| Kombinace | Minimální kontrast |
|-----------|-------------------|
| Primární text na pozadí stránky | 4,5:1 |
| Bílý text na primárním tlačítku | 4,5:1 |
| Primární text na povrchu karty | 4,5:1 |
| Značková barva jako text na bílé | 4,5:1 |
| Sekundární text na pozadí | 4,5:1 |

Používej [WebAIM Contrast Checker API](https://webaim.org/resources/contrastchecker/?fcolor=FFFFFF&bcolor=D6232A&api) pro automatické ověření.

---

## Soubor `research.md`

Musí obsahovat:
- Datum analýzy a verzi
- Tabulku identifikovaných barev z CSS webu klubu (hex + rgb + role)
- Heraldické barvy z SVG znaku (hex + heraldický název + prvek)
- Srovnání CSS barev s heraldickými
- Hodnocení spolehlivosti zdrojů (jistý / odvozeno / spekulace)
- Poznámky pod čarou s URL zdrojů

---

## Co dělat / Co nedělat

**✅ Dělej:**
- Pomáhej s výběrem správných CSS proměnných (`--color-*`) pro konkrétní UI komponenty
- Ověřuj kontrast a splnění WCAG 2.1 AA pro navrhované kombinace barev
- Generuj CSS kód používající **sémantické tokeny** (nikdy přímé hex hodnoty)
- Navrhuj nová schémata na základě výzkumu klubové identity a heraldiky
- Vysvětluj psychologické důvody volby barev
- Pomáhej s implementací přepínání light/dark módu

**❌ Nedělej:**
- Nikdy nepoužívej primitivní tokeny (např. `--sport-red-500`) přímo v CSS komponent
- Neposkytuj kombinace barev nesplňující WCAG 2.1 AA
- Nevytvářej nová schémata bez výzkumu klubové identity, heraldiky a přímé analýzy CSS webu
