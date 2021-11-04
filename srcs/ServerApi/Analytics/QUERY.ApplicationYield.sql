set nocount on;
declare @resourceID varchar(128) = @paramResourceID;
declare @searchYear smallint = @paramSearchYear;
declare @searchMonth smallint = @paramSearchMonth;
declare @applicationType  smallint = 3;

/* INTERVAL */
declare @initialDate datetime = cast(ltrim(str(@searchYear))+'-'+ltrim(str(@searchMonth))+'-01 00:00:00' as datetime);
declare @finalDate datetime = dateadd(day, -1, dateadd(month,1,@initialDate));
declare @yearInitial datetime = dateadd(month, -11, @initialDate );
set @initialDate = dateadd(month,-11, @initialDate);
print 'entries interval: ' + convert(varchar, @initialDate, 121) + ' - ' + convert(varchar, @finalDate, 121);

select AccountID, [Text]
into #ACCOUNTS
from v6_dataAccounts with(nolock)
where
   RowStatus = 1
   and ResourceID=@resourceID
   and [Type]=@applicationType;

select
   cast(convert(varchar(7), SearchDate, 121)+'-01' As date) AS [Date],
   AccountID,
   sum(EntryValue * (case when [Type]=2 then 1 else -1 end)) As [Value]
into #ENTRIES
from v6_dataEntries with(nolock)
where
   RowStatus=1
   and ResourceID=@resourceID
   and SearchDate >= @initialDate
   and SearchDate <= @finalDate
   and TransferID is null
   and AccountID in ( select AccountID from #ACCOUNTS )
group by
   convert(varchar(7), SearchDate, 121),
   AccountID;

select [Date], AccountID, TotalValue
into #BALANCE
from v6_dataBalance with(nolock)
where
   RowStatus = 1
   and ResourceID=@resourceID
   and [Date] <= @finalDate
   and AccountID in ( select distinct AccountID from #ACCOUNTS )
order by
   [Date], AccountID

select
   E.[Date],
   E.AccountID,
   A.Text As AccountText,
   (
      select sum(B.TotalValue)
      from #BALANCE As B
      where
         B.AccountID = E.AccountID
         and B.[Date] < E.[Date]
   ) As Balance,
   E.Value As Gain,
   cast(0 as decimal(15,5)) As GainPercentual,
   cast(0 as decimal(15,5)) As Percentual
into #RESULT
from #ENTRIES As E
   left join #ACCOUNTS As A on (A.AccountID = E.AccountID)

delete
from #RESULT
where Balance is null;

update #RESULT
set
   GainPercentual =
      cast(Gain as decimal(15,5)) / cast(Balance as decimal(15,5)) * 100

update #RESULT
set
   GainPercentual = 0
where GainPercentual < 0

select
   [Date],
   sum(GainPercentual) as TotalGainPercentual
into #TotalGainPercentual
from #RESULT
group by [Date];

update #TotalGainPercentual
set TotalGainPercentual=1
where coalesce(TotalGainPercentual,0)=0

update #RESULT
set
   Percentual =
      GainPercentual / (select top 1 T.TotalGainPercentual from #TotalGainPercentual as T where T.[Date]=RESULT.[Date] ) * 100
from #RESULT as RESULT

select *
from #RESULT
order by
   [Date], AccountText

drop table #TotalGainPercentual;
drop table #ACCOUNTS;
drop table #ENTRIES;
drop table #BALANCE;
drop table #RESULT
