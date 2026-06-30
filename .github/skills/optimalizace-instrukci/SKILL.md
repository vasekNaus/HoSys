---
name: optimalizace-instrukci
description: >
  Analyzuje a optimalizuje veškerou Markdown dokumentaci (*.md) v repozitáři
  tak, aby byl každý dokument maximálně efektivní pro AI agenty (Copilot, Claude,
  Cursor) a zároveň přehledný pro vývojáře.
  Použij tento skill když: uživatel chce zlepšit copilot-instructions.md,
  přidat/přeorganizovat docs/, vytvořit chybějící ADR nebo skill soubory,
  nebo provést celkový audit dokumentace projektu.
user-invocable: true
---

# Optimalizace instrukcí a dokumentace projektu

## Kdy použít

- Uživatel požaduje audit nebo refaktoring `copilot-instructions.md`
- Dokumentace je příliš dlouhá, duplicitní nebo zastaralá
- Projekt postrádá `docs/architecture.md`, modulovou dokumentaci nebo ADR soubory
- Vzniká nový skill nebo implementační postup, který se opakuje
- Agent dělá chyby způsobené nesprávnou nebo chybějící dokumentací

## Předpoklady

- Přístup ke všem `.md` souborům v repozitáři
- Znalost adresářové struktury projektu
- Přehled o technologickém stacku (lze odvodit z manifestů)

---

## Postup

### Fáze 0 — Průzkum před optimalizací

1. Prohledej celý repozitář a sestav seznam všech `.md` souborů: cesta, velikost, téma, aktuálnost.
2. Pokud existuje `copilot-instructions.md`, vyhodnoť jej podle této tabulky:

   | Otázka | Signál problému |
   |---|---|
   | Je delší než 150–200 řádků? | Přetížené kontextové okno |
   | Obsahuje generické rady ("pište čistý kód")? | Zbytečné tokeny |
   | Jsou pravidla bez vysvětlení PROČ? | Nízká compliance agenta |
   | Jsou informace duplikované z jiných souborů? | Riziko rozchodu obsahu |
   | Odkazuje na neexistující soubory nebo sekce? | Matení agenta |

3. Identifikuj, co chybí:
   - `docs/architecture.md` — přehled vrstev a závislostí
   - `docs/conventions.md` — projektově specifické konvence
   - `docs/modules/` — dokumentace modulů
   - `docs/decisions/` — architektonická rozhodnutí (ADR)
   - `.github/skills/` — opakované implementační postupy

### Fáze 1 — Optimalizace copilot-instructions.md

4. Zkrať soubor na **max. 150–200 řádků**; vše nad tuto hranici přesuň do propojených dokumentů.
5. Odstraň vše, co do instrukčního souboru NEPATŘÍ:
   - generické rady (DRY, SOLID, KISS — model je zná z tréninku)
   - vysvětlení obecně známých frameworků
   - velké bloky kódu (odkazuj na soubory)
   - opakování stejného pravidla
   - informace, které agent odvodí z adresářové struktury nebo manifestu
   - firemní pozadí bez relevance pro generování kódu
6. Zajisti, aby soubor obsahoval **povinných 9 sekcí**:
   - jazyk komunikace (explicitní)
   - přehled projektu (2–5 vět, stack)
   - architektura na jednu obrazovku (tabulka vrstev nebo ASCII strom)
   - klíčová pravidla s odůvodněním (`co + proč`, vzor: `❌ NIKDY X. ✅ NIKDY X — protože Y.`)
   - build a testovací příkazy (přesná syntaxe)
   - routovací tabulka kontextu (úkol → soubory k přečtení)
   - zákazy s alternativou (`NESMÍ` = absolutní, `NEMĚLO BY` = preferenční)
   - kdy se zastavit a zeptat se
   - odkaz na zdrojové dokumenty (ne kopie obsahu)

### Fáze 2 — Tvorba nebo aktualizace docs/

7. Vytvoř nebo aktualizuj `docs/architecture.md` se sekcemi:
   - Účel systému (2–4 věty)
   - Architektonické vrstvy (tabulka: vrstva | projekt/složka | role + pravidla závislostí s odůvodněním)
   - Klíčové datové struktury (entity, vztahy, sémantické invarianty neviditelné ze schématu)
   - Integrace s externími systémy
   - Architektonická omezení (NESMĚJÍ být porušena — s odůvodněním každého)
   - Klíčové soubory (logický název → cesta)
8. Pro každý netriviální modul (vlastní schéma / veřejné API / business logika) vytvoř nebo aktualizuj `docs/modules/<modul>.md` s šablonou:
   - Účel, Odpovědnosti, Datový model, Tok zpracování, Klíčové komponenty (s cestami), Rozhraní, Integrační vazby, Závislosti, Omezení a pravidla, Příklady, Odkazovaná dokumentace
   - Optimální rozsah: **200–600 řádků**; při přesahu rozděl na více souborů

### Fáze 3 — ADR dokumenty

9. Vytvoř ADR pro každé rozhodnutí splňující alespoň jedno kritérium:
   - drahé na vrácení (koordinovaná práce, data, migrace)
   - překračuje více komponent nebo vrstev
   - uzavírá jinak otevřené možnosti
   - přidává nezanedbatelnou závislost
10. Použij MADR formát:
    ```
    # ADR-NNN: Stručný název (problém + řešení)
    Status / Datum / Rozhodující
    ## Kontext a problém
    ## Zvažované varianty
    ## Rozhodnutí
    ## Důsledky (pozitivní + negativní)
    ## Reference
    ```
