set nocount on;
declare @resourceID varchar(128) = @paramResourceID;
declare @searchYear smallint = @paramSearchYear;
declare @searchMonth smallint = @paramSearchMonth;
declare @typeExpense smallint = 1;
declare @typeIncome smallint = 2;
declare @yearsToAnalyseTarget smallint = 3;

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
   cast(ltrim(str(year(SearchDate)))+'-'+ltrim(str(month(SearchDate)))+'-01 00:00:00' as datetime) as SearchDate,
   Type, sum(EntryValue) As Value
into #EntriesData
from v6_dataEntries
where
   RowStatus = 1
   and ResourceID = @resourceID
   and AccountID in (select AccountID from #AccountIDs)
   and SearchDate >= @entriesInitial
   and SearchDate <= @entriesFinal
   and TransferID is null
group by year(SearchDate), month(SearchDate), Type;

/* STANDARD DEVIATION */
select
   Type,
   coalesce(STDEVP(Value),0) as StdDevValue,
   coalesce(AVG(Value),0) as AverageValue
into #StdDev
from #EntriesData
group by Type;

/* AVERAGE DATA */
select
   EntriesData.Type, avg(Value) as AverageValue
into #AverageData
from #EntriesData as EntriesData
   inner join #StdDev as StdDev on (StdDev.Type = EntriesData.Type)
where
   Value >= AverageValue - StdDevValue AND
   Value <= AverageValue + StdDevValue
group by EntriesData.Type;

/* YEAR DATA */
select
   SearchDate,
   sum(IncomeValue) as IncomeValue,
   sum(ExpenseValue) as ExpenseValue
into #YearData
from
(
   select
      SearchDate,
      (case when Type=@typeIncome then Value else 0 end) as IncomeValue,
      (case when Type=@typeExpense then Value else 0 end) as ExpenseValue
   from #EntriesData
   where SearchDate >= @yearInitial
) SUB
group by SearchDate

/* APPLY TARGET */
alter table #YearData add IncomeAverage decimal(15,2), ExpenseAverage decimal(15,2);
   update #YearData
   set
      IncomeAverage = (select sum(AverageValue) from #AverageData where Type = @typeIncome),
      ExpenseAverage = (select sum(AverageValue) from #AverageData where Type = @typeExpense)
   from #YearData;
alter table #YearData add IncomeTarget decimal(15,4), ExpenseTarget decimal(15,4);
   update #YearData
   set
      IncomeTarget = IncomeValue/IncomeAverage*100,
      ExpenseTarget = ExpenseValue/ExpenseAverage*100
   from #YearData;

/* BALANCE DATA */
select Date, sum(PaidValue) as Value
into #BalanceData
from v6_dataBalance
where
   ResourceID=@resourceID
   and AccountID in (select AccountID from #AccountIDs)
   and Date <= @entriesFinal
group by Date

/* BALANCE */

alter table #YearData add Balance decimal(15,2);
   update #YearData
   set
      Balance =
      (
         select sum(Value)
         from #BalanceData
         where Date <= YearData.SearchDate
      )
   from #YearData as YearData;

/* RESULT */
select * from #YearData;

/* CLEAR */
drop table #AccountIDs;
drop table #EntriesData;
drop table #YearData;
drop table #StdDev
drop table #AverageData
drop table #BalanceData;
set nocount off;
