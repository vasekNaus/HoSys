# Instrukce pro optimalizaci dokumentace projektu pro AI agenty

> **Verze:** 1.0 · **Účel:** Opakovaně použitelný instrukční soubor pro optimalizaci `copilot-instructions.md` a projektové dokumentace na libovolném projektu. Spouští se nad konkrétním projektem; obecná pravidla se pak konkretizují dle jeho struktury a technologií.

---

## Účel a kontext

Proveď analýzu a optimalizaci veškeré Markdown dokumentace (`*.md`) v repozitáři tak, aby byl každý dokument maximálně efektivní pro práci AI agentů (GitHub Copilot, Claude Code, Cursor a obdobné nástroje) a zároveň zůstal přehledný pro lidské vývojáře.

Hlavní cíl: vytvořit **jasně strukturovanou znalostní základnu** (hub-and-spoke model), která umožní agentům rychle nalézt relevantní informace bez načítání zbytečného kontextu.

> **Klíčový princip:** Do instrukčního souboru patří POUZE informace, které by agent bez nich neodvodil z kódu, manifestu nebo adresářové struktury. Generické rady, které model zná z tréninku, jsou zbytečné — snižují poměr signál/šum v kontextovém okně.

---

## Fáze 0 — Průzkum před optimalizací

Před jakýmkoli zápisem proveď inventuru aktuálního stavu.

### 0.1 Mapování existující dokumentace

Prohledej celý repozitář a vytvoř seznam všech `.md` souborů. Pro každý soubor zaznamenej:
- úplnou cestu,
- odhadovanou velikost,
- primární téma,
- aktuální správnost (zastaralý obsah, poškozené odkazy).

### 0.2 Audit copilot-instructions.md

Pokud soubor existuje, vyhodnoť:

| Otázka | Signál problému |
|---|---|
| Je delší než 150–200 řádků? | Přetížené kontextové okno |
| Obsahuje generické rady ("pište čistý kód")? | Zbytečné tokeny |
| Jsou pravidla bez vysvětlení PROČ? | Nízká compliance agenta |
| Jsou informace duplikované z jiných souborů? | Riziko rozchodu obsahu |
| Odkazuje na neexistující soubory nebo sekce? | Matení agenta |

### 0.3 Identifikace chybějící dokumentace

Zjisti, co chybí:
- `docs/architecture.md` — přehled vrstev a závislostí
- `docs/conventions.md` — konvence kódu projektu
- `docs/modules/` — dokumentace modulů
- `docs/decisions/` — architektonická rozhodnutí (ADR)
- `.github/skills/` — opakované implementační postupy

---

## Architektura dokumentace (hub-and-spoke model)

Cílová struktura, ke které optimalizace směřuje:

```
.github/copilot-instructions.md      ← Navigační hub (max. 150–200 řádků)
                                        Vždy v kontextu agenta
docs/
  architecture.md                    ← Architektura systému (vrstvy, závislosti)
  conventions.md                     ← Konvence kódu (specifické pro projekt)
  modules/
    <modul-a>.md                     ← Jeden soubor = jeden modul
    <modul-b>.md
  decisions/
    adr-001-<rozhodnutí>.md          ← Jedno ADR = jedno rozhodnutí
    adr-002-<rozhodnutí>.md
.github/
  skills/
    <skill-name>/
      SKILL.md                       ← Opakovaný implementační postup
  instructions/
    <oblast>.instructions.md         ← Volitelné path-specific instrukce
README.md                            ← Vstupní rozcestník pro lidi
```

**Pravidlo oddělení odpovědností:**

| Soubor | Obsah | Výstupní publikum |
|---|---|---|
| `copilot-instructions.md` | Navigace, klíčová pravidla s odůvodněním, routovací tabulka | Agent (vždy načten) |
| `docs/architecture.md` | Jak systém vypadá TEĎ | Agent i vývojář |
| `docs/decisions/` | PROČ systém vypadá tak jak vypadá | Agent i vývojář |
| `docs/conventions.md` | Projektově specifické konvence | Agent i vývojář |
| `docs/modules/` | Co každý modul dělá, datový model, integrace | Agent i vývojář |
| `.github/skills/` | JAK provádět opakované úkoly (postup, checklist) | Agent (na vyžádání) |
| `README.md` | Stručný popis projektu a odkazy | Vývojář |

---

## Pravidla pro copilot-instructions.md

### Povinný obsah

1. **Sekce o jazyce komunikace** — explicitně určuje jazyk, ve kterém agent komunikuje, bez ohledu na jazyk promptu.

