using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FriendlyCashFlow.API.Analytics
{

   partial class AnalyticsService
   {

      internal async Task<ActionResult<MonthlyTargetVM>> GetMonthlyTargetAsync(short searchYear, short searchMonth)
      {
         try
         {
            var monthlyTarget = await this.GetMonthlyTargetAsync_Execute(searchYear, searchMonth);
            if (monthlyTarget == null) { return this.WarningResponse("data query error"); }
            return monthlyTarget;
         }
         catch (Exception ex) { return this.ExceptionResponse(ex); }
      }

      private async Task<MonthlyTargetVM> GetMonthlyTargetAsync_Execute(short searchYear, short searchMonth)
      {
         try
         {
            var queryPath = "FriendlyCashFlow.ServerApi.Analytics.QUERY.MonthlyTarget.sql";
            var queryContent = await Helpers.EmbededResource.GetResourceContent(queryPath);
            using (var queryReader = this.GetService<Helpers.DataReaderService>().GetDataReader(queryContent))
            {
               queryReader.AddParameter("@paramResourceID", this.GetService<Helpers.User>().ResourceID);
               queryReader.AddParameter("@paramSearchYear", searchYear);
               queryReader.AddParameter("@paramSearchMonth", searchMonth);

               if (!await queryReader.ExecuteReaderAsync())
                  return null;
               var headersList = await queryReader.GetDataResultAsync<MonthlyTargetHeaderVM>();
               var itemsList = await queryReader.GetDataResultAsync<MonthlyTargetItemVM>();

               var incomeSeriesList = itemsList.Where(x => x.Type == (short)Categories.enCategoryType.Income).Select(x => x.SerieText).Distinct().ToArray();
               foreach (var header in headersList)
               {
                  var existingSeriesOnCurrentMonth = itemsList
                     .Where(x => x.Type == (short)Categories.enCategoryType.Income && x.Date == header.Date)
                     .Select(x => x.SerieText)
                     .Distinct()
                     .ToArray();
                  var missingSeriesListOnCurrentMonth = incomeSeriesList
                     .Where(serie => !existingSeriesOnCurrentMonth.Contains(serie))
                     .Select(serie => new MonthlyTargetItemVM
                     {
                        Date = header.Date,
                        Type = (short)Categories.enCategoryType.Income,
                        SerieText = serie,
                        Value = 0
                     })
                     .ToArray();
                     itemsList.AddRange(missingSeriesListOnCurrentMonth);
               }

               var expenseText = GetTranslation("ANALYTICS_MONTHLY_TARGET_EXPENSE_LABEL");
               foreach (var item in itemsList)
               {
                  if (item.Type == (short)Categories.enCategoryType.Expense)
                     item.SerieText = expenseText;
               }

               return MonthlyTargetVM.Create(headersList.ToArray(), itemsList.ToArray());
            }
         }
         catch (Exception) { throw; }
      }

   }

   partial class AnalyticsController
   {

      [HttpGet("monthlyTarget/{searchYear}/{searchMonth}")]
      public async Task<ActionResult<MonthlyTargetVM>> GetMonthlyTargetAsync(short searchYear, short searchMonth)
      {
         return await this.GetService<AnalyticsService>().GetMonthlyTargetAsync(searchYear, searchMonth);
      }

   }

   public class MonthlyTargetVM
   {
      public MonthlyTargetHeaderVM[] Headers { get; private set; }
      public MonthlyTargetItemVM[] Items { get; private set; }
      internal static MonthlyTargetVM Create(MonthlyTargetHeaderVM[] headers, MonthlyTargetItemVM[] items) =>
         new MonthlyTargetVM
         {
            Headers = headers,
            Items = items
         };
   }

   public class MonthlyTargetHeaderVM
   {
      public DateTime Date { get; set; }
      public string DateText { get { return this.Date.ToString("MMM").ToUpper(); } }
      public decimal BalanceValue { get; set; }
      public decimal TargetValue { get; set; }
   }

   public class MonthlyTargetItemVM
   {
      public DateTime Date { get; set; }
      public string DateText { get { return this.Date.ToString("MMM").ToUpper(); } }
      public short Type { get; set; }
      public string SerieText { get; set; }
      public decimal Value { get; set; }
   }

}
