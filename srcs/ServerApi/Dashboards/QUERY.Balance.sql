set nocount on;

declare @resourceID varchar(128) = 'a0e03962-54a3-47be-a733-652311ef196a';
declare @searchYear smallint = 2020;
declare @searchMonth smallint = 1;
declare @typeExpense smallint = 1;
declare @typeIncome smallint = 2;

/* INTERVAL */
declare @initialDate datetime = cast(ltrim(str(@searchYear))+'-'+ltrim(str(@searchMonth))+'-01' as datetime);
declare @finalDate datetime = dateadd(day, -1, dateadd(month,1,@initialDate));

/* ACCOUNTS */
select AccountID, Text, Type, ClosingDay, DueDay 
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

/* RESULT */
select * from #accounts;
select * from #balance;
select * from #entries

/* CLEAR */
drop table #accounts
drop table #entries
drop table #balance;