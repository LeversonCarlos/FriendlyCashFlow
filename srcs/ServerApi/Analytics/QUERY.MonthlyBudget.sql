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

/* TYPE AVERAGE */
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
            round(Summary.Value,5) >= round((StdDev.AvgValue - StdDev.StdDevValue),5) and 
            round(Summary.Value,5) <= round((StdDev.AvgValue + StdDev.StdDevValue),5)
          )
   ) SUB
   group by Type;

/* PATTERN AVERAGE */
create table #PatternSummary (PatternID bigint, SearchDate datetime, Value float);
   insert into #PatternSummary
   select PatternID, SearchDate, sum(Value) as Value
   from #EntriesData
   where
      Type = @typeExpense
      and SearchDate >= @yearInitial
      and SearchDate <= @yearFinal
   group by PatternID, SearchDate;
create table #PatternStdDev (PatternID bigint, StdDevValue float, AvgValue float);
   insert into #PatternStdDev
   select
      PatternID,
      coalesce(stdevp(Value),0) as StdDevValue,
      coalesce(avg(Value),0) as AvgValue
   from #PatternSummary
   group by PatternID;
create table #PatternAverage (PatternID bigint, Value float);
   insert into #PatternAverage
   select PatternID, avg(Value) as Value
   from
   (
      select
         Summary.PatternID, 
         Summary.Value
      from #PatternSummary as Summary
         inner join #PatternStdDev as StdDev on
         (
            Summary.PatternID = StdDev.PatternID and
            round(Summary.Value,5) >= round((StdDev.AvgValue - StdDev.StdDevValue),5) and 
            round(Summary.Value,5) <= round((StdDev.AvgValue + StdDev.StdDevValue),5)
          )
   ) SUB
   group by PatternID;

/* PATTERN DATA */
create table #PatternData (PatternID bigint, Value float, PatternAverage float, PaternOverflow float, ExpenseAverage float)
   insert into #PatternData 
   select
      EntriesData.PatternID,
      EntriesData.Value, 
      Average.Value as PatternAverage,
      coalesce(EntriesData.Value,0) - coalesce(Average.Value,0) as PaternOverflow,
      (select top 1 Value from #EntriesTypeAverage where Type=@typeExpense) as ExpenseAverage
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
   ) EntriesData
      left join #PatternAverage as Average on (Average.PatternID = EntriesData.PatternID);

/* PATTERN PERCENTAGE */
alter table #PatternData add OverflowPercent float;
   update #PatternData
   set OverflowPercent = (PaternOverflow / ExpenseAverage * 100);
declare @OverflowPercentStdDevValue float; declare @OverflowPercentAvgValue float
   select 
      @OverflowPercentStdDevValue = stdevp(OverflowPercent), 
      @OverflowPercentAvgValue = avg(OverflowPercent)
   from #PatternData
print 'Overflow StdDev Value: ' + ltrim(str(@OverflowPercentStdDevValue));
print 'Overflow Avg Value: ' + ltrim(str(@OverflowPercentAvgValue));

/* RESULT */
select
   PatternData.PatternID,
   Patterns.Text,
   Patterns.CategoryID, 
   --Value, PatternAverage,
   round(PaternOverflow,2) As OverflowValue, 
   OverflowPercent
from #PatternData as PatternData
   left join v6_dataPatterns as Patterns on (Patterns.PatternID = PatternData.PatternID)
where
   round(OverflowPercent,5) >= round((@OverflowPercentAvgValue + @OverflowPercentStdDevValue),5) or 
   round(OverflowPercent,5) <= round((@OverflowPercentAvgValue - @OverflowPercentStdDevValue),5) 
order by OverflowPercent desc

/* CLEAR */
drop table #EntriesData;
drop table #EntriesTypeSummary;
drop table #EntriesTypeStdDev;
drop table #EntriesTypeAverage;
drop table #PatternSummary;
drop table #PatternStdDev;
drop table #PatternAverage;
drop table #PatternData;
set nocount off;
