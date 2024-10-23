
# Specifikace softwarových požadavků pro RestProject

Verze 2.1

Připravil: Kryštof Kubín

Datum: 23.10.2024

Kontakty: kubin.kr.2021@skola.ssps.cz

## Obsah

#### 1. Úvod
 -  1.1 Účel 
 - 1.2 Konvence dokumentu 
 - 1.3 Zamýšlené publikum a doporučení ke čtení
 - 1.4 Rozsah produktu
 - 1.5 Odkazy

#### 2. Celkový popis
 - 2.1 Perspektiva produktu
 - 2.2 Funkce produktu
 - 2.3 Třídy a charakteristiky uživatelů
 - 2.4 Provozní prostředí
 - 2.5 Omezení návrhu a implementace
 - 2.6 Dokumentace pro uživatele
 - 2.7 Předpoklady a závislosti

#### 3. Požadavky na externí rozhraní
 - 3.1 Uživatelská rozhraní
 - 3.2 Hardwarová rozhraní
 - 3.3 Softwarová rozhraní
 - 3.4 Komunikační rozhraní

#### 4. Systémové funkce
 - 4.1 Získávání a zobrazování obrázků
 - 4.2 Funkce překladu a vyhledávání
 - 4.3 Sdílení obrázků

#### 5. Nefunkční požadavky
 - 5.1 Požadavky na výkon
 - 5.2 Požadavky na bezpečnost
 - 5.3 Požadavky na zabezpečení
 - 5.4 Kvalitativní atributy softwaru

#### 6. Ostatní požadavky
&nbsp;
&nbsp;
## 1. Úvod

### 1.1 Účel

Účelem projektu "RestProject" je vyvinout mobilní aplikaci pro Android, která získává, překládá, organizuje a zobrazuje obrázky z NASA Astronomy Picture of the Day (APOD). Rozsah zahrnuje české překlady vysvětlení obrázků a vyhledávací funkce, přičemž aplikace podporuje sdílení obrázků prostřednictvím nativních funkcí zařízení s Androidem.

### 1.2 Konvence dokumentu

Tato SRS používá následující konvence:

Funkční požadavky jsou označeny jako REQ-x.

Nefunkční požadavky jsou specifikovány odděleně a používají kvantifikovatelné termíny.

### 1.3 Zamýšlené publikum a doporučení ke čtení

Tento dokument je určen pro:

Vývojáře: Pro usnadnění implementace.

Konečné uživatele: Pro pochopení funkčnosti aplikace.

### 1.4 Rozsah produktu

Aplikace bude získávat obrázky APOD od NASA, organizovat je podle roku a měsíce, překládat vysvětlení do češtiny a umožní vyhledávání obrázků. Aplikace bude dostupná na zařízeních s Androidem a nabídne nativní možnosti sdílení.

### 1.5 Odkazy 
Dokumentace NASA APOD API.

Dokumentace Google Translate API.

Dokumentace SQLite Database API.

## 2. Celkový popis

### 2.1 Perspektiva produktu

Toto je nová aplikace pro Android, zaměřená na nadšence do vesmíru i širokou veřejnost. Aplikace získává data z NASA APOD API a integruje překlady pomocí Google Translate API.

### 2.2 Funkce produktu

Získávání obrázků a vysvětlení APOD od NASA.

Organizace obrázků podle roku a měsíce.

Překlad vysvětlení obrázků do češtiny.

Vyhledávání obrázků podle data, názvu nebo klíčových slov.

Sdílení obrázků prostřednictvím nativních funkcí Androidu.

Ukládání přeložených vysvětlení a metadat pro offline přístup.

### 2.3 Třídy a charakteristiky uživatelů

Nadšenci do vesmíru: Hlavní cílová skupina, zajímající se o obsah související s vesmírem.

Široká veřejnost: Uživatelé s minimálními technickými znalostmi, hledající snadný přístup k informacím a médiím.

### 2.4 Provozní prostředí

Mobilní platforma: operační systém Android.

### 2.5 Omezení návrhu a implementace

