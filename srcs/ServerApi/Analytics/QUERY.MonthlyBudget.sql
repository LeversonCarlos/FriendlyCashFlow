set nocount on;
declare @resourceID varchar(128) = @paramResourceID;
declare @searchYear smallint = @paramSearchYear;
declare @searchMonth smallint = @paramSearchMonth;
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
   and not CategoryID is null
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
      from
      (
         select
            PatternID,
            (case
             when SearchDate >= @monthInitial and SearchDate <= @monthFinal
             then Value
             else 0
             end) as Value
         from #EntriesData
         where
            Type = @typeExpense and
            SearchDate >= dateadd(m, -1, @monthInitial)
      ) SUB
      group by PatternID
   ) EntriesData
      left join #PatternAverage as Average on (Average.PatternID = EntriesData.PatternID);

/* PATTERN DETAILS */
alter table #PatternData add Text varchar(500), CategoryID bigint;
   update #PatternData
   set
      Text = Patterns.Text,
      CategoryID = Patterns.CategoryID
   from #PatternData as PatternData
      left join v6_dataPatterns as Patterns on (Patterns.PatternID = PatternData.PatternID);

/* PATTERN PERCENTAGE */
alter table #PatternData add OverflowPercent float;
   update #PatternData
   set OverflowPercent = (PaternOverflow / ExpenseAverage * 100);

/* REMOVE ENTRIES LOWER THAN STANDARD DEVIATION */
declare @OverflowPercentStdDevValue float; declare @OverflowPercentAvgValue float
/*
   select
      @OverflowPercentStdDevValue = stdevp(OverflowPercent),
      @OverflowPercentAvgValue = avg(OverflowPercent)
   from (
      select CategoryID, sum(OverflowPercent) as OverflowPercent
      from #PatternData
      group by CategoryID
   ) SUB
   where OverflowPercent < 0;
delete
   from #PatternData
   where
      round(OverflowPercent,5) <= round((@OverflowPercentAvgValue - @OverflowPercentStdDevValue),5)
*/

/* REMOVE ENTRIES INSIDE STANDARD DEVIATION AREA */
select
   @OverflowPercentStdDevValue = stdevp(OverflowPercent),
   @OverflowPercentAvgValue = avg(OverflowPercent)
from #PatternData
print 'Overflow StdDev Value: ' + ltrim(str(@OverflowPercentStdDevValue));
print 'Overflow Avg Value: ' + ltrim(str(@OverflowPercentAvgValue));

/* REMOVE ENTRIES INSIDE STANDARD DEVIATION AREA */
delete
from #PatternData
where
   round(OverflowPercent,5) >= round((@OverflowPercentAvgValue - @OverflowPercentStdDevValue),5) and
   round(OverflowPercent,5) <= round((@OverflowPercentAvgValue + @OverflowPercentStdDevValue),5)

/* CATEGORIES */
select CategoryID
into #CategoriesData
from #PatternData
group by CategoryID

/* PARENT CATEGORY */
alter table #CategoriesData add ParentID bigint;
while exists(select * from #CategoriesData where ParentID is null) begin
   declare @categoryID bigint;
   select top 1 @categoryID=CategoryID from #CategoriesData where ParentID is null order by CategoryID;

   declare @parentID bigint;
   select top 1 @parentID=coalesce(ParentID,0) from v6_dataCategories where CategoryID=@categoryID;

   if @parentID<>0 and not exists(select * from #CategoriesData where CategoryID=@parentID) begin
      insert into #CategoriesData(CategoryID) values(@parentID)
   end

   update #CategoriesData set ParentID=@parentID where CategoryID=@categoryID
end

/* RESULT */
select
   PatternID, Text,
   CategoryID,
   --Value, PatternAverage,
   round(PaternOverflow,2) As OverflowValue,
   OverflowPercent
from #PatternData
order by OverflowPercent desc
select CategoryID, ParentID from #CategoriesData;

/* CLEAR */
drop table #EntriesData;
drop table #EntriesTypeSummary;
drop table #EntriesTypeStdDev;
drop table #EntriesTypeAverage;
drop table #PatternSummary;
drop table #PatternStdDev;
drop table #PatternAverage;
drop table #PatternData;
drop table #CategoriesData;
set nocount off;
