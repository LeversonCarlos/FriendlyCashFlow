set nocount on;
declare @resourceID varchar(128) = @paramResourceID;
declare @searchYear smallint = @paramSearchYear;
declare @searchMonth smallint = @paramSearchMonth;

declare @monthStart datetime = ltrim(str(@searchYear))+'-'+right('00'+ltrim(str(@searchMonth)),2)+'-'+'01';
declare @monthFinish datetime = dateadd(d,-1, dateadd(m, 1, @monthStart))
declare @yearStart datetime = dateadd(m, -11, @monthStart)
declare @yearFinish datetime = @monthFinish
print @yearStart
print @yearFinish


/* ENTRIES */
select
   cast(ltrim(str(YearDate))+'-'+right('00'+ltrim(str(MonthDate)),2)+'-'+'01' as datetime) as EntryDate, CategoryID, PatternID, EntryValue
into #Entries
from
(
   select
      year(SearchDate) as YearDate, month(SearchDate) as MonthDate, CategoryID, PatternID, sum(EntryValue) as EntryValue
   from v6_dataEntries
   where
      RowStatus = 1 and
      ResourceID = @resourceID and
      [Type] = 1 and
      TransferID is null and
      SearchDate >= @yearStart and
      SearchDate <= @yearFinish
      -- and CategoryID in (select CategoryID from #PatternBudget)
   group by
      year(SearchDate), month(SearchDate), CategoryID, PatternID
) sub


/* PATTERN BUDGET */
select
   Categories.ParentID As CategoryID, Categories.HierarchyText as CategoryText, Categories.CategoryID as SubCategoryID, 
   Patterns.PatternID, Patterns.Text as PatternText,
   Budget.[Value] as BudgetValue,
   cast(0 as decimal(15,2)) as MonthValue,
   cast(0 as decimal(15,2)) as YearValue
into #PatternBudget
from v6_dataPatterns as Patterns
   inner join v6_dataCategories as Categories on Categories.CategoryID = Patterns.CategoryID
   left join v6_dataBudget as Budget on (Budget.PatternID = Patterns.PatternID)
where
   Patterns.PatternID in ( select distinct PatternID from #Entries ) or
   Patterns.PatternID in ( select distinct PatternID from v6_dataBudget where RowStatus=1 and ResourceID=@resourceID )


/* VALUES */
update #PatternBudget
set
   YearValue =
   (
      select sum(EntryValue)
      from #Entries
      where
         PatternID = Categories.PatternID
         -- and EntryDate < @monthStart
   ),
   MonthValue =
   (
      select sum(EntryValue)
      from #Entries
      where
         PatternID = Categories.PatternID
         and EntryDate >= @monthStart
   )
from #PatternBudget as Categories;


/* CATEGORY BUDGET */
select
   CategoryID, CategoryText,
   -- PatternID, PatternText,
   BudgetValue,
   MonthValue,
   cast(0 as decimal(5,1)) as MonthPercentage,
   YearValue,
   cast(0 as decimal(5,1)) as YearPercentage
into #CategoryBudget
from
(
   select
      CategoryID, max(CategoryText) as CategoryText,
      sum(coalesce(BudgetValue,0)) as BudgetValue,
      sum(coalesce(MonthValue,0)) as MonthValue,
      sum(coalesce(YearValue,0)) as YearValue
   from #PatternBudget
   where
      CategoryID in ( select CategoryID from #PatternBudget where not BudgetValue is null)
   group by CategoryID
union
   select
      CategoryID, max(CategoryText) as CategoryText,
      cast(null as decimal(15,2)) as BudgetValue,
      sum(coalesce(MonthValue,0)) as MonthValue,
      sum(coalesce(YearValue,0)) as YearValue
   from #PatternBudget
   where
      CategoryID in ( select CategoryID from #PatternBudget where BudgetValue is null) and
      not CategoryID in ( select CategoryID from #PatternBudget where not BudgetValue is null)
   group by CategoryID
) sub



/* PERCENTAGE */
update #CategoryBudget
set
   MonthPercentage =
   (
      case when coalesce(BudgetValue,0) = 0
      then 0
      else coalesce(MonthValue,0) / coalesce(BudgetValue,0) * 100
      end
   ),
   YearPercentage =
   (
      case when coalesce(BudgetValue,0) = 0
      then 0
      else coalesce(YearValue,0) / (coalesce(BudgetValue,0)*12) * 100
      end
   )
from #CategoryBudget

/* RESULT */
select
   CategoryID, CategoryText,
   -- PatternID,PatternText,
   BudgetValue,
   MonthValue, MonthPercentage,
   YearValue, YearPercentage
from #CategoryBudget
order by
   BudgetValue desc, MonthValue desc, YearValue desc


/* CLEAR */
drop table #Entries;
drop table #PatternBudget;
drop table #CategoryBudget;
