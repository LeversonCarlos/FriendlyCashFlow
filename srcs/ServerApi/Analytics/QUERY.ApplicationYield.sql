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
   and Active=1
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
   E.Value As OriginalGain,
   E.Value As Gain,
   cast(0 as decimal(15,5)) As GainPercentual,
   cast(0 as decimal(15,5)) As Percentual
into #RESULT
from #ENTRIES As E
   left join #ACCOUNTS As A on (A.AccountID = E.AccountID)

update #RESULT
set Balance =
   (
      select sum(B.TotalValue)
      from #BALANCE As B
      where
         B.AccountID = R.AccountID
         and B.[Date] <= R.[Date]
   )
from #RESULT as R
where Balance is null;

delete
from #RESULT
where Balance is null;

select [Date], AccountID, abs(Gain) as GainLosses
into #NegativeGain
from #RESULT
where Gain < 0;

create table #NegativeGainReview( [Date] datetime, Gain float, GainPercentual float, GainReduce float);

while exists(select * from #NegativeGain) begin
   declare @gainDate datetime, @gainAccountID bigint, @gainLosses float ;
      select top 1 @gainDate=[Date], @gainAccountID=AccountID, @gainLosses=GainLosses
      from #NegativeGain
      order by AccountID, [Date];

   delete from #NegativeGainReview;
      insert into #NegativeGainReview
      select Date, Gain, 0 as GainPercentual, 0 as GainReduce
      from #RESULT
      where
         AccountID=@gainAccountID and Gain > 0;

   declare @gainTotal float;
      select @gainTotal=sum(Gain) from #NegativeGainReview;

   update #NegativeGainReview
      set GainPercentual = Gain / @gainTotal;
   update #NegativeGainReview
      set GainReduce = round(@gainLosses * GainPercentual,2);

   update #RESULT
   set
      Gain = RESULT.Gain - NegativeGainReview.GainReduce
   from #RESULT as RESULT
      inner join #NegativeGainReview as NegativeGainReview on (NegativeGainReview.[Date]=RESULT.[Date])
   where
      RESULT.AccountID = @gainAccountID;

   update #RESULT
   set Gain = 0
   from #RESULT as RESULT
   where
      AccountID = @gainAccountID and
      [Date] = @gainDate;

   delete from #NegativeGain where AccountID=@gainAccountID and [Date]=@gainDate;
end

drop table #NegativeGainReview;
drop table #NegativeGain;

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
