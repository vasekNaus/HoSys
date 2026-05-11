# Přehled funkcí

## Modul: Kontrola fakturace

Hlavní motivace vzniku systému. Umožňuje porovnat skutečně využitý ledový čas (čerpáno z rezervačního systému sportoviště) s fakturovanými položkami od externího provozovatele.

- Načtení bloků z rezervačního systému (`plan.Block`)
- Mapování bloků na sezóny a kategorie SportSys
- Porovnání objemu hodin (rezervace vs. faktura)
- Zobrazení rozdílů a anomálií
- Export výsledků kontroly

## Modul: Evidence sportovních událostí

### Tréninky
- Evidenci tréninků v rámci sezóny a kategorie
- Typy tréninku (herní, fyzická příprava, …)
- Fáze tréninku (přípravná, soutěžní, …)
- Stav tréninku (plánováno, odehráno, zrušeno)
- Vazba na tréninkový plán
- Vazba na sportoviště (`IceRink`)
- Import tréninků z výkazů trenérů (Excel → SQL Server)
- Detekce duplicitních záznamů při importu

### Zápasy
- Evidence zápasů v rámci sezóny a kategorie
- Domácí / venkovní zápas
- Soupeř a jeho domovské sportoviště
- Skóre (vstřelené / obdržené góly)
- Stav zápasu (plánováno, odehráno, kontumace, …)
- Identifikátor zápasu (`MatchCode`) pro párování s externími systémy

### Unifikovaný pohled (SportEvent)
- VIEW `dbo.SportEvent` spojující tréninky i zápasy
- Možnost filtrovat přes celý kalendář bez ohledu na typ události
- Unikátní ID napříč entitami zajištěno sdílenou DB sekvencí

## Modul: Sportoviště

- Evidence zimních stadionů / sportovišť (`IceRink`)
- Název, adresa, město, volitelná GPS souřadnice (`geography`)
- Použití jako cizí klíč v trénincích, zápasech i u soupeřů

## Modul: Správa soupeřů

- Evidence soupeřících klubů (`Opponent`)
- Vazba na domovské sportoviště

## Modul: Správa trenérů a smluv

- Evidence smluv s trenéry (připravováno)
- Sledování platnosti smluv
- Podklad pro generování plateb

## Modul: Platební automatizace

- Generování platebních příkazů na základě smluv a odpracovaných hodin
- Export do formátu pro internetové bankovnictví (připravováno)

## Nástroje a integrace

| Nástroj | Popis |
|---|---|
| `SportSys.ConsoleApp` | Import výkazů trenérů z `.xlsx` souborů do DB |
| EF Core migrace | Správa schématu vlastní databáze SportSys |
| Read-only napojení na `plan.*` | Čtení dat z externího rezervačního systému bez zásahu do jeho dat |
