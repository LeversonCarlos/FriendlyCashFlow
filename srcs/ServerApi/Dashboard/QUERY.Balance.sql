set nocount on;

declare @resourceID varchar(128) = @paramResourceID;
declare @searchYear smallint = @paramSearchYear;
declare @searchMonth smallint = @paramSearchMonth;
declare @searchExcludeTransfers smallint = @paramExcludeTransfers;
declare @typeExpense smallint = 1;
declare @typeIncome smallint = 2;
declare @accountTypeCreditCard smallint = 2;

/* INTERVAL */
declare @initialDate datetime = cast(ltrim(str(@searchYear))+'-'+ltrim(str(@searchMonth))+'-01' as datetime);
declare @finalDate datetime = dateadd(day, -1, dateadd(month,1,@initialDate));

/* ACCOUNTS */
select AccountID, Text, Type, DueDay, @initialDate as InitialDate, @finalDate as FinalDate
into #accounts
from v6_dataAccounts
where ResourceID=@resourceID and RowStatus=1 and Active=1;

/* ADJUST INTERVAL FOR DUE DATE ON CREDIT CARDS */
if (@searchExcludeTransfers=0) begin
   update #accounts
   set FinalDate = dateadd(day, -1, dateadd(month,2,InitialDate))
   from #accounts as accounts
   where Type=@accountTypeCreditCard and DueDay < DATEPART(day, getdate());
end

/* ADD DUEDAY TO TEXT ON CREDIT CARDS */
alter table #accounts add DueDate datetime;
   update #accounts
   set DueDate = cast(ltrim(str(year(FinalDate)))+'-'+ltrim(str(month(FinalDate)))+'-'+ltrim(str(DueDay)) as datetime)
   from #accounts
   where Type=@accountTypeCreditCard and DueDay>0
update #accounts
set Text = Text + ' - '+ convert(varchar(5),DueDate,3) +''
from #accounts as accounts
where Type=@accountTypeCreditCard

/* ENTRIES */
select
   AccountID, Type, Paid, sum(EntryValue) as EntryValue
into #entries
from
(
   select
      entries.AccountID, entries.Type, entries.Paid,
      entries.EntryValue * (case when entries.Type=@typeExpense then -1 else 1 end) As EntryValue
   from #accounts as accounts
      inner join v6_dataEntries as entries on
      (
         entries.RowStatus = 1
         and entries.ResourceID=@resourceID
         and entries.AccountID = accounts.AccountID
         and entries.SearchDate >= cast(convert(varchar(10),accounts.InitialDate,121)+' 00:00:00' as datetime)
         and entries.SearchDate <= cast(convert(varchar(10),accounts.FinalDate,121)+' 23:59:59' as datetime)
         and ( @searchExcludeTransfers=0 or coalesce(entries.TransferID,'')='' )
      )
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

/* PAID ENTRIES */
alter table #accounts add PaidIncome decimal(15,2), PaidExpense decimal(15,2);
update #accounts
set
   PaidIncome = coalesce((select coalesce(sum(EntryValue),0) from #entries as entries where entries.AccountID= accounts.AccountID and entries.Type=@typeIncome and entries.Paid=1),0),
   PaidExpense = coalesce((select coalesce(sum(EntryValue),0) from #entries as entries where entries.AccountID= accounts.AccountID and entries.Type=@typeExpense and entries.Paid=1),0)
from #accounts as accounts;

/* PAID ENTRIES: CURRENT BALANCE */
update #accounts
set
   CurrentBalance += (PaidIncome + PaidExpense)
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
