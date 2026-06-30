# Instrukce pro optimalizaci Markdown dokumentace projektu pro AI agenty

## Účel

Proveď analýzu a optimalizaci veškeré Markdown dokumentace (`*.md`) v repozitáři tak, aby byla maximálně efektivní pro práci AI agentů (GitHub Copilot, Copilot Coding Agent, Cursor, Claude Code a obdobné nástroje) a zároveň zůstala přehledná pro vývojáře.

Cílem není pouze upravit obsah dokumentů, ale vytvořit jasně strukturovanou znalostní základnu, která umožní agentům rychle nalézt relevantní informace bez načítání zbytečného kontextu.

---

# Hlavní principy

## 1. Oddělení odpovědností

Každý typ dokumentace musí mít jednoznačný účel.

Používej následující rozdělení:

```text
README.md
.github/copilot-instructions.md
docs/architecture.md
docs/conventions.md
docs/modules/*.md
docs/decisions/*.md
.github/skills/*/skill.md
```

### README.md

Vstupní rozcestník projektu.

Obsahuje pouze:

- stručný popis projektu,
- technologický stack,
- základní informace pro spuštění,
- odkazy na dokumentaci,
- odkaz na `.github/copilot-instructions.md`.

Neobsahuje:

- detailní architekturu,
- detailní popis modulů,
- implementační postupy,
- rozsáhlé technické informace.

---

### .github/copilot-instructions.md

Hlavní navigační dokument pro AI agenta.

Obsahuje:

- základní pravidla práce,
- odkazy na dokumentaci,
- odkazy na skills,
- doporučený postup řešení úkolů.

Neobsahuje:

- detailní znalosti domény,
- rozsáhlou architekturu,
- detailní implementační návody.

---

### docs

Obsahuje znalosti o systému.

Veškeré informace o architektuře, modulech, procesech a rozhodnutích musí být umístěny zde.

---

### .github/skills

Obsahuje pracovní postupy.

Skills nesmí obsahovat znalosti o konkrétním systému.

Obsahují pouze:

- postupy,
- checklisty,
- pravidla práce,
- doporučené kroky,
- příklady.

---

# 2. Minimalizace kontextu

Agent musí být schopen načíst pouze informace relevantní pro aktuální úkol.

Proto:

- nerozšiřuj jeden dokument o nesouvisející témata,
- rozděluj rozsáhlé dokumenty,
- odstraňuj duplicity,
- používej odkazy mezi dokumenty.

Preferuj:

```text
docs/modules/users.md
docs/modules/documents.md
docs/modules/workflow.md
```

Před:

```text
docs/system.md
```

s několika tisíci řádků.

---

# 3. Dokumentace podle domény

Dokumentaci organizuj podle funkčních oblastí systému.

Preferované členění:

```text
docs/modules/
├── users.md
├── documents.md
├── workflow.md
├── reporting.md
├── integrations.md
└── ocr.md
```

Nepreferované členění:

```text
docs/
├── csharp.md
├── sql.md
├── backend.md
├── frontend.md
└── database.md
```

Pokud dokumentace popisuje konkrétní business oblast, musí být zařazena podle modulu, nikoliv podle technologie.

---

# 4. Dokumentace architektury

Pokud neexistuje:

```text
docs/architecture.md
```

vytvoř jej.

Dokument musí obsahovat:

## Účel systému

Stručný popis systému jako celku.

## Architektonické vrstvy

Například:

- Presentation
- Application
- Domain
- Infrastructure

## Závislosti mezi vrstvami

Popiš povolené závislosti.

## Klíčové subsystémy

Popiš hlavní části systému.

## Integrace

Popiš externí systémy a služby.

## Architektonická omezení

Popiš pravidla, která nesmí být porušována.

---

# 5. Vývojářské konvence

Pokud neexistuje:

```text
docs/conventions.md
```

vytvoř jej.

Dokument by měl obsahovat:

## Naming conventions

- pojmenování tříd,
- pojmenování metod,
- pojmenování služeb,
- pojmenování DTO,
- pojmenování databázových objektů.

## Struktura řešení

- struktura projektů,
- struktura složek,
- doporučené členění kódu.

## Kódovací standardy

- async/await,
- CancellationToken,
- nullability,
- validace,
- error handling.

## Testování

- jednotkové testy,
- integrační testy,
- naming testů.

## Architektonické principy

- CQRS,
- Repository Pattern,
- Clean Architecture,
- Dependency Injection.