2. **Přehled projektu** — 2–5 vět: co systém dělá, kdo ho používá, technologický stack.

3. **Architektura na jednu obrazovku** — tabulka vrstev nebo ASCII strom adresářů s anotacemi. Pouze to, co by agent z kódu sám neodvodil.

4. **Klíčová pravidla s odůvodněním** — každé pravidlo má formát: co + proč. Vysvětlení motivace zvyšuje compliance a pomáhá agentovi aplikovat pravidlo i na neanticipované situace.

   Vzor:
   ```
   ❌ NIKDY neregistruj DbContext přímo v Razor projektu.
   ✅ NIKDY neregistruj DbContext přímo v Razor projektu — registrace probíhá výhradně
      v `ServiceCollectionExtensions.cs` v Contract vrstvě, aby nedocházelo k duplikacím
      a porušení vrstevnaté architektury.
   ```

5. **Build a testovací příkazy** — přesná syntaxe, ne jen "spusť testy". Umožňuje agentovi autonomní verify-loop.

6. **Routovací tabulka kontextu** (pro agentic workflows) — tabulka, která říká agentovi, jaké soubory číst pro jaký typ úkolu. Snižuje zbytečné prohledávání.

   Vzor:
   ```markdown
   ## Kontext pro konkrétní úkoly
   | Úkol zahrnuje... | Přečti tyto soubory |
   |---|---|
   | databázové modely | docs/architecture.md, docs/conventions.md |
   | modul X | docs/modules/x.md |
   | architektonické rozhodnutí | docs/decisions/ |
   | nový implementační postup | .github/skills/ |
   ```

7. **Zákazy s alternativou** — `NESMÍ` pro absolutní zákazy, `NEMĚLO BY` pro preferenční pravidla. Vždy párovat se správnou alternativou.

8. **Kdy se zastavit a zeptat se** — explicitní výčet situací, kdy agent nesmí pokračovat bez lidského vstupu.

9. **Odkaz na zdrojové dokumenty** — `Viz docs/architecture.md`, ne kopírování obsahu.

### Co do copilot-instructions.md NEPATŘÍ

- Generické programátorské rady (DRY, SOLID, KISS — model je zná z tréninku)
- Vysvětlení obecně známých frameworků a knihoven
- Velké bloky kódu — odkazuj na konkrétní soubory
- Opakování stejného pravidla vícekrát (opakování nezvyšuje compliance — struktura ano)
- Informace, které agent odvodí z adresářové struktury nebo manifestu
- Rozsáhlá architektura (patří do `docs/architecture.md`)
- Firemní nebo produktové pozadí bez relevance pro generování kódu

### Pravidlo tokenu: max. 150–200 řádků

Vše nad tuto hranici přesuň do propojených souborů. Kratší instrukce = nižší kontext = přesnější zaměření agenta.

---

## Pravidla pro architekturní dokument (docs/architecture.md)

Pokud neexistuje, vytvoř jej. Dokument popisuje, jak systém vypadá TEĎ — ADR dokumenty popisují, PROČ vypadá takhle.

### Povinné sekce

```markdown
# Architektura systému

## Účel systému
[2–4 věty: co systém dělá a co neřeší]

## Architektonické vrstvy
[Tabulka: Vrstva | Projekt/složka | Role]
[Pravidla závislostí: co smí referencovat co — s vysvětlením PROČ]

## Klíčové datové struktury
[Přehled důležitých entit, jejich vztahů a sémantické invarianty
 — zejména ty, které NEJSOU viditelné ze schématu]

## Integrace s externími systémy
[Přehled: co konzumujeme, co poskytujeme, přístupová práva]

## Architektonická omezení
[Pravidla, která NESMĚJÍ být porušena — každé s odůvodněním]

## Klíčové soubory
[Mapa: logický název → cesta k souboru — kritické pro navigaci agentů]
```

### Dokumentace datového modelu — specifická pravidla

Uvádět vždy:
- **Sémantické invarianty** — omezení, která nejsou vynucena schématem (např. "toto pole je immutabilní po vytvoření", "ID sdílí jednu DB sekvenci mezi dvěma tabulkami")
- **Vypočtené sloupce** — jasně označit, že se nepočítají v aplikačním kódu
- **Relace** — ASCII diagram nebo tabulka závislostí
- **Schémata / namespace** — mapování logických celků na databázová schémata

---

## Pravidla pro modulovou dokumentaci (docs/modules/<modul>.md)

### Kdy vytvořit modulový dokument

- modul má vlastní databázové schéma nebo vrstvu entit
- modul exponuje veřejné API nebo sadu servisů
- modul je netriviálně integrován s jinými moduly
- modul obsahuje netriviální business logiku

