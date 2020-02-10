set nocount on;
declare @resourceID varchar(128) = 'd219724c-b985-4dd0-aa21-0a38dec9e833';
declare @searchYear smallint = 2020;
declare @searchMonth smallint = 2;
declare @typeExpense smallint = 1;
declare @typeIncome smallint = 2;

/* MONTH DATA INTERVAL */
declare @monthInitial datetime = cast(ltrim(str(@searchYear))+'-'+ltrim(str(@searchMonth))+'-01 00:00:00' as datetime);
declare @monthFinal datetime = dateadd(day, -1, dateadd(month,1,@monthInitial));
set @monthFinal = cast(convert(varchar(10),@monthFinal,121) +' 23:59:59' as datetime)
print 'month interval: ' + convert(varchar, @monthInitial, 121) + ' - ' + convert(varchar, @monthFinal, 121);

/* ENTRIES DATA INTERVAL */
declare @entriesInitial datetime = dateadd(month, -13, @monthInitial);
declare @entriesFinal datetime = @monthFinal;
print 'entries interval: ' + convert(varchar, @entriesInitial, 121) + ' - ' + convert(varchar, @entriesFinal, 121);

/* ENTRIES DATA */
select
   identity(int,1,1) as Ordination, PatternID, sum(EntryValue) As Value
into #EntriesData
from v6_dataEntries
where
   RowStatus = 1
   and ResourceID = @resourceID
   and AccountID in (select AccountID from v6_dataAccounts where ResourceID=@resourceID and RowStatus=1 and Active=1)
   and SearchDate >= @entriesInitial
   and SearchDate <= @entriesFinal
   and Type = @typeExpense
   and TransferID is null
group by PatternID
order by sum(EntryValue) desc;

/* PERCENTAGE */
declare @sumValue decimal(15,2)
select @sumValue=sum(Value) from #EntriesData;
alter table #EntriesData add perc decimal(15,4);
   update #EntriesData set perc=Value/@sumValue;

/* ACCUMULATED PERCENTAGE */
alter table #EntriesData add Pareto decimal(15,4);
   update #EntriesData 
   set Pareto = (select sum(perc) from #EntriesData as i where i.Ordination <= o.Ordination )
   from #EntriesData as o;

/* DESCRIPTION */
alter table #EntriesData add Text varchar(500);
update #EntriesData
set Text = (select top 1 Text from v6_dataPatterns where PatternID=entriesData.PatternID)
from #EntriesData as entriesData


/* RESULT */
select Text, Value, Pareto
from #EntriesData 
where
   Pareto <= 0.9;

drop table #EntriesData
