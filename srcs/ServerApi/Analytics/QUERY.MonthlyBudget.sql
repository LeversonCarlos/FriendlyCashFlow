set nocount on;
declare @resourceID varchar(128) = '2f9c80d7-162a-46b9-96e7-af609c07a998';
declare @searchYear smallint = 2020;
declare @searchMonth smallint = 3;
declare @typeExpense smallint = 1;
declare @typeIncome smallint = 2;

/* MONTH DATA INTERVAL */
declare @monthInitial datetime = cast(ltrim(str(@searchYear))+'-'+ltrim(str(@searchMonth))+'-01 00:00:00' as datetime);
declare @monthFinal datetime = dateadd(day, -1, dateadd(month, 1, @monthInitial));
set @monthFinal = cast(convert(varchar(10),@monthFinal,121) +' 23:59:59' as datetime)
print 'month interval: ' + convert(varchar, @monthInitial, 121) + ' - ' + convert(varchar, @monthFinal, 121);

/* YEAR DATA INTERVAL */
declare @yearFinal datetime = dateadd(second, -1, @monthInitial);
declare @yearInitial datetime = dateadd(month, -13, @monthInitial);
print 'year interval: ' + convert(varchar, @yearInitial, 121) + ' - ' + convert(varchar, @yearFinal, 121);

/* ENTRIES DATA INTERVAL */
declare @entriesInitial datetime = @yearInitial;
declare @entriesFinal datetime = @monthFinal;
print 'entries interval: ' + convert(varchar, @entriesInitial, 121) + ' - ' + convert(varchar, @entriesFinal, 121);

/* ENTRIES DATA */
select
   Type,
   cast(ltrim(str(year(SearchDate)))+'-'+ltrim(str(month(SearchDate)))+'-01 00:00:00' as datetime) as SearchDate,
   PatternID, 
   sum(EntryValue) As Value
into #EntriesData
from v6_dataEntries
where
   RowStatus = 1
   and ResourceID = @resourceID
   and AccountID in (select AccountID from v6_dataAccounts where ResourceID=@resourceID and RowStatus=1 and Active=1)
   and SearchDate >= @entriesInitial
   and SearchDate <= @entriesFinal
   and TransferID is null
group by 
   Type, 
   year(SearchDate), month(SearchDate), 
   PatternID;

/* AVERAGE ENTRIES */
create table #EntriesTypeSummary (Type smallint, SearchDate datetime, Value float);
   insert into #EntriesTypeSummary
   select Type, SearchDate, sum(Value) as Value
   from #EntriesData
   where
      SearchDate >= @yearInitial and 
      SearchDate <= @yearFinal
   group by Type, SearchDate;
create table #EntriesTypeStdDev (Type smallint, StdDevValue float, AvgValue float);
   insert into #EntriesTypeStdDev
   select
      Type,
      coalesce(stdevp(Value),0) as StdDevValue,
      coalesce(avg(Value),0) as AvgValue 
   from #EntriesTypeSummary
   group by Type
create table #EntriesTypeAverage (Type smallint, Value float);
   insert into #EntriesTypeAverage
   select Type, avg(Value) as Value
   from
   (
      select
         Summary.Type, 
         Summary.Value
      from #EntriesTypeSummary as Summary
         inner join #EntriesTypeStdDev as StdDev on
         (
            Summary.Type = StdDev.Type and
            Summary.Value >= (StdDev.AvgValue - StdDev.StdDevValue) and 
            Summary.Value <= (StdDev.AvgValue + StdDev.StdDevValue)
          )
   ) SUB
   group by Type;


select * from #EntriesTypeAverage



/* AVERAGE INCOME */
create table #MonthlyIncome (SearchDate datetime, Value float);
   insert into #MonthlyIncome
   select SearchDate, sum(Value) as Value
   from #EntriesData
   where
      Type = @typeIncome
      and SearchDate >= @yearInitial
      and SearchDate <= @yearFinal
   group by SearchDate;
declare @IncomeStdDevValue float; declare @IncomeAvgValue float;
   select 
      @IncomeStdDevValue = coalesce(stdevp(Value),0),
      @IncomeAvgValue = coalesce(avg(Value),0) 
   from #MonthlyIncome
declare @IncomeValue float;
   select @IncomeValue=avg(Value)
   from #MonthlyIncome
   where 
      Value >= (@IncomeAvgValue - @IncomeStdDevValue) AND
      Value <= (@IncomeAvgValue + @IncomeStdDevValue);
print 'income Value: ' + ltrim(str(@IncomeValue));

/* MONTHLY EXPENSE */
create table #MonthlyExpense (PatternID bigint, SearchDate datetime, Value float);
   insert into #MonthlyExpense
   select PatternID, SearchDate, sum(Value) as Value
   from #EntriesData
   where
      Type = @typeExpense
      and SearchDate >= @yearInitial
      and SearchDate <= @yearFinal
   group by PatternID, SearchDate;
create table #MonthlyExpenseStdDev (PatternID bigint, StdDevValue float, AvgValue float);
   insert into #MonthlyExpenseStdDev
   select
      PatternID,
      coalesce(stdevp(Value),0) as StdDevValue,
      coalesce(avg(Value),0) as AvgValue
   from #MonthlyExpense
   group by PatternID;
create table #MonthlyExpenseAverage (PatternID bigint, Value float);
   insert into #MonthlyExpenseAverage
   select
      MonthlyExpense.PatternID,
      sum(MonthlyExpense.Value) as Value
   from #MonthlyExpense as MonthlyExpense
      left join #MonthlyExpenseStdDev as MonthlyExpenseStdDev on (MonthlyExpenseStdDev.PatternID=MonthlyExpense.PatternID)
   where 
      MonthlyExpense.Value >= (MonthlyExpenseStdDev.AvgValue - MonthlyExpenseStdDev.StdDevValue) AND
      MonthlyExpense.Value <= (MonthlyExpenseStdDev.AvgValue + MonthlyExpenseStdDev.StdDevValue)
   group by MonthlyExpense.PatternID;

/* RESULT */
select
   EntriesData.PatternID,
   Patterns.Text,
   EntriesData.Value, 
   Average.Value as AverageValue,
   EntriesData.Value / coalesce(Average.Value, EntriesData.Value) As ExpensePercent
from
(
   select
      PatternID,
      sum(Value) as Value
   from #EntriesData 
   where
      Type = @typeExpense
      and SearchDate >= @monthInitial
      and SearchDate <= @monthFinal
   group by PatternID
) as EntriesData
   left join v6_dataPatterns as Patterns on (Patterns.PatternID = EntriesData.PatternID)
   left join #MonthlyExpenseAverage as Average on (Average.PatternID = EntriesData.PatternID)
order by (EntriesData.Value / coalesce(Average.Value, EntriesData.Value)) desc

/* RESULT */
-- select * from #EntriesData;

/* CLEAR */
drop table #EntriesData;
drop table #EntriesTypeSummary;
drop table #EntriesTypeStdDev;
drop table #EntriesTypeAverage;
drop table #MonthlyIncome;
drop table #MonthlyExpense;
drop table #MonthlyExpenseStdDev;
drop table #MonthlyExpenseAverage;
set nocount off;
