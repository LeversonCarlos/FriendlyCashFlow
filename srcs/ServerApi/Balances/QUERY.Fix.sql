declare @resourceID varchar(128) = @paramResourceID;
declare @accountID bigint = @paramAccountID;
declare @searchYear smallint = @paramSearchYear;
declare @searchMonth smallint = @paramSearchMonth;

-- BALANCE DATE
declare @balanceDate datetime = cast( ltrim(str(@searchYear)) +'-'+ right('0'+ltrim(str(@searchMonth)),2) +'-'+ '01' as datetime);

-- CURRENT BALANCE
declare @currentTotalValue float, @currentPaidValue float
select top 1  @currentTotalValue=TotalValue, @currentPaidValue=PaidValue
from v6_dataBalance
where RowStatus=1 and resourceID=@resourceID and Date=@balanceDate and AccountID=@accountID;

-- DEFINE INTERVAL
declare @initialDate datetime = cast(convert(varchar(7), @balanceDate,121)+'-01 00:00:00' as datetime);
declare @finalDate datetime = dateadd(SECOND,-1,dateadd(month,1,@initialDate));

-- RETRIEVE CURRENT VALUES
declare @totalValue float, @paidValue float
select @totalValue=coalesce(sum(TotalValue),0), @paidValue=coalesce(sum(PaidValue),0)
from
(
    select
        EntryValue as TotalValue,
        (case when Paid=1 then EntryValue else 0 end) as PaidValue
    from
    (
        select
            coalesce(EntryValue,0) * (case when Type=2 then 1 else -1 end) As EntryValue, Paid
        from v6_dataEntries
        where RowStatus=1 and ResourceID=@resourceID and AccountID=@accountID and SearchDate>=@initialDate and SearchDate<=@finalDate
    ) SUB
) SUB2

-- VALIDATE
if (@totalValue=@currentTotalValue and @paidValue=@currentPaidValue) begin
    select '' as KeyValue;
    return;
end

-- WRITE UPDATE COMMAND
declare @command nvarchar(2048);
set @command = '' +
   'update v6_dataBalance ' +
   'set TotalValue={TotalValue}, PaidValue={PaidValue} ' +
   'where ' +
      'RowStatus=1 and ' +
      'ResourceID = ''{ResourceID}'' and ' +
      '[Date] = ''{Date}'' and ' +
      'AccountID = {AccountID} ' +
   '';
set @command = replace(@command, '{TotalValue}', convert(varchar(15),@totalValue));
set @command = replace(@command, '{PaidValue}', convert(varchar(15),@paidValue));
set @command = replace(@command, '{ResourceID}', @resourceID);
set @command = replace(@command, '{Date}', convert(varchar(10),@initialDate,121));
set @command = replace(@command, '{AccountID}', ltrim(str(@accountID)));
print @command;

-- EXECUTE FIX
execute sp_executesql @command;

-- RETURN WARNING MESSAGE
select 'CurrentTotalValue:' + convert(varchar(15),@currentTotalValue) as KeyValue union
select 'TotalValue:' + convert(varchar(15),@totalValue) as KeyValue union
select 'CurrentPaidValue:' + convert(varchar(15),@currentPaidValue) as KeyValue union
select 'PaidValue:' + convert(varchar(15),@paidValue) as KeyValue;
