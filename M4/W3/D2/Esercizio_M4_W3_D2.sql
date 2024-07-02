--1
select * from Products;

--2
select * from Products where UnitsInStock >= 40;

--3
select * from Employees where City = 'London';

--4
select * from Orders order by Freight desc;

--5
select * from Orders where Freight > 90 and Freight < 120;
 
--6
select * from Products where CategoryID = 1;

--7
select * from [Order Details] where Discount <> 0;

--8
select * from Orders where CustomerID = 'BOTTM' and Freight > 50;