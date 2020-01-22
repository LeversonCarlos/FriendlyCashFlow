use FriendlyCashFlow
set nocount on;

declare @resourceID varchar(128) = @paramResourceID;
declare @searchYear smallint = @paramSearchYear;
declare @searchMonth smallint = @paramSearchMonth;
declare @typeExpense smallint = 1;
declare @typeIncome smallint = 2;

/* INTERVAL */
declare @initialDate datetime = cast(ltrim(str(@searchYear))+'-'+ltrim(str(@searchMonth))+'-01' as datetime);
declare @finalDate datetime = dateadd(day, -1, dateadd(month,1,@initialDate));

/* ACCOUNTS */
select AccountID, Text, Type
into #accounts
from v6_dataAccounts
where ResourceID=@resourceID and RowStatus=1 and Active=1;

/* ENTRIES */
select
   AccountID, Type, Paid, sum(EntryValue) as EntryValue
into #entries
from
(
   select
      AccountID, Type, Paid,
      EntryValue * (case when Type=@typeExpense then -1 else 1 end) As EntryValue
   from v6_dataEntries
   where
      RowStatus = 1
      and ResourceID=@resourceID
      and AccountID in (select AccountID from #accounts)
      and SearchDate >= cast(convert(varchar(10),@initialDate,121)+' 00:00:00' as datetime)
      and SearchDate <= cast(convert(varchar(10),@finalDate,121)+' 23:59:59' as datetime)
) SUB
group by AccountID, Type, Paid;

/* BALANCE */
select AccountID, sum(TotalValue) as TotalValue, sum(PaidValue) as PaidValue
into #balance
from v6_dataBalance
where
   ResourceID = @resourceID
   and AccountID in (select AccountID from #accounts)
   and Date < dateFromParts(@searchYear, @searchMonth, 1)
group by AccountID;

/* CURRENT BALANCE */
alter table #accounts add CurrentBalance decimal(15,2);
   update #accounts
   set CurrentBalance =
   (
      select coalesce(sum(PaidValue),0)
      from #balance as balance
      where
         balance.AccountID = accounts.AccountID
    )
   from #accounts as accounts;

/* INITIALIZE FORECASTS */
alter table #accounts add Forecast decimal(15,2);
   update #accounts
   set
      Forecast =
      (
         select coalesce(sum(TotalValue),0) - coalesce(sum(PaidValue),0)
         from #balance as balance
         where
            balance.AccountID = accounts.AccountID
       )
   from #accounts as accounts;

/* REVIEW FORECASTS */
alter table #accounts add IncomeForecast decimal(15,2), ExpenseForecast decimal(15,2);
   update #accounts
   set
      IncomeForecast = (case when Forecast>0 then Forecast else 0 end),
      ExpenseForecast = (case when Forecast<0 then Forecast else 0 end);
alter table #accounts drop column Forecast;

/* PAID INCOMES */
update #accounts
set
   CurrentBalance += coalesce((select coalesce(sum(EntryValue),0) from #entries as entries where entries.AccountID= accounts.AccountID and entries.Type=@typeIncome and entries.Paid=1),0)
from #accounts as accounts;

/* PAID EXPENSES */
update #accounts
set
   CurrentBalance += coalesce((select sum(EntryValue) from #entries as entries where entries.AccountID= accounts.AccountID and entries.Type=@typeExpense and entries.Paid=1),0)
from #accounts as accounts;

/* UNPAID ENTRIES */
update #accounts
set
   IncomeForecast += coalesce((select coalesce(sum(EntryValue),0) from #entries as entries where entries.AccountID= accounts.AccountID and entries.Type=@typeIncome and entries.Paid=0),0),
   ExpenseForecast += coalesce((select coalesce(sum(EntryValue),0) from #entries as entries where entries.AccountID= accounts.AccountID and entries.Type=@typeExpense and entries.Paid=0),0)
from #accounts as accounts;

/* RESULT */
select * from #accounts;

/* CLEAR */
drop table #accounts
drop table #entries
drop table #balance;
