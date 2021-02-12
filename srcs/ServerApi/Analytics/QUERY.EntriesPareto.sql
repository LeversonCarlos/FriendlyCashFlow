set nocount on;
declare @resourceID varchar(128) = @paramResourceID;
declare @searchYear smallint = @paramSearchYear;
declare @searchMonth smallint = @paramSearchMonth;
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
   and not CategoryID is null
group by PatternID
order by sum(EntryValue) desc;

/* PERCENTAGE */
declare @sumValue decimal(15,2)
select @sumValue=sum(Value) from #EntriesData;
alter table #EntriesData add perc decimal(15,4);
   update #EntriesData set perc=Value/@sumValue;

/* STANDARD DEVIATION */
declare @StdDevValue decimal, @AverageValue decimal
select
   @StdDevValue=STDEV(Value),
   @AverageValue=AVG(Value)
from #EntriesData;
print 'StdDevValue: ' + str(@StdDevValue);
print 'AverageValue: ' + str(@AverageValue);
delete from #EntriesData where Value < @AverageValue - @StdDevValue;

/* PARETO */
alter table #EntriesData add Pareto decimal(15,4);
   update #EntriesData
   set Pareto = (select sum(perc) from #EntriesData as i where i.Ordination <= o.Ordination )
   from #EntriesData as o;
delete from #EntriesData where not Ordination in (select top 50 Ordination from #EntriesData)

/* DESCRIPTION */
alter table #EntriesData add Text varchar(500), CategoryID bigint;
update #EntriesData
set
   CategoryID = dataPatterns.CategoryID,
   Text = dataPatterns.Text
from #EntriesData as entriesData
   left join v6_dataPatterns as dataPatterns on (dataPatterns.PatternID=entriesData.PatternID);

/* CATEGORIES */
select CategoryID
into #CategoriesData
from #EntriesData
group by CategoryID

/* PARENT CATEGORY */
alter table #CategoriesData add ParentID bigint;
while exists(select * from #CategoriesData where ParentID is null) begin
   declare @categoryID bigint;
   select top 1 @categoryID=CategoryID from #CategoriesData where ParentID is null order by CategoryID;

   declare @parentID bigint;
   select top 1 @parentID=coalesce(ParentID,0) from v6_dataCategories where CategoryID=@categoryID;

   if @parentID<>0 and not exists(select * from #CategoriesData where CategoryID=@parentID) begin
      insert into #CategoriesData(CategoryID) values(@parentID)
   end

   update #CategoriesData set ParentID=@parentID where CategoryID=@categoryID
end


/* RESULT */
select Text, Value, CategoryID, Pareto from #EntriesData;
select CategoryID, ParentID from #CategoriesData;

drop table #EntriesData
drop table #CategoriesData;