Bezpečné uložení API klíčů (NASA APOD, Google Translate).

SQLite bude použit pro lokální ukládání obrázků a překladů.

Překlady budou poskytovány prostřednictvím Google Translate API.

### 2.6 Dokumentace pro uživatele

Aplikace bude zahrnovat následující dokumentaci:

Vestavěné nápovědní funkce.

### 2.7 Předpoklady a závislosti

Předpokládá se spolehlivé internetové připojení pro získávání obrázků a překlad.

Aplikace je závislá na dostupnosti NASA APOD API a Google Translate API.

## 3. Požadavky na externí rozhraní

### 3.1 Uživatelská rozhraní

Jednoduché a intuitivní rozhraní pro procházení obrázků.

Organizované karty podle roku a měsíce pro procházení obrázků.

Vyhledávací funkce integrovaná do rozhraní pro snadný přístup.

### 3.2 Hardwarová rozhraní

Aplikace bude interagovat s funkcemi zařízení s Androidem pro sdílení obrázků, ukládání a přístup k síti.

### 3.3 Softwarová rozhraní

NASA APOD API pro získávání obrázků.

Google Translate API pro překlad vysvětlení obrázků.

SQLite pro lokální ukládání dat.

### 3.4 Komunikační rozhraní

Aplikace bude používat HTTPS pro zabezpečenou komunikaci s externími API.

## 4. Systémové funkce

### 4.1 Získávání a zobrazování obrázků

### 4.1.1 Popis a priorita

Aplikace získává denní obrázky NASA APOD a organizuje je podle roku a měsíce (Priorita: Vysoká).

### 4.1.2 Sekvence podnětů a odpovědí

Podnět: Uživatel otevře aplikaci a vybere konkrétní rok a měsíc.

Odpověď: Aplikace získá obrázky a zobrazí je spolu s přeloženými vysvětleními.

### 4.1.3 Funkční požadavky

REQ-1: Aplikace musí získávat obrázky z NASA APOD API.

REQ-2: Aplikace musí organizovat obrázky podle roku a měsíce.

### 4.2 Funkce překladu a vyhledávání

### 4.2.1 Popis a priorita

Aplikace poskytuje automatické překlady vysvětlení obrázků do češtiny a umožňuje vyhledávání podle názvu, data nebo klíčových slov (Priorita: Vysoká).

### 4.2.2 Sekvence podnětů a odpovědí

Podnět: Uživatel vyhledává obrázek podle klíčového slova.

Odpověď: Aplikace zobrazí relevantní obrázky na základě vyhledávacího termínu.

### 4.2.3 Funkční požadavky

REQ-3: Aplikace musí poskytovat české překlady pomocí Google Translate API.

REQ-4: Aplikace musí podporovat vyhledávání obrázků podle názvu, data nebo klíčového slova.

### 4.3 Sdílení obrázků

### 4.3.1 Popis a priorita

Aplikace umožňuje uživatelům sdílet obrázky prostřednictvím nativních sdílecích funkcí Androidu (Priorita: Střední).

### 4.3.2 Funkční požadavky

REQ-5: Aplikace musí umožňovat sdílení obrázků prostřednictvím e-mailu a sociálních sítí pomocí nativních funkcí sdílení Androidu.

## 5. Nefunkční požadavky

### 5.1 Požadavky na výkon

Aplikace musí načítat obrázky do 5 sekund.

Aplikace musí být optimalizována pro paměť na zařízeních s nižším výkonem.

### 5.2 Požadavky na bezpečnost

Nepoužitelné.

### 5.3 Požadavky na zabezpečení

Aplikace musí bezpečně nakládat s API klíči NASA APOD a Google Translate.

### 5.4 Kvalitativní atributy softwaru

Aplikace musí být spolehlivá, s minimálními výpadky.

Aplikace musí být snadno udržovatelná, umožňující jednoduché aktualizace překladu a logiky API.

## 6. Ostatní požadavky

Lokální úložiště prostřednictvím SQLite by mělo zajistit offline přístup k obrázkům a překladům.# Specifikace softwarových požadavků pro RestProject