### Šablona modulového dokumentu

```markdown
# Název modulu

## Účel
[2–4 věty: co modul dělá a PROČ existuje jako samostatný celek]

## Odpovědnosti
[Bullet list konkrétních capability — scopovaných, ne vágních]

## Datový model
[Přehled entit, polí, typů, omezení, vztahů]
[Sémantické invarianty — co schéma samo nevyjadřuje]

## Tok zpracování
[Číslovaný seznam kroků: jak data prochází modulem]

## Klíčové komponenty
[Pojmenované třídy/servisy/soubory + jednořádkový popis každého]
[Vždy uvádět cestu k souboru pro navigaci agenta]

## Rozhraní (API)
[Veřejný povrch: metody, endpointy, události — se signaturami]

## Integrační vazby
[Konzumuje od: — co, od koho, v jakém formátu]
[Poskytuje pro: — co, komu, v jakém formátu]

## Závislosti
[Knihovny, env proměnné, konfigurační klíče]

## Omezení a pravidla
[MUSÍ / NESMÍ — hard invarianty s odůvodněním]

## Příklady
[Minimální funkční ukázky kódu pro klíčové scénáře]

## Odkazovaná dokumentace
[Odkazy na docs/architecture.md, docs/decisions/, skills/]
```

### Pravidlo velikosti modulového dokumentu

Optimální rozsah: **200–600 řádků**. Pokud je dokument delší, rozděl jej:

```
docs/modules/document-management.md
docs/modules/document-storage.md
docs/modules/document-search.md
```

---

## Pravidla pro architektonická rozhodnutí (docs/decisions/)

### Kdy vytvořit ADR

| Situace | Akce |
|---|---|
| Rozhodnutí je drahé na vrácení (koordinovaná práce, data, migrace) | **ADR** |
| Rozhodnutí překračuje více komponent nebo vrstev | **ADR** |
| Rozhodnutí uzavírá jinak otevřené možnosti | **ADR** |
| Přidání nezanedbatelné závislosti (copyleft, slabě udržovaná) | **ADR** |
| Triviální změna bez architektonického dopadu | Commit message |
| Nový doménový termín | Glosář, ne ADR |

**Jednoduchý test:** "Zeptal by se budoucí vývojář (nebo agent) PROČ bylo toto rozhodnutí přijato?" — Pokud ano, napiš ADR.

### Šablona ADR (MADR formát)

```markdown
# ADR-NNN: Stručný název (problém + řešení)

- **Status:** proposed | accepted | deprecated | superseded by ADR-NNN
- **Datum:** YYYY-MM-DD
- **Rozhodující:** [role nebo jména]

## Kontext a problém

[2–4 věty. Jaké síly nebo omezení vedly k rozhodnutí?
Fakta, ne hodnocení. Čtenář za 6 měsíců musí být schopen
rekonstruovat proč.]

## Zvažované varianty

- **Varianta A** — co to bylo, proč bylo atraktivní, proč odmítnuto
- **Varianta B** — …
- **Zvolená varianta** — proč vybrána

## Rozhodnutí

"Budeme…" — aktivní forma, jedna nebo dvě věty.

## Důsledky

**Pozitivní:** …
**Negativní:** …
[ADR obsahující pouze pozitiva je neúplné.]

## Reference

[Odkazy na specifikace, starší ADR, RFC, issues]
```

### Pravidla ADR

- ADR jsou **immutabilní po přijetí** — při změně vytvoř nové ADR s odkazem na předchozí (`Supersedes: ADR-NNN`), původní se nemění
- Pojmenování: `adr-001-kratky-popis.md` (číslo + slug)
- ADR index `docs/decisions/README.md` — tabulka: číslo | název | status | datum

---

## Pravidla pro skills (.github/skills/)

### Kdy vytvořit skill

- Implementační postup se opakuje (více než 2× ve stejném projektu)
- Postup má více kroků a závisí na pořadí
- Postup je specifický pro projekt, ale ne závislý na konkrétní doméně
- Existuje checklist, který agent musí projít

### Anatomie SKILL.md souboru

Skills mají formát: YAML frontmatter + Markdown tělo. Každý skill je v samostatném podadresáři:

```
.github/skills/
└── <skill-name>/          ← název musí odpovídat name: v YAML
    ├── SKILL.md           ← povinný
    └── <doplňkové soubory> ← šablony, skripty
```

