# Task-1

#### Таблица *Coordinates* базы данных содержит координаты отрезка *x1*, *x2* на координатной оси.

**1.** Используя **SQL** запрос, сформировать список вида **(len; num)**, где **len** – длина отрезка, округлённая до целого числа, **num** – количество отрезков длины **len**. Список отсортировать по возрастанию поля **len**.

**2.** Сохранить получившийся список в таблицу ***Frequencies***, предварительно очистив её содержимое.

**3.** Используя **SQL** запрос, найти записи в ***Frequencies***, в которых **len** больше **num**.

#### Пример таблицы *Coordinates* в базе данных:

x1    |x2   |
-----:|----:|
0     |1    |
2,5   |0    |
-3,145|-2   |

#### Вывод:  
1;2  
3;1  

3;1

***

# Замечания 
* Пункты 1 и 2 выполнить в разных циклах. В результате выполнения пункта 1 должна быть создана коллекция. В следующем цикле эту коллекцию выгрузить в таблицу ***Frequencies***.

* Использовать СУБД **Access**. Для этого предварительно нужно создать базу данных с таблицами ***Coordinates*** и ***Frequencies***. В таблицу ***Coordinates*** нужно занести координаты нескольких отрезков.

* В программе подключение к созданной базе данных выполняется следующим образом:
```C#
	const string connectionString=@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=""C:\Projects\DB\db1.mdb""";
	static void Main(string[] args)
	{
		OleDbConnection cnn= = new OleDbConnection(connectionString);
		…
```

* В программе нужно правильно перехватывать возможные исключения.

* Все ресурсы, необходимые для установки соединения и выполнения запросов, должны быть возвращены в секции `finally` в обратном порядке.

* Задача по условию похожа на задачу из коллекций. Но по содержанию задачи разные. Обработка, группировка и сортировка значений таблицы Coordinates должны быть выполнены SQL-запросом. Поэтому компаратор, сортировка, поиск по коллекции не нужны.
  
Нужны ***четыре*** запроса: 
1. Выборка записей вместе с обработкой, группировкой, сортировкой: 
```SQL
SELECT ... AS len, Count(*) AS num FROM Coordinates GROUP BY ... ORDER BY ... 
```
2. Удаление всех записей из таблицы ***Frequencies***.
3. Добавление записи в таблицу ***Frequencies***. 
4. Выборка записей из таблицы ***Frequencies***.

* Правильность работы запросов полезно предварительно проверить в среде СУБД Access.

* Правильное округление в SQL int(аргумент + 0.5).

* Обратите внимание на пример вывода: исходный список также нужно вывести, а элементы, где len>num вывести через пустую строку.
