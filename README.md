# Kalkulator Ratalny MAUI

Aplikacja mobilna stworzona w technologii **.NET MAUI**, pozwalająca na szybkie i dokładne obliczanie rat kredytowych oraz generowanie szczegółowego harmonogramu spłat z możliwością eksportu do pliku PDF (Windows only).

---

## Spis treści

- [Opis projektu](#opis-projektu)  
- [Funkcjonalności](#funkcjonalności)  
- [Technologie i biblioteki](#technologie-i-biblioteki)  
- [Architektura i struktura kodu](#architektura-i-struktura-kodu)  
- [Instrukcja instalacji i uruchomienia](#instrukcja-instalacji-i-uruchomienia)  
- [Przykład użycia](#przykład-użycia)  
- [Eksport harmonogramu do PDF](#eksport-harmonogramu-do-pdf)  
- [Rozwój i dalsze kroki](#rozwój-i-dalsze-kroki)  
- [Licencja](#licencja)  

---

## Opis projektu

**Kalkulator Ratalny MAUI** to narzędzie dedykowane osobom chcącym precyzyjnie obliczyć harmonogram spłat kredytu. Aplikacja umożliwia:

- Obliczenie raty kredytu z podziałem na część kapitałową i odsetkową.
- Wizualizację harmonogramu spłat w czytelnej tabeli.
- Eksport harmonogramu do pliku PDF (funkcjonalność dostępna dla użytkowników systemu Windows).
- Intuicyjną nawigację między ekranem ustawień kredytu a widokiem harmonogramu.

---

## Funkcjonalności

- **Obliczanie rat:** dynamiczne obliczanie wysokości rat na podstawie wprowadzonych parametrów (kwota, oprocentowanie, okres spłaty).
- **Harmonogram spłat:** szczegółowa lista rat, gdzie każda pozycja zawiera:  
  - numer raty,  
  - całkowitą kwotę raty,  
  - część kapitałową,  
  - część odsetkową,  
  - pozostałe saldo do spłaty.  
- **Eksport PDF:** wygenerowanie estetycznego raportu PDF z harmonogramem, gotowego do wydruku lub wysłania mailem.
- **Przyjazny UI:** prosty i czytelny interfejs oparty na XAML z kontrolkami MAUI.

---

## Technologie i biblioteki

- **.NET MAUI** — framework Microsoft do budowy aplikacji mobilnych i desktopowych.
- **XAML** — język deklaratywny do tworzenia UI.
- **PdfSharpCore** — biblioteka umożliwiająca tworzenie i edycję plików PDF w .NET.
- **C#** — język programowania wykorzystywany do logiki aplikacji.

---

## Architektura i struktura kodu

- **Model danych:**  
  `HarmonogramItem` — klasa reprezentująca pojedynczą ratę kredytu z właściwościami:  
  - `NumerRaty` (int)  
  - `KwotaRaty` (decimal)  
  - `Kapital` (decimal)  
  - `Odsetki` (decimal)  
  - `PozostaleSaldo` (decimal)

- **Widok:**  
  `HarmonogramPage.xaml` — zawiera układ tabeli i definicję widoku listy z nagłówkami i wierszami.

- **Logika widoku:**  
  `HarmonogramPage.xaml.cs` — obsługuje interakcje użytkownika, np. powrót do poprzedniego ekranu, eksport do PDF oraz inicjalizację danych.

---

## Instrukcja instalacji i uruchomienia

### Wymagania

- Visual Studio 2022+ z zainstalowanym wsparciem .NET MAUI  
- .NET SDK 7.0 lub nowszy  
- System Windows (zalecany do generowania PDF)

### Kroki

1. Sklonuj repozytorium:
   ```bash
   git clone https://github.com/twoje-repo/kalkulator-ratalny-maui.git
   cd kalkulator-ratalny-maui
2. Otwórz projekt w Visual Studio.
3. Wybierz platformę docelową (Android, iOS, Windows) z górnego paska.
4. Uruchom projekt naciskając F5 lub klikając Run.

---

## Przykład użycia

Po uruchomieniu aplikacji:

1. Wprowadź dane kredytu: kwotę, oprocentowanie i czas trwania.
2. Naciśnij **"Oblicz raty"**.
3. Przejdź do zakładki z harmonogramem spłat.
4. Sprawdź szczegóły każdej raty w tabeli.
5. Jeśli jesteś na Windows, kliknij **„Eksportuj do PDF”**, aby zapisać raport w folderze **Dokumenty**.

---

## Eksport harmonogramu do PDF

Funkcjonalność generowania PDF korzysta z biblioteki **PdfSharpCore** i działa aktualnie tylko na systemie **Windows**.

PDF zawiera:

- Tytuł i datę wygenerowania  
- Tabelę z kolumnami: numer raty, kwota, kapitał, odsetki, saldo  
- Automatyczne dzielenie na strony  
- Estetyczne nagłówki i formatowanie walut

**Ścieżka zapisu:** folder `Dokumenty` użytkownika, plik nazwany:  
`Harmonogram_yyyyMMdd_HHmmss.pdf`

---

## Rozwój i dalsze kroki

- Eksport PDF na Android i iOS  
- Historia kalkulacji i lokalna baza danych  
- Dodanie wykresów spłaty kapitału i odsetek  

---

## Autorzy projektu

- Jakub Golonka
- Jakub Matras
- Krystian Frączek
- Łukasz Szewczyk
