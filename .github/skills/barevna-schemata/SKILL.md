---
name: barevna-schemata
description: "Systém barevných schémat pro webové aplikace sportovních klubů. Obsahuje principy design systému, barevné tokeny (light/dark mode), WCAG 2.1 AA přístupnost a hotové schéma pro HC Klatovy. Použij tento skill při práci s CSS barvami, designem komponent nebo přidáváním nového klubového schématu."
---

# Skill: Barevná schémata pro sportovní weby

Tento skill popisuje systém barevných schémat pro webové aplikace sportovních klubů — od výzkumu klubové identity přes definici tokenů až po implementaci v CSS.

---

## Klíčový princip

Tříúrovňová hierarchie tokenů zajišťuje konzistenci a snadnou změnu schématu:

```
Layer 1 — PRIMITIVNÍ TOKENY   (např. --sport-red-500)
           ↓ nikdy přímo v komponentách
Layer 2 — SÉMANTICKÉ TOKENY   (např. --color-brand-primary)
           ↓ tyto se používají v CSS komponent
Layer 3 — KOMPONENTOVÉ TOKENY (např. --button-bg) [volitelné]
```

**Nikdy nepoužívej primitivní tokeny přímo v CSS komponent** — vždy sémantické tokeny s prefixem `--color-`.

---

## Dostupná schémata

| Složka | Schéma | Klub |
|--------|--------|------|
| [`hc-klatovy/`](hc-klatovy/) | Sport | HC Klatovy — červená + námořní modrá + bílá + heraldická zlatá |

---

## Pravidla (musí být splněna vždy)

1. **Nikdy** nepoužívej primitivní tokeny (`--sport-red-500`) přímo v CSS — použij sémantické (`--color-brand-primary`)
2. Všechny kombinace textu a pozadí musí splňovat **WCAG 2.1 AA** (min. 4,5:1 pro normální text, 3:1 pro velký text a UI prvky)
3. **Nevytvářej nová schémata bez výzkumu** — analýza CSS webu klubu + heraldika města + Wikipedie
4. Pro červenou jako barvu textu v malém písmu ověř kontrast — splňuje AA, ale ne AAA

---

## Přepínání módů

```html
<!-- Světlý mód (výchozí) -->
<html data-theme="light">

<!-- Tmavý mód -->
<html data-theme="dark">
```

```javascript
// JavaScript přepínač
document.documentElement.setAttribute(
  'data-theme',
  document.documentElement.getAttribute('data-theme') === 'dark' ? 'light' : 'dark'
);
```

---

## Jak přidat nové schéma

1. Vytvoř novou složku s názvem klubu (např. `fc-plzen/`)
2. Zkopíruj strukturu ze složky `hc-klatovy/` jako šablonu
3. Proveď výzkum (viz `02-workflow.md`):
   - Analyzuj CSS webu klubu (hex hodnoty primárních barev)
   - Zjisti heraldiku města (SVG znaku na Wikimedia Commons)
   - Ověř klubové barvy na Wikipedii
4. Definuj primitivní tokeny s prefixem schématu (např. `--fc-plzen-*`)
5. Definuj sémantické tokeny (`--color-*`) pro light a dark mode
6. Ověř WCAG 2.1 AA kontrasty — [WebAIM Contrast Checker](https://webaim.org/resources/contrastchecker/)
7. Zdokumentuj rationale a zdroje v `research.md`

---

## Podrobná dokumentace (v tomto skill adresáři)

| Soubor | Obsah |
|--------|-------|
| [01-principles.md](01-principles.md) | Psychologie barev, Dark/Light mode, architektura tokenů, WCAG 2.1 |
| [02-workflow.md](02-workflow.md) | Postup výzkumu a přidání nového schématu |
| [hc-klatovy/palette.md](hc-klatovy/palette.md) | Primitivní tokeny HC Klatovy — surové hex hodnoty |
| [hc-klatovy/tokens.md](hc-klatovy/tokens.md) | Sémantické tokeny — Light Mode a Dark Mode |
| [hc-klatovy/usage.md](hc-klatovy/usage.md) | Příklady CSS komponent, přepínání módů |
| [hc-klatovy/research.md](hc-klatovy/research.md) | Výzkum — analýza webu, heraldika, klubové barvy |
| [_research/research.md](_research/research.md) | Originální výzkumná rešerše (surový výstup) |
