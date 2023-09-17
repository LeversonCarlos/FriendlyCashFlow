set nocount on;
declare @resourceID varchar(128) = @paramResourceID;
declare @searchYear smallint = @paramSearchYear;
declare @searchMonth smallint = @paramSearchMonth;
declare @typeExpense smallint = 1;
declare @typeIncome smallint = 2;
declare @investmentAccount smallint = 3;
declare @yearsToAnalyseTarget smallint = 2;

/* INTERVAL */
declare @entriesInitial datetime = cast(ltrim(str(@searchYear))+'-'+ltrim(str(@searchMonth))+'-01 00:00:00' as datetime);
declare @entriesFinal datetime = dateadd(day, -1, dateadd(month,1,@entriesInitial));
declare @yearInitial datetime = dateadd(month, -11, @entriesInitial );
set @entriesInitial = dateadd(month, (-12*@yearsToAnalyseTarget), @entriesInitial);
print 'entries interval: ' + convert(varchar, @entriesInitial, 121) + ' - ' + convert(varchar, @entriesFinal, 121);

/* ACCOUNTS */
select AccountID, Type
into #AccountIDs
from v6_dataAccounts
where ResourceID=@resourceID and RowStatus=1 and Active=1

/* ENTRIES DATA */
select
   sub.Date,
   sub.Type,
   sub.AccountID,
   (select top 1 p.Text from v6_dataPatterns as p where p.PatternID = sub.PatternID ) As SerieText,
   sub.Value
into #EntriesData
from
(
   select
      cast(ltrim(str(year(SearchDate)))+'-'+ltrim(str(month(SearchDate)))+'-01 00:00:00' as datetime) as Date,
      Type,
      AccountID,
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
      AccountID,
      (case when Type=1 then 0 else PatternID end)

) sub;

/* LOCATE DATA FROM INVESTMENT ACCOUNTS */
select
   Date,
   Type,
   Value * (case when Type=@typeIncome then 1 else -1 end) as Value,
   SerieText
into #InvestmentData
from #EntriesData
where
   AccountID in ( select AccountID from #AccountIDs where Type=@investmentAccount );

/* REMOVE CURRENT DATA FROM INVESTMENT ACCOUNTS */
delete
from #EntriesData
where
   AccountID in ( select AccountID from #AccountIDs where Type=@investmentAccount );

/* TARGET VALUE */
declare @targetValue decimal (15,3);
   select sum(coalesce(Value,0)) as Value
   into #Average
   from #EntriesData
   where Type=@typeExpense
   group by Date;
WITH AvgStd AS (
      SELECT
         AVG(Value) AS avgnum,
         STDEVP(Value) AS stdnum
      from #Average
   )
   select
   @targetValue = AVG(Value)
   from #Average
   CROSS JOIN AvgStd
   where
      Value >= (avgnum - stdnum) and
      Value <= (avgnum + stdnum);
drop table #Average

/* INSERT SUMARIZED DATA FROM INVESTMENT ACCOUNTS */
declare @investmentText varchar(50);
   select top 1 @investmentText=SerieText from #InvestmentData where Type=@typeIncome;
insert into #EntriesData
   select
      Date,
      (case when Value < 0 then @typeExpense else @typeIncome end) as Type,
      0 As AccountID,
      @investmentText as SerieText,
      Value
   from
   (
      select
         Date,
         sum(Value) as Value
      from #InvestmentData
      group by
         Date
   ) sub
   where Value <> 0;

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
   select Date
   from #EntriesData
   where Date >= @yearInitial
   group by Date
) sub;

/* ITEMS LIST */
select
   Date,
   Type,
   SerieText,
   sum(Value) as Value
into #ItemsList
from #EntriesData
where
   Date >= @yearInitial
group by
   Date,
   Type,
   SerieText;

/* RESULTS*/
select * from #HeadersList order by Date;
select * from #ItemsList order by Date asc, Type desc, Value desc;

/* CLEAR */
drop table #HeadersList;
drop table #ItemsList;
drop table #AccountIDs;
drop table #EntriesData;
drop table #BalanceData;
drop table #InvestmentData;
set nocount off;
