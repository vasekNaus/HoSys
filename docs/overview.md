# Přehled systému SportSys

## Účel

SportSys je interní informační systém sportovní organizace (hokejový klub). Slouží jako **nadstavba nad existujícími systémy**, které organizace používá, a poskytuje funkce, jež tyto systémy samostatně nenabízejí. Primárním cílem je propojit data z různých zdrojů na jednom místě a umožnit jejich kontrolu, analýzu a automatizaci rutinních úkonů.

## Kontext a motivace

Organizace využívá minimálně tyto externí systémy:

| Systém | Účel | Způsob integrace |
|---|---|---|
| Rezervační systém sportoviště (schéma `plan`) | Správa bloků ledového času a rezervací | Read-only napojení přes sdílenou databázi (EF Core modely v namespace `Emr`) |
| Účetní / fakturační systém | Správa přijatých faktur | Manuální nebo importovaný vstup |

Tyto systémy neposkytují vzájemné propojení ani kontrolní mechanismy. SportSys tuto mezeru zaplňuje.

## Uživatelé systému

Systém je určen primárně pro interní použití **výboru sportovní organizace** – zejména pro osoby zodpovědné za:
- kontrolu fakturace a nákladů
- plánování a evidenci tréninků a zápasů
- správu smluv s trenéry a generování plateb

## Klíčové schopnosti

- **Kontrola fakturace sportoviště** – porovnání skutečně využitého ledového času (z rezervačního systému) s fakturovanými položkami.
- **Evidence sportovních událostí** – tréninky a zápasy evidované v centrální databázi SportSys, oddělené od rezervačního systému.
- **Import dat** – načítání výkazů trenérů z Excelu do databáze.
- **Správa smluv a plateb** – evidence smluv s trenéry, generování platebních příkazů a export pro internetové bankovnictví.

## Rozsah projektu

SportSys **nepřebírá** funkce existujících systémů (rezervace, účetnictví). Pracuje s jejich výstupy a doplňuje je o chybějící kontrolní a analytické vrstvy.

## Související dokumenty

- [architecture.md](architecture.md) – technická architektura
- [features.md](features.md) – přehled funkcí
- [use-cases.md](use-cases.md) – scénáře použití
