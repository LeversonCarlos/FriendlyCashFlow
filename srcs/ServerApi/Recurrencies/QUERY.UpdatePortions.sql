declare @resourceID varchar(128) = @paramResourceID;
declare @recurrencyID bigint = @paramRecurrencyID;

declare @recurrencyTotal smallint;
select @recurrencyTotal = count(*) 
from v6_dataEntries
where 
   RowStatus=1 and 
   ResourceID = @resourceID and 
   RecurrencyID = @recurrencyID;
print @recurrencyTotal

update v6_dataEntries
set 
   RecurrencyItem = 
   (
      select count(*)
      from v6_dataEntries as dataEntriesI
      where 
         dataEntriesI.RowStatus=1 and 
         dataEntriesI.ResourceID = @resourceID and 
         dataEntriesI.RecurrencyID = @recurrencyID and 
         dataEntriesI.Sorting < dataEntries.Sorting
    ),
   RecurrencyTotal = @recurrencyTotal
from v6_dataEntries as dataEntries
where 
   RowStatus=1 and 
   ResourceID = @resourceID and 
   RecurrencyID = @recurrencyID;

select cast(1 as bit) as OK;
