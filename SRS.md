


<!-- TOC --><a name="specifikace-softwarových-poadavk-pro-restproject"></a>
# Specifikace softwarových požadavků pro RestProject

Verze 2.2.2

Připravil: Kryštof Kubín

Datum: 23.10.2024

Kontakty: kubin.kr.2021@skola.ssps.cz

## Obsah

<!-- TOC start (generated with https://github.com/derlin/bitdowntoc) -->

- [Specifikace softwarových požadavků pro RestProject](#specifikace-softwarových-poadavk-pro-restproject)
   * [1. Úvod](#1-úvod)
      + [1.1 Účel](#11-úel)
      + [1.2 Konvence dokumentu](#12-konvence-dokumentu)
      + [1.3 Zamýšlené publikum a doporučení ke čtení](#13-zamýlené-publikum-a-doporuení-ke-tení)
      + [1.4 Rozsah produktu](#14-rozsah-produktu)
      + [1.5 Odkazy ](#15-odkazy)
   * [2. Celkový popis](#2-celkový-popis)
      + [2.1 Perspektiva produktu](#21-perspektiva-produktu)
      + [2.2 Funkce produktu](#22-funkce-produktu)
      + [2.3 Třídy a charakteristiky uživatelů](#23-tídy-a-charakteristiky-uivatel)
      + [2.4 Provozní prostředí](#24-provozní-prostedí)
      + [2.5 Omezení návrhu a implementace](#25-omezení-návrhu-a-implementace)
      + [2.6 Dokumentace pro uživatele](#26-dokumentace-pro-uivatele)
      + [2.7 Předpoklady a závislosti](#27-pedpoklady-a-závislosti)
   * [3. Požadavky na externí rozhraní](#3-poadavky-na-externí-rozhraní)
      + [3.1 Uživatelská rozhraní](#31-uivatelská-rozhraní)
      + [3.2 Hardwarová rozhraní](#32-hardwarová-rozhraní)
      + [3.3 Softwarová rozhraní](#33-softwarová-rozhraní)
      + [3.4 Komunikační rozhraní](#34-komunikaní-rozhraní)
   * [4. Systémové funkce](#4-systémové-funkce)
      + [4.1 Získávání a zobrazování obrázků](#41-získávání-a-zobrazování-obrázk)
      + [4.1.1 Popis a priorita](#411-popis-a-priorita)
      + [4.1.2 Sekvence podnětů a odpovědí](#412-sekvence-podnt-a-odpovdí)
      + [4.1.3 Funkční požadavky](#413-funkní-poadavky)
      + [4.2 Funkce překladu a vyhledávání](#42-funkce-pekladu-a-vyhledávání)
      + [4.2.1 Popis a priorita](#421-popis-a-priorita)
      + [4.2.2 Sekvence podnětů a odpovědí](#422-sekvence-podnt-a-odpovdí)
      + [4.2.3 Funkční požadavky](#423-funkní-poadavky)
      + [4.3 Sdílení obrázků](#43-sdílení-obrázk)
      + [4.3.1 Popis a priorita](#431-popis-a-priorita)
      + [4.3.2 Funkční požadavky](#432-funkní-poadavky)
   * [5. Nefunkční požadavky](#5-nefunkní-poadavky)
      + [5.1 Požadavky na výkon](#51-poadavky-na-výkon)
      + [5.2 Požadavky na bezpečnost](#52-poadavky-na-bezpenost)
      + [5.3 Požadavky na zabezpečení](#53-poadavky-na-zabezpeení)
      + [5.4 Kvalitativní atributy softwaru](#54-kvalitativní-atributy-softwaru)
   * [6. Ostatní požadavky](#6-ostatní-poadavky)

<!-- TOC end -->

<!-- TOC --><a name="1-úvod"></a>
## 1. Úvod

<!-- TOC --><a name="11-úel"></a>
### 1.1 Účel

Účelem projektu "RestProject" je vyvinout mobilní aplikaci pro Android, která získává, překládá, organizuje a zobrazuje obrázky z NASA Astronomy Picture of the Day (APOD). Rozsah zahrnuje české překlady vysvětlení obrázků a vyhledávací funkce, přičemž aplikace podporuje sdílení obrázků prostřednictvím nativních funkcí zařízení s Androidem.

<!-- TOC --><a name="12-konvence-dokumentu"></a>
### 1.2 Konvence dokumentu

Tato SRS používá následující konvence:

Funkční požadavky jsou označeny jako REQ-x.

Nefunkční požadavky jsou specifikovány odděleně a používají kvantifikovatelné termíny.

<!-- TOC --><a name="13-zamýlené-publikum-a-doporuení-ke-tení"></a>
### 1.3 Zamýšlené publikum a doporučení ke čtení

Tento dokument je určen pro:

Vývojáře: Pro usnadnění implementace.

Konečné uživatele: Pro pochopení funkčnosti aplikace.

<!-- TOC --><a name="14-rozsah-produktu"></a>
### 1.4 Rozsah produktu

Aplikace bude získávat obrázky APOD od NASA, organizovat je podle roku a měsíce, překládat vysvětlení do češtiny a umožní vyhledávání obrázků. Aplikace bude dostupná na zařízeních s Androidem a nabídne nativní možnosti sdílení.

<!-- TOC --><a name="15-odkazy"></a>
### 1.5 Odkazy 
[Dokumentace NASA APOD API.](https://github.com/nasa/apod-api)

[Dokumentace Google Translate API.](https://cloud.google.com/translate/docs/reference/rest)

[Dokumentace SQLite Database API.](https://www.sqlite.org/docs.html)

<!-- TOC --><a name="2-celkový-popis"></a>
## 2. Celkový popis

<!-- TOC --><a name="21-perspektiva-produktu"></a>
### 2.1 Perspektiva produktu

Toto je nová aplikace pro Android, zaměřená na nadšence do vesmíru i širokou veřejnost. Aplikace získává data z NASA APOD API a integruje překlady pomocí Google Translate API.

<!-- TOC --><a name="22-funkce-produktu"></a>
### 2.2 Funkce produktu

Získávání obrázků a vysvětlení APOD od NASA.

Organizace obrázků podle roku a měsíce.

Překlad vysvětlení obrázků do češtiny.

Vyhledávání obrázků podle data, názvu nebo klíčových slov.

Sdílení obrázků prostřednictvím nativních funkcí Androidu.

Ukládání přeložených vysvětlení a metadat pro offline přístup.

<!-- TOC --><a name="23-tídy-a-charakteristiky-uivatel"></a>
### 2.3 Třídy a charakteristiky uživatelů

Nadšenci do vesmíru: Hlavní cílová skupina, zajímající se o obsah související s vesmírem.

Široká veřejnost: Uživatelé s minimálními technickými znalostmi, hledající snadný přístup k informacím a médiím.

<!-- TOC --><a name="24-provozní-prostedí"></a>
### 2.4 Provozní prostředí

Mobilní platforma: operační systém Android.

<!-- TOC --><a name="25-omezení-návrhu-a-implementace"></a>
### 2.5 Omezení návrhu a implementace

Bezpečné uložení API klíčů (NASA APOD, Google Translate).

SQLite bude použit pro lokální ukládání obrázků a překladů.

Překlady budou poskytovány prostřednictvím Google Translate API.

<!-- TOC --><a name="26-dokumentace-pro-uivatele"></a>
### 2.6 Dokumentace pro uživatele

Aplikace bude zahrnovat následující dokumentaci:

Vestavěné nápovědní funkce.

<!-- TOC --><a name="27-pedpoklady-a-závislosti"></a>
### 2.7 Předpoklady a závislosti

Předpokládá se spolehlivé internetové připojení pro získávání obrázků a překlad.

Aplikace je závislá na dostupnosti NASA APOD API a Google Translate API.

<!-- TOC --><a name="3-poadavky-na-externí-rozhraní"></a>
## 3. Požadavky na externí rozhraní

<!-- TOC --><a name="31-uivatelská-rozhraní"></a>
### 3.1 Uživatelská rozhraní

Jednoduché a intuitivní rozhraní pro procházení obrázků.

Organizované karty podle roku a měsíce pro procházení obrázků.

Vyhledávací funkce integrovaná do rozhraní pro snadný přístup.

<!-- TOC --><a name="32-hardwarová-rozhraní"></a>
### 3.2 Hardwarová rozhraní

Aplikace bude interagovat s funkcemi zařízení s Androidem pro sdílení obrázků, ukládání a přístup k síti.

<!-- TOC --><a name="33-softwarová-rozhraní"></a>
### 3.3 Softwarová rozhraní

NASA APOD API pro získávání obrázků.

Google Translate API pro překlad vysvětlení obrázků.

SQLite pro lokální ukládání dat.

<!-- TOC --><a name="34-komunikaní-rozhraní"></a>
### 3.4 Komunikační rozhraní

Aplikace bude používat HTTPS pro zabezpečenou komunikaci s externími API.

<!-- TOC --><a name="4-systémové-funkce"></a>
## 4. Systémové funkce

**4.1 Získávání a zobrazování obrázků**

**4.2 Funkce překladu a vyhledávání**

**4.3 Sdílení obrázků**



<!-- TOC --><a name="41-získávání-a-zobrazování-obrázk"></a>
### 4.1 Získávání a zobrazování obrázků

<!-- TOC --><a name="411-popis-a-priorita"></a>

#### 4.1.1 Popis, priorita a urgence

Aplikace získává denní obrázky NASA APOD a organizuje je podle roku a měsíce. Tyto obrázky jsou rozděleny do "karet" které zobrazují poslední obrázek v daném roce nebo měsíci.

-   **Priorita**: Vysoká
-   **Urgence**: Kritická, implementace nezbytná pro základní funkčnost aplikace.

<!-- TOC --><a name="412-sekvence-podnt-a-odpovdí"></a>
#### 4.1.2 Sekvence podnětů a odpovědí

Podnět: Uživatel otevře aplikaci a vybere "kartu" konkrétní roku.

Odpověď: Aplikace zobrazí "karty" měsíců a poslední obrázek v každém měsíci.

Podnět: Uživatel otevře "kartu" měsíce.

Odpověd: Aplikace získá obrázky a zobrazí je spolu s přeloženými vysvětleními.

<!-- TOC --><a name="413-funkní-poadavky"></a>
#### 4.1.3 Funkční požadavky

REQ-1.0: Aplikace musí získávat obrázky z NASA APOD API.
 - **Priorita**: Vysoká, **Urgence**: Vysoká

REQ-2.0: Aplikace musí zobrazit všechny obrázky v daném měsíci.
 - **Priorita**: Vysoká, **Urgence**: Střední

REQ-2.1: Aplikace musí zobrazit poslední obrázek v daném roce nebo měsíci na "kartě". 
 - **Priorita**: Vysoká, **Urgence**: Vysoká

<!-- TOC --><a name="42-funkce-pekladu-a-vyhledávání"></a>
### 4.2 Funkce překladu a vyhledávání

<!-- TOC --><a name="421-popis-a-priorita"></a>
#### 4.2.1 Popis a priorita

Aplikace poskytuje automatické překlady vysvětlení obrázků do češtiny a umožňuje vyhledávání podle názvu, data nebo klíčových slov.

-   **Priorita**: Vysoká
-   **Urgence**: Střední, nutné pro usnadnění používání českým publikem.

<!-- TOC --><a name="422-sekvence-podnt-a-odpovdí"></a>
#### 4.2.2 Sekvence podnětů a odpovědí

Podnět: Uživatel vyhledává obrázek podle klíčového slova.

Odpověď: Aplikace zobrazí relevantní obrázky na základě vyhledávacího termínu.

<!-- TOC --><a name="423-funkní-poadavky"></a>
#### 4.2.3 Funkční požadavky

REQ-3.0: Aplikace musí poskytovat české překlady pomocí Google Translate API.
 - **Priorita**: Vysoká, **Urgence**: Nízká

REQ-3.1: Aplikace musí skladovat české překlady v databázi.
 - **Priorita**: Vysoká, **Urgence**: Střední

REQ-4.0: Aplikace musí podporovat vyhledávání obrázků podle názvu, data nebo klíčového slova.
 - **Priorita**: Střední, **Urgence**: Nízká


<!-- TOC --><a name="43-sdílení-obrázk"></a>
### 4.3 Sdílení obrázků
<!-- TOC --><a name="431-popis-a-priorita"></a>

###  4.3.1 Popis, priorita a urgence

Aplikace umožňuje uživatelům sdílet obrázky prostřednictvím nativních sdílecích funkcí Androidu.

-   **Priorita**: Střední
-   **Urgence**: Střední, vhodné pro zvýšení interakce mezi uživateli, ale není nezbytné pro první verzi aplikace.

<!-- TOC --><a name="432-funkní-poadavky"></a>
#### 4.3.2 Funkční požadavky

REQ-5.0: Aplikace musí umožňovat sdílení obrázků prostřednictvím e-mailu a sociálních sítí pomocí nativních funkcí sdílení Androidu.
 - **Priorita**: Střední, **Urgence**: Nízká

<!-- TOC --><a name="5-nefunkní-poadavky"></a>
## 5. Nefunkční požadavky

<!-- TOC --><a name="51-poadavky-na-výkon"></a>
### 5.1 Požadavky na výkon

Aplikace musí načítat obrázky do 5 sekund.

Aplikace musí být optimalizována pro paměť na zařízeních s nižším výkonem.

<!-- TOC --><a name="52-poadavky-na-bezpenost"></a>
### 5.2 Požadavky na bezpečnost

Aplikace nazachovává s informace uživatele.

<!-- TOC --><a name="53-poadavky-na-zabezpeení"></a>
### 5.3 Požadavky na zabezpečení

Aplikace musí bezpečně nakládat s API klíči NASA APOD a Google Translate.

<!-- TOC --><a name="54-kvalitativní-atributy-softwaru"></a>
### 5.4 Kvalitativní atributy softwaru

Aplikace musí být spolehlivá, s minimálními výpadky.

Aplikace musí být snadno udržovatelná, umožňující jednoduché aktualizace překladu a logiky API.

<!-- TOC --><a name="6-ostatní-poadavky"></a>
## 6. Ostatní požadavky

Lokální úložiště prostřednictvím SQLite by mělo zajistit offline přístup k obrázkům a překladům.# Specifikace softwarových požadavků pro RestProject