11. ADR jsou **immutabilní** — při změně vytvoř nové s `Supersedes: ADR-NNN`, původní zachovej.
12. Udržuj `docs/decisions/README.md` jako index: číslo | název | status | datum.

### Fáze 4 — Skills

13. Vytvoř nový skill (`SKILL.md` v samostatném podadresáři) pokud:
    - postup se opakuje (2× a více)
    - má více kroků závislých na pořadí
    - je projektově specifický, ale ne závislý na konkrétní doméně
14. Struktura každého SKILL.md: YAML frontmatter (`name`, `description`, `user-invocable`) + sekce: Kdy použít, Předpoklady, Postup, Omezení, Checklist, Příklady.
15. **Skills nesmí obsahovat doménové znalosti systému** — popisují ZPŮSOB práce, ne OBSAH systému.

### Fáze 5 — Audit každého MD souboru

16. Pro každý `.md` soubor proveď 10bodovou analýzu:
    1. Účel — co je primární cíl dokumentu?
    2. Zařazení — patří do `copilot-instructions`, `docs/`, `skills/`, nebo `README`?
    3. Duplicity — je obsah jinde? Ponech na jednom místě, odkaž.
    4. Sloučení — překrývá se s jiným dokumentem? Slouč.
    5. Rozdělení — pokrývá více nesouvisejících témat? Rozděl.
    6. Aktuálnost — jsou informace platné? Oprav nebo odstraň zastaralé.
    7. Odkazy — jsou všechny relativní odkazy funkční? Oprav nefunkční.
    8. Terminologie — konzistentní s ostatní dokumentací?
    9. Pravidla s odůvodněním — jsou k zákazům uvedeny důvody PROČ?
    10. Navigace pro agenty — jsou kritické soubory odkazovány cestou, ne jen názvem?

### Fáze 6 — Výstupní report

17. Vytvoř souhrnný report se sekcemi:
    - Nová struktura dokumentace (cílový adresářový strom s popisy)
    - Provedené změny (nové / přesunuté / sloučené / rozdělené / aktualizované soubory)
    - Identifikované problémy (duplicity, zastaralé info, chybějící dokumentace, nefunkční odkazy)
    - Doporučení pro další rozvoj

---

## Omezení

- Skill **nesmí** měnit zdrojový kód projektu.
- Skill **nesmí** smazat ADR soubory ani je měnit — při změně vytvoř nové.
- Skill **nesmí** přidávat obsah, který agent může odvodit z kódu, manifestu nebo adresářové struktury sám.
- Skill **nesmí** kopírovat stejné informace na více míst — vždy jeden zdroj pravdy.
- Do `copilot-instructions.md` nesmí přibýt generické rady ani obsah překračující 200 řádků.

---

## Checklist

- [ ] Všechny `.md` soubory v repozitáři zmapovány
- [ ] `copilot-instructions.md` auditován (délka, generické rady, duplicity, odkazy)
- [ ] Povinných 9 sekcí v `copilot-instructions.md` přítomno
- [ ] `docs/architecture.md` existuje a je aktuální
- [ ] Každý netriviální modul má `docs/modules/<modul>.md`
- [ ] Přijatá architektonická rozhodnutí mají ADR soubor
- [ ] `docs/decisions/README.md` index aktualizován
- [ ] Opakované postupy mají skill v `.github/skills/`
- [ ] Žádné duplicitní informace mezi soubory
- [ ] Všechny relativní odkazy jsou funkční
- [ ] Výstupní report sestaven

---

## Příklady

### Pravidlo s odůvodněním (copilot-instructions.md)

```markdown
❌ Neregistruj DbContext přímo v Razor projektu.
✅ NIKDY neregistruj DbContext přímo v Razor projektu — registrace probíhá
   výhradně v `ServiceCollectionExtensions.cs` v Contract vrstvě, aby
   nedocházelo k duplikacím a porušení vrstevnaté architektury.
```

### Routovací tabulka kontextu

```markdown
## Kontext pro konkrétní úkoly

| Úkol zahrnuje... | Přečti tyto soubory |
|---|---|
| databázové modely | docs/architecture.md, docs/conventions.md |
| modul Inventory | docs/modules/inventory.md |
| architektonické rozhodnutí | docs/decisions/ |
| nový implementační postup | .github/skills/ |
```

### Identifikace antipatternu

| Antipattern | Problém | Řešení |
|---|---|---|
| Pravidlo bez odůvodnění | Agent neví, zda je "load-bearing" nebo stylový preference | Přidej PROČ na stejný řádek |
| Dvě kopie stejné informace | Agent neví, která je kanonická | Jeden zdroj pravdy, ostatní odkazují |
| Monolitický dokument | Kontext přetéká; agent nezachytí relevantní část | Rozděl na soubory po jednom tématu |
| "Future enhancements" v docs | Agent může hallucindovat neimplementované funkce | Přesuň do issues |

---

## Priorita při konfliktu dokumentačních zdrojů

1. Specifikace a schémata (YAML, JSON schema) — nejvyšší priorita
2. ADR soubory (`docs/decisions/`) — závazná architektonická pravidla
3. `.github/copilot-instructions.md` — globální konvence
4. Path-specific instructions — přepíše pro konkrétní scope
5. Skills — postup, ne pravidlo; nejnižší priorita

---

## Reference

Tento skill vychází z rešerše `.github/skills/optimalizace-instrukci/reference/Rešerše.md`, která čerpá z:
GitHub Copilot docs (June 2026), Anthropic ACI design principles, MADR formátu a vzorů z open-source projektů (hailo-ai, woocommerce, recyclarr, adr/madr).