---

# 6. Dokumentace modulů

Každý významný modul systému musí mít vlastní dokument.

Preferovaná struktura:

```md
# Název modulu

## Účel

## Funkcionalita

## Hlavní komponenty

## Datový model

## Tok zpracování

## Integrace

## Omezení

## Související dokumentace
```

Pokud je dokument příliš rozsáhlý, rozděl jej na více souborů.

Například:

```text
docs/modules/document-management.md
docs/modules/document-storage.md
docs/modules/document-search.md
```

namísto jednoho rozsáhlého dokumentu.

---

# 7. Architektonická rozhodnutí (ADR)

Pokud dokumentace obsahuje informace typu:

- proč bylo něco navrženo určitým způsobem,
- proč byla vybrána určitá technologie,
- jaké varianty byly zvažovány,
- jaké kompromisy byly přijaty,

přesuň tyto informace do:

```text
docs/decisions/
```

Každé významné rozhodnutí vytvoř jako samostatný dokument.

Příklad:

```text
docs/decisions/
├── adr-001-layered-architecture.md
├── adr-002-cqrs.md
└── adr-003-ocr-provider.md
```

Každý ADR dokument by měl obsahovat:

```md
# Kontext

# Rozhodnutí

# Důsledky

# Alternativy
```

---

# 8. Skills

Identifikuj opakovaně používané implementační postupy.

Pro každý významný postup vytvoř samostatný skill.

Příklady:

```text
.github/skills/
├── backend-development/
├── api-development/
├── database-migrations/
├── unit-testing/
├── integration-testing/
├── ui-development/
├── ocr-processing/
└── logging-monitoring/
```

Každý skill musí mít strukturu:

```md
# Název skillu

## Kdy použít

## Postup

## Omezení

## Checklist

## Příklady
```

Skills nesmí obsahovat doménové znalosti konkrétního systému.

Skills popisují pouze způsob práce.

---

# 9. Optimalizace obsahu

Pro každý Markdown soubor proveď:

1. Určení účelu dokumentu.
2. Zařazení do správné kategorie.
3. Odstranění duplicit.
4. Sloučení překrývajících se dokumentů.
5. Rozdělení příliš rozsáhlých dokumentů.
6. Aktualizaci zastaralého obsahu.
7. Opravu neplatných odkazů.
8. Sjednocení terminologie.
9. Sjednocení struktury nadpisů.

---

# 10. Pravidla kvality

Každý dokument by měl:

- řešit jednu konkrétní oblast,
- mít jednoznačný název,
- používat konzistentní terminologii,
- mít logickou hierarchii nadpisů,
- odkazovat na související dokumentaci,
- neobsahovat duplicitní informace.

Vyhýbej se:

- obecným názvům typu `system.md`,
- velmi dlouhým dokumentům,
- více tématům v jednom souboru,
- opakování stejných informací na více místech.

---

# 11. Aktualizace odkazů

Po všech přesunech a úpravách:

- zkontroluj všechny relativní odkazy,
- oprav rozbité odkazy,
- doplň odkazy na související dokumentaci,
- zkontroluj reference z README a copilot-instructions.

---

# 12. Finální výstup

Po dokončení optimalizace vytvoř souhrn obsahující:

## Nová struktura dokumentace

Cílový strom adresářů.

## Nově vytvořené soubory

Seznam nových dokumentů.

## Přesunuté soubory

Původní umístění → nové umístění.

## Sloučené dokumenty

Seznam sloučených souborů.

## Rozdělené dokumenty

Původní soubor → nově vzniklé soubory.

## Identifikované problémy

- duplicity,
- zastaralé informace,
- chybějící dokumentace,
- neplatné odkazy.

## Doporučení pro další rozvoj dokumentace

Navrhni další oblasti vhodné pro vznik:
- modulové dokumentace,
- ADR dokumentů,
- skills.

---

# Priorita rozhodování

Při nejasnostech používej následující priority:

1. Jednoznačnost informací.
2. Snadná orientace AI agenta.
3. Minimalizace načítaného kontextu.
4. Modulární struktura dokumentace.
5. Konzistence napříč repozitářem.
6. Zachování všech relevantních znalostí projektu.

Hlavním cílem je vytvořit dokumentaci, která umožní AI agentům efektivně analyzovat, navrhovat a implementovat změny v repozitáři s minimálním množstvím zbytečného kontextu a maximální přesností.