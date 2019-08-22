declare @resourceID varchar(128) = 'a0e03962-54a3-47be-a733-652311ef196a';
declare @accountID bigint = 0;
declare @dateYear smallint = 2019;
declare @dateMonth smallint = 07;
declare @searchText varchar(255) = '';

set nocount on;

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

select 
   dataEntries.EntryID, 
   dataEntries.Type, 
   dataEntries.Text, 
   dataEntries.DueDate, 
   dataEntries.Value, 
   dataEntries.AccountID, 
   dataAccounts.Text As AccountText,
   dataEntries.CategoryID, 
   dataCategories.HierarchyText As CategoryText,
   dataEntries.Paid, 
   dataEntries.PayDate, 
   dataEntries.PatternID, 
   dataEntries.RecurrencyID, 
   dataEntries.TransferID 
from v6_dataEntries As dataEntries 
   inner join v6_dataCategories As dataCategories on (dataCategories.CategoryID = dataEntries.CategoryID)
   left join v6_dataAccounts As dataAccounts on (dataAccounts.AccountID = dataEntries.AccountID)
where 
   dataEntries.EntryID in (select EntryID from #IDs)
order by 
   dataEntries.Sorting;

drop table #IDs;
set nocount off;