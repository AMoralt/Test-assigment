# Разработка на SQL

## Описание задачи
Задание посвящено практической работе с языком SQL. Необходимо выбрать один из предложенных вариантов и выполнить соответствующие подзадания.

**Выбранный вариант:** Вариант 2

## Проверяемые навыки
- Написание SQL-запросов
- Логическое мышление и способность составлять алгоритмы решения

---

## Вариант 2

### Задание 1
```sql
SELECT 
    s.Surname,
    s.Name,
    SUM(sa.Quantity) AS SalesVolume
FROM Sales AS sa
INNER JOIN Sellers AS s ON sa.IDSel = s.ID
WHERE sa.Date BETWEEN '2013-10-01' AND '2013-10-07'
GROUP BY s.Surname, s.Name
ORDER BY s.Surname, s.Name;```

### Задание 2
```;WITH SalesByEmployee AS (
    -- Агрегация продаж по продукту и сотруднику за период 01.10.2013 - 07.10.2013
    SELECT 
        IDProd, 
        IDSel, 
        SUM(Quantity) AS EmpSales
    FROM Sales
    WHERE Date BETWEEN '2013-10-01' AND '2013-10-07'
    GROUP BY IDProd, IDSel
),
TotalSalesByProduct AS (
    -- Общий объем продаж по продукту за период 01.10.2013 - 07.10.2013
    SELECT 
        IDProd, 
        SUM(Quantity) AS TotalSales
    FROM Sales
    WHERE Date BETWEEN '2013-10-01' AND '2013-10-07'
    GROUP BY IDProd
),
ArrivalsByProduct AS (
    -- Список уникальных IDProd, для которых были поступления в период 07.09.2013 - 07.10.2013
    SELECT DISTINCT IDProd
    FROM Arrivals
    WHERE Date BETWEEN '2013-09-07' AND '2013-10-07'
)
SELECT 
    p.Name AS ProductName,
    s.Surname,
    s.Name,
    CAST(sbe.EmpSales AS DECIMAL(18,4)) / tsp.TotalSales * 100 AS SalesPercentage
FROM SalesByEmployee sbe
INNER JOIN TotalSalesByProduct tsp ON sbe.IDProd = tsp.IDProd
INNER JOIN Products p ON sbe.IDProd = p.ID
INNER JOIN Sellers s ON sbe.IDSel = s.ID
INNER JOIN ArrivalsByProduct abp ON p.ID = abp.IDProd
ORDER BY p.Name, s.Surname, s.Name;```

