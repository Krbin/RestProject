# Specifikace softwarových požadavků (SRS) pro RestProject

Verze: 1.0

Datum: 20. října 2024

Autor: Kryštof Kubín

Kontakty: kubin.kr.2021@skola.ssps.cz

## 1. Úvod
### 1.1 Účel

Cílem projektu je vytvořit multiplatformní aplikaci v .NET MAUI, která zobrazí obrázky z NASA Astronomy Picture of the Day (APOD) API. Obrázky budou organizovány podle roků a měsíců. Každý obrázek bude mít přeložené vysvětlení do češtiny, které bude možné vyhledávat pomocí klíčových slov, dat, a dalších parametrů. Uživatelé budou mít možnost sdílet obrázky prostřednictvím nativních sdílecích funkcí zařízení (e-mail, sociální sítě).

### 1.2 Rozsah projektu

Aplikace bude poskytovat přístup k obrázkům NASA APOD prostřednictvím jejich API a bude organizovat obrázky podle let a měsíců. Přeložená vysvětlení obrázků budou ukládána do databáze, což umožní jejich efektivní vyhledávání podle různých kritérií, včetně jména obrázku, data nebo klíčových slov ve vysvětlení. Překlad vysvětlení bude automatizován pomocí Google Translate API. Uživatelé budou mít možnost sdílet obrázky pomocí funkcí svého zařízení.

### 1.3 Cílová skupina

Projekt je určen pro širokou veřejnost, zejména pro zájemce o vesmír a astronomii, kteří chtějí objevovat a sdílet obrázky NASA APOD s českými vysvětleními. Aplikace bude přístupná na více platformách (mobilní a desktopové) a nabídne jednoduché a intuitivní uživatelské rozhraní.

### 1.4 Definice a zkratky

NASA APOD: NASA Astronomy Picture of the Day, denní snímek z vesmíru s odborným vysvětlením.

API: Application Programming Interface, rozhraní pro přístup k funkcím a datům systému.

Google Translate API: Rozhraní pro automatický překlad textů.

.NET MAUI: Multiplatformní vývojové prostředí od Microsoftu, které umožňuje vývoj aplikací pro mobilní zařízení a desktopy z jednoho kódu.

SRS: Specifikace softwarových požadavků.

## 2. Celkový popis
### 2.1 Produkt jako celek

Aplikace bude vytvořena v .NET MAUI. Získávání obrázků proběhne prostřednictvím NASA APOD API. Obrázky budou přístupné prostřednictvím intuitivního uživatelského rozhraní, rozděleného podle let a měsíců. Vysvětlení k obrázkům budou automaticky překládána do češtiny a uložena v lokální databázi, čímž se umožní rychlé vyhledávání. K dispozici bude také možnost sdílet obrázky přes nativní sdílecí rozhraní jednotlivých zařízení.

### 2.2 Uživatelské skupiny

Běžní uživatelé: Uživatelé s obecným zájmem o astronomii, kteří chtějí prohlížet, hledat a sdílet obrázky z NASA APOD.

### 2.3 Provozní prostředí

Platforma: Aplikace je určena předně pro platformu Android.

Databáze: Lokální databáze SQLite pro ukládání přeložených vysvětlení obrázků a dalších informací.

## 3. Požadavky na rozhraní
### 3.1 Uživatelské rozhraní

Hlavní obrazovka: Uživatel bude mít přístup k přehledu obrázků organizovaných podle roků a měsíců. Po výběru měsíce se zobrazí seznam obrázků s jejich náhledy a základními informacemi.

Detail obrázku: Po kliknutí na obrázek se zobrazí jeho detail s plnou verzí obrázku a přeloženým vysvětlením.

Vyhledávání: Na hlavní obrazovce bude vyhledávací pole, které umožní hledání obrázků podle jména, data nebo klíčových slov.

Sdílecí funkce: Na detailní stránce obrázku bude tlačítko pro sdílení, které umožní sdílet obrázek prostřednictvím nativních funkcí zařízení (např. e-mail, sociální sítě).

### 3.2 Softwarová rozhraní

NASA APOD API: Pro získání obrázků a jejich vysvětlení.

Google Translate API: Pro automatický překlad vysvětlení obrázků z angličtiny do češtiny.

