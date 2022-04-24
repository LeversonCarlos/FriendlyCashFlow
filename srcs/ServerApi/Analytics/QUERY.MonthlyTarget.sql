set nocount on;
declare @resourceID varchar(128) = @paramResourceID;
declare @searchYear smallint = @paramSearchYear;
declare @searchMonth smallint = @paramSearchMonth;
declare @typeExpense smallint = 1;
declare @typeIncome smallint = 2;
declare @yearsToAnalyseTarget smallint = 2;

/* INTERVAL */
declare @entriesInitial datetime = cast(ltrim(str(@searchYear))+'-'+ltrim(str(@searchMonth))+'-01 00:00:00' as datetime);
declare @entriesFinal datetime = dateadd(day, -1, dateadd(month,1,@entriesInitial));
declare @yearInitial datetime = dateadd(month, -11, @entriesInitial );
set @entriesInitial = dateadd(month, (-12*@yearsToAnalyseTarget), @entriesInitial);
print 'entries interval: ' + convert(varchar, @entriesInitial, 121) + ' - ' + convert(varchar, @entriesFinal, 121);

/* ACCOUNTS */
select AccountID
into #AccountIDs
from v6_dataAccounts
where ResourceID=@resourceID and RowStatus=1 and Active=1

/* ENTRIES DATA */
select
   sub.Date,
   sub.Type,
   (select top 1 p.Text from v6_dataPatterns as p where p.PatternID = sub.PatternID ) As [SerieText],
   sub.Value
into #EntriesData
from
(
   select
      cast(ltrim(str(year(SearchDate)))+'-'+ltrim(str(month(SearchDate)))+'-01 00:00:00' as datetime) as Date,
      Type,
      (case when Type=1 then 0 else PatternID end) as PatternID,
      sum(EntryValue) As Value
   from v6_dataEntries
   where
      RowStatus = 1
      and ResourceID = @resourceID
      and AccountID in (select AccountID from #AccountIDs)
      and SearchDate >= @entriesInitial
      and SearchDate <= @entriesFinal
      and TransferID is null
      and not CategoryID is null
   group by
      year(SearchDate), month(SearchDate),
      Type,
      (case when Type=1 then 0 else PatternID end)

) sub;

/* TARGET VALUE */
declare @standardDeviationValue decimal (15,3), @averageValue decimal (15,3);
   select
      @standardDeviationValue = StandardDeviationValue,
      @averageValue = AverageValue
   from
   (
      select
         coalesce(STDEVP(Value),0) as StandardDeviationValue,
         coalesce(AVG(Value),0) as AverageValue
      from #EntriesData
      where Type=@typeExpense
   ) sub;
declare @targetValue decimal (15,3);
   select top 1
      @targetValue = TargetValue
   from
   (
      select
         avg(Value) as TargetValue
      from #EntriesData as EntriesData
      where
         Value >= (@averageValue - @standardDeviationValue) AND
         Value <= (@averageValue + @standardDeviationValue)
   ) sub;


/* BALANCE DATA */
select
   Date, sum(PaidValue) as Value
into #BalanceData
from v6_dataBalance
where
   ResourceID=@resourceID
   and AccountID in (select AccountID from #AccountIDs)
   and Date <= @entriesFinal
group by Date;

/* HEADERS LIST */
select
   sub.Date,
   (
      select sum(Value)
      from #BalanceData
      where Date <= sub.Date
   ) as BalanceValue,
   @targetValue as TargetValue
into #HeadersList
from
(
   select
      Date
   from #EntriesData
   where Date >= @yearInitial
   group by
      Date
) sub;

/* ITEMS LIST */
select *
into #ItemsList
from #EntriesData
where Date >= @yearInitial;

/* RESULTS*/
select * from #HeadersList order by Date;
select * from #ItemsList order by Date asc, Type desc, Value desc;

/* CLEAR */
drop table #HeadersList;
drop table #ItemsList;
drop table #AccountIDs;
drop table #EntriesData;
drop table #BalanceData;
set nocount off;
