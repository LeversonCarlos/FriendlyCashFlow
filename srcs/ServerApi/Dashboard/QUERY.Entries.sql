set nocount on;

declare @resourceID varchar(128) = @paramResourceID;
declare @typeExpense smallint = 1;
declare @typeIncome smallint = 2;
declare @finalDate datetime = dateadd(day, 7, getdate())

/* ACCOUNTS */
select AccountID
into #accountIDs
from v6_dataAccounts
where
   ResourceID=@resourceID and RowStatus=1 and Active=1;

/* ENTRIES */
select EntryID
into #entryIDs
from v6_dataEntries
where
   RowStatus = 1
   and ResourceID=@resourceID
   and AccountID in (select AccountID from #accountIDs)
   and SearchDate <= cast(convert(varchar(10),@finalDate,121)+' 23:59:59' as datetime)
   and Paid = 0


/* RESULT */
select
   dataEntries.EntryID,
   dataEntries.Type,
   dataEntries.Text,
   dataEntries.SearchDate,
   dataEntries.DueDate,
   dataEntries.EntryValue * (case when dataEntries.Type=@typeExpense then -1 else 1 end) As EntryValue,
   dataEntries.AccountID,
   dataAccounts.Text As AccountText,
   dataEntries.CategoryID,
   dataCategories.HierarchyText As CategoryText,
   dataEntries.Paid,
   dataEntries.PayDate,
   dataEntries.PatternID,
   dataEntries.RecurrencyID,
   dataEntries.RecurrencyItem,
   dataEntries.RecurrencyTotal,
   dataEntries.TransferID,
   dataEntries.Sorting
from v6_dataEntries As dataEntries
   left join v6_dataCategories As dataCategories on (dataCategories.CategoryID = dataEntries.CategoryID)
   left join v6_dataAccounts As dataAccounts on (dataAccounts.AccountID = dataEntries.AccountID)
where
   dataEntries.EntryID in (select EntryID from #entryIDs)
order by
   dataEntries.Sorting;

/* CLEAR */
drop table #accountIDs;
drop table #entryIDs;
