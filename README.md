
# Приложение WebApi с 3 методами. 

## Первый метод 
Принимает файл вида .csv, в котором на каждой новой строке значение вида:<br>
{Дата и время в формате ГГГГ-ММ-ДД_чч-мм-сс};{Целочисленное значение времени в секундах};{Показатель в виде числа с плавающей запятой}<br>
Пример:
2022-03-18_09-18-17;1744;1632,472

Если файл с таким именем уже существует, перезаписывать значения в базе.
Этот файл парсится, значения записываются в базу в таблицу Values. 
Валидация:
- Дата не может быть позже текущей и раньше 01.01.2000
- Время не может быть меньше 0
- Значение показателя не может быть меньше 0
- Количество строк не может быть меньше 1 и больше 10 000
Из значений подсчитываются следующие значения и записываются в таблицу Results:
- Все время (максимальное значение времени минус минимальное значение времени)
- Минимальное дата и время, как момент запуска первой операции
- Среднее время выполнения
- Среднее значение по показателям
- Медина по показателям
- Максимальное значение показателя
- Минимальное значение показателя
- Количество строк

## Второй метод 
Возвращает Результаты в формате JSON. 
Могут применяться фильтры:
- По имени файла
- По времени запуска первой операции (от, до)
- По среднему показателю (в диапазоне)
- По среднему времени (в диапазоне)

## Третий метод
Получить значения из таблицы Values по имени файла

## Стек
- .NET 6
- WebApi
- Swagger
- СУБД MS SQL
- Microsoft.EntityFrameworkCore
- Microsoft.EntityFrameworkCore.SqlServer
- Deedle 
- AutoMapper.Extensions.Microsoft.DependencyInjection


