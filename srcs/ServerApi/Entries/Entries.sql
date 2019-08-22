declare @resourceID varchar(128) = 'a0e03962-54a3-47be-a733-652311ef196a';
declare @accountID bigint = 0;
declare @dateYear smallint = 2019;
declare @dateMonth smallint = 08;
declare @searchText varchar(255) = '';

set nocount on;

declare @typeExpense smallint = 1;
declare @typeIncome smallint = 2;

/* INITIAL COMMAND */
declare @command nvarchar(max);
set @command = '
   insert into #IDs
   select EntryID
   from v6_dataEntries
   where 
      RowStatus = 1
   ';

/* RESOURCE CONDITION */
set @command = @command + ' and ResourceID = '''+ @resourceID +'''';

/* ACCOUNT CONDITION */
if (@accountID <> 0) begin
   set @command = @command + char(10) + ' and AccountID = '+ ltrim(str(@accountID));
end
if (@accountID = 0) begin
   set @command = @command + char(10) + ' and AccountID <> 0'; /* just to be using the table index */
end

/* DATE CONDITION */
if (@dateYear <> 0 and @dateMonth <> 0) begin
   declare @initialDate datetime = cast(ltrim(str(@dateYear))+'-'+ltrim(str(@dateMonth))+'-01' as datetime);
   declare @finalDate datetime = dateadd(day, -1, dateadd(month,1,@initialDate));
   set @command = @command + char(10) + ' and SearchDate >= '''+ convert(varchar(10),@initialDate,121) +'''';
   set @command = @command + char(10) + ' and SearchDate <= '''+ convert(varchar(10),@finalDate,121) +'''';
end

/* SEARCH TEXT CONDITION */
if (@searchText <> '') begin
   SELECT value as searchTerm into #searchTerms FROM STRING_SPLIT(@searchText, ' ') where value <> '';
   declare @searchTextCommand nvarchar(max) = '';
   while (select count(*) from #searchTerms)>0 begin
      declare @searchTerm varchar(255);
      select top 1 @searchTerm=searchTerm from #searchTerms;
      set @searchTextCommand = @searchTextCommand + 
         (case when @searchTextCommand='' then '' else ' and ' end) + 
         'Text like ''%' + @searchTerm + '%''';
      delete from #searchTerms where searchTerm=@searchTerm;
   end
   if (@searchTextCommand <> '') begin
      set @command = @command + char(10) + ' and (' + @searchTextCommand + ')';
   end
   drop table #searchTerms;
end

/* LOCATE ENTRY IDs */
print @command;
create table #IDs (EntryID bigint);
execute sp_executesql @command;

/* ENTRIES MAIN DATA */
select 
   dataEntries.EntryID, 
   dataEntries.Type, 
   dataEntries.Text, 
   dataEntries.DueDate, 
   dataEntries.Value * (case when dataEntries.Type=@typeExpense then -1 else 1 end) As EntryValue, 
   dataEntries.AccountID, 
   dataAccounts.Text As AccountText,
   dataEntries.CategoryID, 
   dataCategories.HierarchyText As CategoryText,
   dataEntries.Paid, 
   dataEntries.PayDate, 
   dataEntries.PatternID, 
   dataEntries.RecurrencyID, 
   dataEntries.TransferID, 
   dataEntries.Sorting 
into #dataEntries
from v6_dataEntries As dataEntries 
   inner join v6_dataCategories As dataCategories on (dataCategories.CategoryID = dataEntries.CategoryID)
   left join v6_dataAccounts As dataAccounts on (dataAccounts.AccountID = dataEntries.AccountID)
