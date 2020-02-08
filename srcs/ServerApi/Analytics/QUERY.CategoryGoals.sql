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

/* YEAR DATA INTERVAL */
declare @yearFinal datetime = dateadd(second, -1, @monthInitial);
declare @yearInitial datetime = dateadd(month, -13, @monthInitial);
print 'year interval: ' + convert(varchar, @yearInitial, 121) + ' - ' + convert(varchar, @yearFinal, 121);

/* ENTRIES DATA INTERVAL */
declare @entriesInitial datetime = @yearInitial;
declare @entriesFinal datetime = dateadd(second, -1, @monthFinal);
print 'entries interval: ' + convert(varchar, @entriesInitial, 121) + ' - ' + convert(varchar, @entriesFinal, 121);

/* ENTRIES DATA */
select SearchDate, CategoryID, sum(EntryValue) As Value
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
group by SearchDate, CategoryID;

/* STANDARD DEVIATION */
select
   CategoryID,
   coalesce(STDEVP(Value),0) as StdDevValue,
   coalesce(AVG(Value),0) as AverageValue
into #YearStdDev
from #EntriesData
group by CategoryID;

/* AVERAGE DATA */
select
   EntriesData.CategoryID, avg(Value) as AverageValue
into #AverageData
from #EntriesData as EntriesData
   inner join #YearStdDev as YearStdDev on (YearStdDev.CategoryID = EntriesData.CategoryID)
where
   Value >= AverageValue - StdDevValue AND
   Value <= AverageValue + StdDevValue
group by EntriesData.CategoryID;

/* MONTH DATA */
select CategoryID
into #MonthData
from #EntriesData
group by CategoryID

/* APPLY MONTH VALUES */
alter table #MonthData add [Value] decimal(15,2), AverageValue decimal(15,2);
update #MonthData
set
   Value =
   (
      select sum(Value)
      from #EntriesData
      where SearchDate >= @monthInitial and SearchDate <= @monthFinal and CategoryID = MonthData.CategoryID
    ),
   AverageValue =
   (
      select top 1 AverageValue
      from #AverageData
      where CategoryID = MonthData.CategoryID
    )
from #MonthData as MonthData;

/* PARENT DATA */
alter table #MonthData add ParentID bigint, Text varchar(4000);
while exists(select * from #MonthData where ParentID is null) begin
   declare @categoryID bigint;
   select top 1 @categoryID=CategoryID from #MonthData where ParentID is null order by CategoryID;

   declare @parentID bigint, @text varchar(4000);
   select top 1 @parentID=coalesce(ParentID,0), @text=Text from v6_dataCategories where CategoryID=@categoryID;

   if @parentID<>0 and not exists(select * from #MonthData where CategoryID=@parentID) begin
      insert into #MonthData(CategoryID,Value) values(@parentID, null)
   end

   update #MonthData set ParentID=@parentID, Text=@text where CategoryID=@categoryID
end

/* RESULT */
select * from #MonthData;

/* CLEAR */
drop table #EntriesData;
drop table #MonthData;
drop table #YearStdDev
drop table #AverageData
set nocount off;
