# SqlManager
Konsolowa aplikacja umożliwiająca zarządzania bazami danych na serwerze MS SQL.

## Korzystanie z programu

### Connection string
Aby rozpocząć korzystanie z programu należy edytować zmienną ***connectionString*** w pliku ***Program.cs***.<br/> <br/>
Jak ma wyglądać zmienna ***connectionString***:
- ***Data Source = (serwer); Integrated Security = true;*** - jeżeli korzystamy z Integrated Security
- ***Data Source = (serwer); Integrated Security = false; User ID = (ID); Password = (hasło);*** - jeżeli nie korzystamy z Integrated Security
<h3>Ważne jest to aby connectionString kończył się średnikiem! Nie należy również podawać wartości Inital Catalog!</h3>

### Nawigacja po programie
Jeśli poprawnie podano connectionString, po uruchomieniu programu pojawi się lista baz danych zapisanych na serwerze oraz opcja:
- ***--Add new database--*** - pozwalająca utworzyć na serwerze nową bazę danych.

Poruszać się po menu można za pomocą ***strzałki w górę i strzałki w dół***, aktualnie wybrania opcja zaznaczona jest kolorem niebieskim.
Wciśnięcie klawisza ***Escape*** spowoduje zakończenie programu. Klawisz ***Enter*** natomiast zatwierdza aktualnie wybraną opcję.<br/>
Zatwierdzenie bazy danych wyświetli menu zawierające wszystkie tabele znajdujące się w wybranej bazie danych, oraz opcje zarządzania bazą danych:
- ***--Add new Table--*** - pozwalająca dodać nową tabelę w bazie danych.
- ***--Delete database--*** - pozwalająca usunąć aktualnie wybraną bazę danych.
- ***--Return--*** - pozwalającą powrócic do poprzedniego menu

Poruszać się po tym menu można w ten sam sposób jak po poprzednim. Po zatwierdzeniu tabeli wyświetlone zostaną opcje zarządzania tabelą:
- ***--Print table--*** - pozwalająca wyświetlić wybraną tabelę.
- ***--Edit table--*** - pozwalająca edytować konkretną komórkę w tabeli.
- ***--Add new row--*** - pozwalająca dodać nowy wiersz do tabeli.
- ***--Delete table--*** - pozwalająca usunąć aktualnie wybraną tabelę.
- ***--Return--*** - pozwalająca powrócić do poprzedniego menu.

Po wybraniu dowolnej opcji należy postępować zgodnie z instrukcjami podanymi przez program.

## Program
Program tworzy obiekty ***databaseDialog***, ***tableDialog***, oraz ***tableOptions***, zmienną ***connectionString***, wyłącza widoczność kursora, oraz wywołuje funkcję ***databaseDialog.Start()***;

## ClassLibrary

Jest to biblioteka klas, zawierająca 11 klas, w tym 7 klas statycznych i jedną klasę abstrakcyjną

### Dialog

Jest to klasa abstrakcyjna, która odpowiada za stworzenie listy opcji w danym menu, a także wyświetlanie, oraz kontrolę takiego menu.<br/>
Zawiera ona następujące metody:
- ***Add()*** - publiczna metoda typu void pozwalająca dodać nową opcję do listy
- ***Control()*** - publiczna funkcja typu int, odpowiadająca za poruszanie się po menu, oraz zatwierdzanie opcji, zwraca ona która opcja została wybrana.
- ***Draw()*** - funkcja typu void, odpowiadająca za wypisanie menu.

### DatabaseDialog

Jest to klasa dziedzicząca z klasy abstrakcyjnej ***Dialog***. Odpowiada ona za menu wyświetlające listę baz danych, oraz pozwalające dodać nową bazę danych.<br/>
Zawiera ona następujące metody:
- ***UpdateList()*** - prywatna metoda typu void, pozwalająca na zaktualizowanie listy opcji, w oparciu o informacje pobrane z bazy danych.
- ***Start()*** - publiczna metoda pozwalająca rozpocząć wyświetlanie oraz kontrolowanie menu.

### TableDialog

Jest to klasa dziedzicząca z klasy abstrakcyjnej ***Dialog***. Odpowiada ona za menu wyświetlające listę tabel w bazie danych, oraz opcje kontrolowania wybranej bazy danych.<br/>
Zawiera ona następujące metody:
- ***UpdateList()*** - prywatna metoda typu void, pozwalająca na zaktualizowanie listy opcji, w oparciu o informacje pobrane z bazy danych.
- ***Start()*** - publiczna metoda pozwalająca rozpocząć wyświetlanie oraz kontrolowanie menu.

### TableOptions

Jest to klasa dziedzicząca z klasy abstrakcyjnej ***Dialog***, odpowiada ona za menu wyświetlające opcje zarządzania tabelą.<br/>
Zawiera ona metodę:
- ***Start*** - publiczna metoda pozwalająca rozpocząć wyświetlanie oraz kontrolowanie menu.