where 
   dataEntries.EntryID in (select EntryID from #IDs)
order by 
   dataEntries.Sorting;

/* GROUP TRANSFER DATA */
select TransferID,
   max(TransferIncomeEntryID) as TransferIncomeEntryID, max(TransferExpenseEntryID) as TransferExpenseEntryID,
   max(TransferIncomeAccountID) as TransferIncomeAccountID, max(TransferExpenseAccountID) as TransferExpenseAccountID,
   max(TransferIncomeAccountText) as TransferIncomeAccountText, max(TransferExpenseAccountText) as TransferExpenseAccountText
into #dataTransfer
from
(
   select 
      TransferID, 
      (case when Type=@typeIncome then EntryID else 0 end) as TransferIncomeEntryID, 
      (case when Type=@typeExpense then EntryID else 0 end) as TransferExpenseEntryID, 
      (case when Type=@typeIncome then AccountID else 0 end) as TransferIncomeAccountID, 
      (case when Type=@typeExpense then AccountID else 0 end) as TransferExpenseAccountID, 
      (case when Type=@typeIncome then AccountText else '' end) as TransferIncomeAccountText, 
      (case when Type=@typeExpense then AccountText else '' end) as TransferExpenseAccountText 
   from 
   (
      select TransferID, Type, EntryID, AccountID, AccountText
      from #dataEntries
      where 
         coalesce(TransferID,'') <> ''
   ) SUB1
) SUB2
group by TransferID;

/* APPLY TRANSFER DATA */
alter table #dataEntries add 
   TransferIncomeEntryID bigint, TransferExpenseEntryID bigint, 
   TransferIncomeAccountID bigint, TransferExpenseAccountID bigint, 
   TransferIncomeAccountText varchar(50), TransferExpenseAccountText varchar(50);
update #dataEntries 
set 
   TransferIncomeEntryID = dataTransfer.TransferIncomeEntryID,
   TransferExpenseEntryID = dataTransfer.TransferExpenseEntryID,
   TransferIncomeAccountID = dataTransfer.TransferIncomeAccountID, 
   TransferExpenseAccountID = dataTransfer.TransferExpenseAccountID, 
   TransferIncomeAccountText = dataTransfer.TransferIncomeAccountText, 
   TransferExpenseAccountText = dataTransfer.TransferExpenseAccountText 
from #dataEntries as dataEntries
   inner join #dataTransfer as dataTransfer on (dataTransfer.TransferID=dataEntries.TransferID)
where 
   coalesce(dataEntries.TransferID,'') <> '';

/* BALANCE */
alter table #dataEntries add BalanceTotalValue decimal(15,2), BalancePaidValue decimal(15,2);
if (@dateYear <> 0 and @dateMonth <> 0) begin

   select AccountID, TotalValue, PaidValue 
   into #dataBalance
   from v6_dataBalance as dataBalance
   where 
      ResourceID = @resourceID
      and AccountID in (select distinct AccountID from #dataEntries)
      and Date = dateadd(month,-1,cast(ltrim(str(@dateYear))+'-'+ltrim(str(@dateMonth))+'-01' as datetime)); 

   update #dataEntries
   set 
      BalanceTotalValue = 
         (select top 1 TotalValue from #dataBalance as dataBalance where dataBalance.AccountID=dataEntries.AccountID) + 
         (select sum(EntryValue) from #dataEntries as dataEntriesI where dataEntriesI.AccountID=dataEntries.AccountID and dataEntriesI.Sorting <= dataEntries.Sorting), 
      BalancePaidValue = 
         (select top 1 PaidValue from #dataBalance as dataBalance where dataBalance.AccountID=dataEntries.AccountID) + 
         (select sum(EntryValue) from #dataEntries as dataEntriesI where dataEntriesI.AccountID=dataEntries.AccountID and dataEntriesI.Sorting <= dataEntries.Sorting and dataEntriesI.Paid=1)
   from #dataEntries as dataEntries;

   drop table #dataBalance;
end

/* RESULT */
select * from #dataEntries order by Sorting;

/* CLEAR */
drop table #dataTransfer;
drop table #dataEntries;
drop table #IDs;
set nocount off;