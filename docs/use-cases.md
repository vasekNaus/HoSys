# Scénáře použití (Use Cases)

## UC-01: Kontrola faktury za ledový čas

**Aktér:** Ekonom / člen výboru  
**Trigger:** Přijata faktura od provozovatele zimního stadionu za uplynulý měsíc

**Postup:**
1. Uživatel otevře modul Kontrola fakturace.
2. Vybere zúčtovací období (měsíc/sezóna) a kategorii.
3. Systém načte bloky z rezervačního systému (`plan.Block`) pro dané období.
4. Systém vypočítá celkový objem minut/hodin dle rezervací.
5. Uživatel zadá nebo importuje položky z faktury.
6. Systém zobrazí srovnávací tabulku: rezervace vs. faktura.
7. Pokud existují rozdíly, systém je zvýrazní a umožní export pro reklamaci.

**Výsledek:** Uživatel potvrdí, nebo zamítne fakturu s podloženým výpočtem.

---

## UC-02: Import výkazů trenérů z Excelu

**Aktér:** Administrátor / asistent  
**Trigger:** Trenér odevzdá měsíční výkaz v souboru `.xlsx`

**Postup:**
1. Administrátor spustí `SportSys.ConsoleApp` (nebo budoucí UI v SportSys.Web).
2. Vybere složku s `.xlsx` soubory (formát: `[číslo] [příjmení trenéra].xlsx`).
3. Aplikace přečte každý soubor, identifikuje trenéra z názvu souboru.
4. Záznamy jsou validovány (datum, čas, kategorie).
5. Duplicity jsou detekovány a přeskočeny / označeny k ověření.
6. Validní záznamy jsou vloženy do tabulky `Training` v databázi SportSys.

**Výsledek:** Databáze obsahuje záznamy tréninků odpovídající odevzdaným výkazům.

---

## UC-03: Přehled tréninků sezóny

**Aktér:** Člen výboru / trenér  
**Trigger:** Potřeba zkontrolovat počty tréninků v aktuální sezóně

**Postup:**
1. Uživatel přejde do sekce Evidence tréninků.
2. Vybere sezónu a volitelně kategorii nebo typ tréninku.
3. Systém zobrazí seznam tréninků s filtrovacími a řadicími možnostmi.
4. Uživatel může zobrazit detail záznamu nebo provést export.

**Výsledek:** Uživatel má přehled o odtrénovaných jednotkách včetně sportoviště a délky.

---

## UC-04: Generování plateb trenérům

**Aktér:** Ekonom  
**Trigger:** Konec měsíce / výplatní termín

**Postup:**
1. Systém načte platné smlouvy s trenéry pro aktuální období.
2. Porovná počet odtrénovaných hodin z evidence tréninků se smluvní odměnou.
3. Vygeneruje seznam platebních příkazů.
4. Uživatel zkontroluje a potvrdí platby.
5. Systém exportuje soubor pro internetové bankovnictví.

**Výsledek:** Platby jsou připraveny k odeslání bez ručního přepisování dat.

---

## UC-05: Evidence zápasů a výsledků

**Aktér:** Sekretář / člen výboru  
**Trigger:** Odehrání zápasu

**Postup:**
1. Uživatel přejde do sekce Zápasy a vybere sezónu + kategorii.
2. Doplní výsledek (skóre) a případnou poznámku k již naplánovanému zápasu.
3. Systém aktualizuje stav zápasu na „odehráno".
4. Zápas je dostupný v pohledu `SportEvent` společně s tréninky.

**Výsledek:** Výsledky jsou evidovány centrálně a dostupné pro statistické přehledy.