```markdown
---
name: <skill-name>          # lowercase, pomlčky, max 64 znaků, musí = název adresáře
description: >              # Popis co skill dělá A KDY ho použít (max 1024 znaků)
  Krátký popis skillu.
  Použij tento skill když...
user-invocable: true        # true = dostupný jako /slash příkaz
---

# Název skillu

## Kdy použít
[Konkrétní trigger podmínky — kdy tento skill spustit]

## Předpoklady
[Co musí platit před spuštěním]

## Postup
1. Krok první...
2. Krok druhý...
[Každý krok je imperativní — "udělej X", ne "X se dělá"]

## Omezení
[Co skill NESMÍ dělat nebo měnit]

## Checklist
- [ ] Krok 1 dokončen
- [ ] Krok 2 dokončen
- [ ] Výsledek ověřen

## Příklady
[Ukázka vstupu a výstupu]
```

### Skills vs. ostatní dokumenty

| | SKILL.md | docs/modules/*.md | copilot-instructions.md |
|---|---|---|---|
| Obsah | **Imperativní** (co dělat) | **Deskriptivní** (jak to funguje) | **Normativní** (vždy platná pravidla) |
| Načítání | Na vyžádání | Na vyžádání | Vždy |
| Doménová znalost | ❌ Nesmí obsahovat | ✅ Obsahuje | Pouze to nejdůležitější |
| Příklad | "Jak přidat nový modul" | "Jak modul X funguje" | "NIKDY nevkládej DbContext do UI" |

**Zásadní pravidlo:** Skills nesmí obsahovat doménové znalosti konkrétního systému — popisují ZPŮSOB práce, ne OBSAH systému.

---

## Pravidla pro README.md

README je vstupní rozcestník pro lidské vývojáře, nikoliv pro agenty.

Obsahuje **pouze**:
- stručný popis projektu (3–5 vět),
- technologický stack,
- základní příkazy pro spuštění a build,
- odkaz na `.github/copilot-instructions.md`,
- odkaz na `docs/`.

Neobsahuje:
- detailní architekturu (patří do `docs/architecture.md`),
- konvence kódu (patří do `docs/conventions.md`),
- implementační návody,
- ADR obsah.

---

## Optimalizační checklist pro každý MD soubor

Pro každý existující Markdown soubor proveď tuto analýzu:

1. **Určení účelu** — co je primární cíl dokumentu?
2. **Správné zařazení** — patří do `copilot-instructions`, `docs/`, `skills/`, nebo `README`?
3. **Duplicity** — je obsah zopakován jinde? Pokud ano, ponech na jednom místě a odkaž.
4. **Sloučení** — překrývá se s jiným dokumentem? Zvažte sloučení.
5. **Rozdělení** — pokrývá více nesouvisejících témat? Rozděl.
6. **Aktuálnost** — jsou informace stále platné? Oprav nebo odstraň zastaralé.
7. **Odkazy** — jsou všechny relativní odkazy funkční? Oprav nefunkční.
8. **Terminologie** — jsou termíny konzistentní s ostatní dokumentací?
9. **Pravidla s odůvodněním** — jsou k zákazům a pravidlům uvedeny důvody PROČ?
10. **Navigace pro agenty** — jsou kritické soubory odkazovány cestou, ne jen názvem?

---

## Antipattern: co dokumentaci poškozuje

Vyhýbej se těmto vzorům, které AI agenty mátou nebo snižují kvalitu generování:

| Antipattern | Problém | Řešení |
|---|---|---|
| **Pravidlo bez odůvodnění** | Agent neví, zda je pravidlo "load-bearing" nebo stylový preference | Přidej "PROČ" na stejný řádek |
| **Dvě kopie stejné informace** | Agent neví, která je kanonická; při aktualizaci se rozejdou | Jeden zdroj pravdy, ostatní odkazují |
| **ADR editovaný po přijetí** | Agent ztrácí historii vývoje rozhodnutí | Supersede novým ADR, původní zachovej |
| **Monolitický dokument** | Kontext neustále přetéká; agent nezachytí relevantní část | Rozděl na soubory po jednom tématu |
| **Nedefinované doménové termíny** | Agent vymýšlí plausibilně znějící termíny → sémantický drift | Glosář + "zastav se pokud termín není v glosáři" |
| **Pozitivní ADR bez nevýhod** | Agent nevidí trade-off; může se pokusit rozhodnutí revertovat | Vždy uvést alespoň jednu negativní důsledek |
| **Spec editovaný po implementaci** | Agent nemůže rozlišit "co bylo dohodnuté" od "co bylo odůvodněné zpětně" | Zmraž spec po implementaci; změny jdou do nové ADR |
| **"Future enhancements" v docs** | Agent může hallucindovat neimplementované funkce | Odstraň nebo přesuň do issues |
| **Instrukce pro věci, co agent neudělá** | Zbytečný šum | Odstraň |

---

## Správa a udržování dokumentace

### Pravidlo souběžné aktualizace

Změna kódu, která ovlivňuje dokumentovanou architekturu, konvenci nebo modul, **musí** aktualizovat dokumentaci ve stejném commitu/PR. Žádné "docs later".

### Signály zastaralosti

| Signál | Akce |
|---|---|
| `copilot-instructions.md` odkazuje na neexistující soubory | Okamžitá oprava |
| Pravidla v dokumentaci rozporují chování kódu | Revize ADR nebo dokumentu |
| Modulový dokument neodpovídá aktuálnímu API | Aktualizace modulu |
| Termín v kódu není definován v glosáři/architektuře | Doplnění glosáře |
| Jediný velký soubor pokrývá rozrůstající se projekt | Plánované rozdělení |

### Priorita při konfliktu

Pokud existuje konflikt mezi dokumentačními soubory, platí hierarchie:

1. **Specifikace a schémata** (YAML, JSON schema, formální spec soubory) — nejvyšší priorita
2. **ADR soubory** (`docs/decisions/`) — závazná architektonická pravidla
3. **`.github/copilot-instructions.md`** — globální konvence
4. **Path-specific instructions** — přepíše pro konkrétní scope
5. **Skills** — postup, ne pravidlo; nejnižší priorita

### Volitelná automatizace

Pro projekty s dostatečnou zralostí zvažte:
- Skript pro regeneraci části `copilot-instructions.md` z formálních specifikací
- Lint pravidlo: PR, které mění API nebo datový model, musí obsahovat změnu v docs
- `last_reviewed` frontmatter metadata v ADR souborech
- CI krok, který kontroluje, zda cesty odkazované v instrukcích existují

---

## Priorita rozhodování při nejasnostech

1. **Jednoznačnost informací** — dvě interpretace jsou horší než jedna imperfektní
2. **Snadná orientace AI agenta** — agent naviguje soubory, ne koncepty
3. **Minimalizace načítaného kontextu** — kratší soubory = přesnější focus
4. **Modulární struktura** — jedno téma = jeden soubor
5. **Konzistence terminologie** — stejný termín v kódu, testech i dokumentaci
6. **Zachování všech relevantních znalostí** — nepřijít o "PROČ" při refaktoringu

---

## Finální výstup optimalizace

Po dokončení optimalizace vytvoř souhrnný report obsahující:

### Nová struktura dokumentace
Cílový strom adresářů s popisem každého souboru.

### Provedené změny

**Nově vytvořené soubory** — seznam s odůvodněním vytvoření.

**Přesunuté soubory** — Původní umístění → nové umístění + důvod přesunu.

**Sloučené dokumenty** — Seznam sloučených souborů + výsledný soubor.

**Rozdělené dokumenty** — Původní soubor → nově vzniklé soubory.

**Aktualizované soubory** — Co bylo změněno a proč.

### Identifikované problémy

- duplicity a jejich řešení,
- zastaralé informace,
- chybějící dokumentace,
- nefunkční nebo chybějící odkazy.

### Doporučení pro další rozvoj

Identifikuj oblasti, kde by bylo přínosné:
- vytvořit nové ADR dokumenty (pro přijatá rozhodnutí bez dokumentace),
- přidat skills pro opakované postupy,
- rozdělit příliš rozsáhlé dokumenty,
- vytvořit glosář doménových termínů,
- nastavit automatizaci pro udržení dokumentace aktuální.

---

## Zdroje a reference

Tento instrukční soubor vychází z těchto zdrojů:

- GitHub Copilot official docs — repository custom instructions (June 2026)
- Anthropic: "Building Effective Agents" — ACI design principles
- `hailo-ai/hailo-apps` — context routing tables, phase-gate workflow
- `woocommerce/woocommerce-android:AGENTS.md` — architecture disambiguation, skills pattern
- `OPM/ResInsight:docs/agents/` — hub-and-spoke documentation model
- `lukas-grigis/ralphctl` — inclusion test, token budget (100–150 lines), decision-rationale pairs
- `vpciii/methodology` — ADR-as-constraint system, documentation hierarchy, agent failure modes
- `recyclarr/recyclarr:AGENTS.md` — skill trigger system, schema-as-source-of-truth
- `micmcc/spec-driven-development-starter` — automated instruction regeneration, spec-driven workflow
- `adr/madr` — canonical MADR template
- VS Code Agent Skills docs (agentskills.io standard)
