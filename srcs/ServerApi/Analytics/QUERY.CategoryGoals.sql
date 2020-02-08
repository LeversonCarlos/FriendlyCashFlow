set nocount on;
declare @resourceID varchar(128) = '2f9c80d7-162a-46b9-96e7-af609c07a998';
declare @searchYear smallint = 2020;
declare @searchMonth smallint = 2;
declare @typeExpense smallint = 1;
declare @typeIncome smallint = 2;

/* MONTH DATA INTERVAL */
declare @initialDate datetime = cast(ltrim(str(@searchYear))+'-'+ltrim(str(@searchMonth))+'-01 00:00:00' as datetime);
declare @finalDate datetime = dateadd(day, -1, dateadd(month,1,@initialDate));
set @finalDate = cast(convert(varchar(10),@finalDate,121) +' 23:59:59' as datetime)
print 'month interval: ' + convert(varchar, @initialDate, 121) + ' - ' + convert(varchar, @finalDate, 121);

/* MONTH DATA */
select CategoryID, sum(EntryValue) As Value
into #MonthData
from v6_dataEntries 
where 
   RowStatus = 1
   and ResourceID = @resourceID
   and AccountID in (select AccountID from v6_dataAccounts where ResourceID=@resourceID and RowStatus=1 and Active=1)
   and SearchDate >= @initialDate
   and SearchDate <= @finalDate
   and Type = @typeExpense
   and TransferID is null
group by CategoryID;

/* AVERAGE INTERVAL */
set @finalDate = dateadd(second, -1, @initialDate);
set @initialDate = dateadd(month, -13, @initialDate);
print 'average interval: ' + convert(varchar, @initialDate, 121) + ' - ' + convert(varchar, @finalDate, 121);

/* YEAR DATA */
select CategoryID, EntryValue As Value
into #YearData
from v6_dataEntries 
where 
   RowStatus = 1
   and ResourceID = @resourceID
   and AccountID in (select AccountID from v6_dataAccounts where ResourceID=@resourceID and RowStatus=1 and Active=1)
   and SearchDate >= @initialDate
   and SearchDate <= @finalDate
   and Type = @typeExpense
   and TransferID is null 
   and CategoryID in (select CategoryID from #MonthData);

/* STANDARD DEVIATION */
select 
   CategoryID, 
   coalesce(STDEVP(Value),0) as StdDevValue , 
   coalesce(AVG(Value),0) as AverageValue 
into #YearStdDev
from #YearData
group by CategoryID;

/* AVERAGE DATA */
/*
select 
   YearData.CategoryID, 0 as AverageValue, avg(Value) as PureAverageValue
from #YearData as YearData 
group by YearData.CategoryID
union
*/
select 
   YearData.CategoryID, avg(Value) as AverageValue
into #AverageData
from #YearData as YearData 
   inner join #YearStdDev as YearStdDev on (YearStdDev.CategoryID = YearData.CategoryID) 
where 
   Value > AverageValue - StdDevValue AND
   Value < AverageValue + StdDevValue
group by YearData.CategoryID;

/* APPLY AVERAGE */
alter table #MonthData add AverageValue decimal;
update #MonthData
set AverageValue =
(
   select top 1 AverageValue
   from #AverageData as AverageData
   where AverageData.CategoryID = MonthData.CategoryID
)
from #MonthData as MonthData

/* RESULT */
select * from #MonthData;
-- select * from #YearStdDev

/* CLEAR */
drop table #MonthData;
drop table #YearData
drop table #YearStdDev
drop table #AverageData
set nocount off;