SQLite databáze: Lokální databáze pro ukládání přeložených vysvětlení a metadat obrázků (rok, měsíc, název, klíčová slova).

## 4. Vlastnosti systému
### 4.1 Multiplatformní podpora v .NET MAUI

Aplikace bude vyvíjena v .NET MAUI, což umožní jednorázový vývoj s možností nasazení na více platformách.

Popis: Multiplatformní aplikace s jedním sdíleným kódem, což minimalizuje náklady na vývoj a údržbu.

Důležitost: Zajišťuje dosažitelnost aplikace pro široké spektrum uživatelů, bez ohledu na platformu, kterou používají.

Podpora zařízení: Aplikace bude optimalizována pro různé formáty obrazovky, včetně mobilních telefonů, tabletů a desktopů.

### 4.2 Organizace obrázků podle roků a měsíců

Obrázky získané z NASA APOD budou organizovány podle roků a měsíců, což uživatelům umožní snadné procházení historických snímků.

Popis: Obrázky budou rozděleny do kategorií podle roků a měsíců. Uživatel bude moci vybrat konkrétní rok a měsíc, aby zobrazil všechny obrázky za dané období.

Důležitost: Zajišťuje strukturovaný a snadno navigovatelný přístup k velkému množství obrázků.

Vizualizace: Každý měsíc bude reprezentován jako dlaždice s přehledem obrázků včetně náhledů a základních informací.

### 4.3 Přeložená vysvětlení obrázků pomocí Google Translate API

Každé vysvětlení k obrázku bude automaticky přeloženo do češtiny pomocí Google Translate API a uloženo v lokální databázi.

Popis: Vysvětlení k obrázkům z NASA APOD jsou původně v angličtině. Aplikace zajistí jejich automatický překlad do češtiny pomocí Google Translate API, přičemž přeložený text bude uložen v databázi a bude použit pro vyhledávání a zobrazování.

Důležitost: Překlad do češtiny umožňuje českým uživatelům lépe porozumět odborným vysvětlením.

Aktualizace: Překlad se bude aktualizovat při přidání nových obrázků.

### 4.4 Vyhledávací funkce na základě jména, data a klíčových slov

Aplikace umožní vyhledávat obrázky na základě různých parametrů, jako je jméno, datum a klíčová slova ve vysvětlení.

Popis: Uživatelé budou mít možnost zadávat do vyhledávacího pole klíčová slova, jména nebo data a rychle najít obrázky, které odpovídají zadaným kritériím.

Důležitost: Rychlé a efektivní vyhledávání umožní uživatelům snadnou navigaci mezi velkým množstvím obrázků.

### 4.5 Nativní sdílecí funkce zařízení

Aplikace poskytne možnost sdílení obrázků prostřednictvím vestavěných sdílecích funkcí zařízení.

Popis: Uživatel bude moci sdílet obrázky přes sociální sítě, e-mail nebo jiné aplikace nainstalované na jeho zařízení.

Důležitost: Umožňuje uživatelům snadno sdílet zajímavé obrázky s ostatními, což zvyšuje angažovanost a šíření obsahu.

### 4.6 Lokální databáze SQLite

Aplikace bude ukládat přeložená vysvětlení a metadata obrázků v lokální databázi SQLite pro offline přístup a rychlejší zobrazení.

Popis: Všechna data (přeložená vysvětlení, informace o obrázcích, klíčová slova) budou uložena v lokální databázi na zařízení uživatele.

Důležitost: Databáze umožní rychlé vyhledávání a prohlížení obrázků i při dočasném přerušení internetového připojení.

## 5. Nefunkční požadavky
### 5.1 Výkonnostní požadavky

Rychlost odezvy: Aplikace musí načítat obrázky a vysvětlení do 5 sekund při stabilním připojení.

Paměťové nároky: Aplikace by měla efektivně nakládat s pamětí, aby byla vhodná i pro zařízení s nižšími technickými specifikacemi.

### 5.2 Bezpečnostní požadavky

Ochrana API klíčů: Přístup k NASA APOD API a Google Translate API musí být chráněn prostřednictvím bezpečnostních klíčů, které nebudou dostupné uživatelům.
