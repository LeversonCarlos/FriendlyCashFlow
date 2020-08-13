using System;
using System.Linq;
using System.Threading.Tasks;

namespace FriendlyCashFlow.API.Balances
{

   partial class BalancesService
   {

      internal async Task FixAsync(Entries.EntryData value)
      {
         var startTime = DateTime.Now;
         try
         {
            if (!value.AccountID.HasValue || value.AccountID == 0)
               return;

            var queryPath = "FriendlyCashFlow.ServerApi.Balances.QUERY.Fix.sql";
            var queryContent = await Helpers.EmbededResource.GetResourceContent(queryPath);
            using (var queryReader = this.GetService<Helpers.DataReaderService>().GetDataReader(queryContent))
            {
               queryReader.AddParameter("@paramResourceID", this.GetService<Helpers.User>().ResourceID);
               queryReader.AddParameter("@paramAccountID", value.AccountID);
               queryReader.AddParameter("@paramSearchYear", value.SearchDate.Year);
               queryReader.AddParameter("@paramSearchMonth", value.SearchDate.Month);

               if (!await queryReader.ExecuteReaderAsync())
                  return;

               var keyValues = await queryReader.GetValueResultAsync<string>();
               if (keyValues.All(x => string.IsNullOrEmpty(x)))
                  return;

               keyValues.Add($"AccountID:{value.AccountID}");
               keyValues.Add($"SearchDate:{value.SearchDate.ToString("yyyy-MM")}");
               keyValues.Add($"DueDate:{value.DueDate.ToString("yyyy-MM")}");
               keyValues.Add($"EntryValue:{value.EntryValue}");
               keyValues.Add($"Paid:{value.Paid}");
               keyValues.Add($"Type:{value.Type}");
               this.TrackEvent("Fixed Balance", keyValues.ToArray());

            }

         }
         catch (Exception) { throw; }
      }

   }

}
