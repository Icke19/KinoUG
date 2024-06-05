# Projekt - Moje Kino 


## Autorzy:

- Patryk Sękielewski
- Igor Puścikowski 
- Cezary Sawicki


Moje Kino to internatywna aplikacja kina umożliwiająca przeglądanie dostępnych filmów, rezerwację biletów oraz dodawanie ich do koszyka. Kluczowe funkcje to: przeglądanie repertuaru kinowego, rezerwacja biletów na wybrane seanse oraz zarządzanie rezerwacjami w koszyku przed dokonaniem zakupu. Aplikacja Idealna dla miłośników kina, którzy cenią sobie wygodny i szybki sposób na zakup biletów.

## Wymagania funkcjonalne

| ID  | NAZWA                                                      | OPIS                                                                                      | PRIORYTET | KATEGORIA |
| --- | ---------------------------------------------------------- | ----------------------------------------------------------------------------------------- | --------- | --------- |
| 1   | Widok harmonogramów oraz seansów                           | Użytkownik ma możliwość wyświetlania tytułu oraz godziny seansu                           | 1         | f         |
| 2   | Uwierzytelnianie użytkownika i administratora              | Użytkownicy i administratorzy mają możliwość zarejestrowania się, zalogowania się oraz wylogowania się | 2         | f         |
| 3   | Zarządzanie użytkownikami              | Admin będzie mógł widzieć wszystkich użytkowników oraz usuwać ich     | 3         | f         |
| 4   | Zarządzanie Filmami                                        | Admin ma możliwość edycji, usuwania i dodawania filmów                                     | 1         | f         |
| 5   | Zarządzanie repertuarami                                   | Admin ma możliwość edycji, usuwania i dodawania repertuarów                                | 1         | f         |
| 6   | Dwie role                                                  | Aplikacja będzie obejmować role administratora i zwykłego użytkownika                      | 2         | f         |
| 7   | Rezerwacja miejsca                                         | Możliwość wyboru konkretnego miejsca w sali kinowej                                       | 1         | f         |

## Wymagania niefunkcjonalne

| ID  | NAZWA                                   | OPIS                                                            | PRIORYTET | KATEGORIA |
| --- | --------------------------------------- | --------------------------------------------------------------- | --------- | --------- |
| 1   | Cache'owanie treści na stronie           | Poprawa wydajności serwisu                                       | 3         | nf        |
| 2   | Responsywność                           | Dostosowanie widoku do różnych rozmiarów urządzeń                | 2         | nf        |
| 3  | Dark mode           | Tryb ciemny dla strony                                      | 3         | nf        |

## Scenariusze testowe
   
### Uwierzytelnianie użytkownika i administratora

#### Warunki Wstępne: brak

#### Kroki:
- Otwarcie aplikacji
- Kliknięcie przycisku "Konto" na Navbar
- Kliknięcie przycisku "Zarejestruj się"
- Uzupełnienie poprawnie pól "Imię", "Nazwisko", "Email", "Hasło", "Potwierdź hasło"
- Kliknięcie przycisku "Zarejestruj się"

#### Oczekiwany Rezultat: Użytkownik zarejestrował się pomyślnie i zostanie zapisany do bazy danych.

### Użytkownik może kupić bilet

#### Warunki Wstępne: Zalogowany na koncie Usera

#### Kroki:
- Odpalenie Aplikacji
- Zalogowanie się
- Kliknięcie przycisku "Repertuar"
- Kliknięcie "Kup Bilet" pod jednym z filmów
- Wybranie miejsc
- Kliknięcie "Zakup bilet"
- Widok koszyka, w którym mamy napisane miejsce
- Kliknięcie przycisku "Potwierdź zakup"

#### Oczekiwany Rezultat: Bilet zostaje dodany do bazy danych wraz z danymi użytkownika.

### Zarządzanie użytkownikami

#### Warunki Wstępne: Zalogowany na koncie admina

#### Kroki:
- Odpalenie aplikacji
- Kliknięcie przycisku "Konto"
- Kliknięcie przycisku "Manage Users"

#### Oczekiwany Rezultat: Admin widzi listę wszystkich użytkowników oraz ich biletów.

### Logowanie admina

#### Warunki Wstępne: Konto użytkownika w bazie danych

#### Kroki:
- Start aplikacji
- Kliknięcie "Konto" na Navbarze
- Uzupełnienie pól "Email" i "Password" poprawnymi danymi
- Kliknięcie przycisku "Zaloguj"

#### Oczekiwany Rezultat: Admin został zalogowany pomyślnie i został przerzucony na stronę główną.

### Logowanie użytkownika

#### Warunki Wstępne: Konto użytkownika w bazie danych

#### Kroki:
- Start aplikacji
- Kliknięcie "Konto" na Navbarze
- Uzupełnienie pól "Email" i "Password" poprawnymi danymi
- Kliknięcie przycisku "Zaloguj"

#### Oczekiwany Rezultat: Użytkownik został zalogowany pomyślnie i został przerzucony na stronę główną.

## Wymagania systemowe
| Kategoria                          | Wymagania                                                                                       |
|------------------------------------|-------------------------------------------------------------------------------------------------|
| System operacyjny                  | Windows, Linux, MacOS wraz z przeglądarką internetową                                           |
| .NET SDK                           | Wersja: 5.0 lub nowsza                                                                          |
| Środowisko programistyczne backend | Wersja: Visual Studio 2019 lub nowszy z wsparciem dla ASP.NET i Web Development                 |
| Środowisko programistyczne frontend| Rider                                                                                           |
| Microsoft SQL Server               | SQL Server 2017 lub nowszy                                                                      |
| Zarządzanie bazą danych            | SQL Server Management Studio (SSMS)                                                             |
| Kontrola wersji                    | Narzędzie: Git, Repozytoria: GitHub                                                             |
| Testowanie                         | Narzędzie: Postman, Narzędzie: SwaggerUI                                                        |
