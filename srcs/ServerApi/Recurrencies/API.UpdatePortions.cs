using System;
using System.Threading.Tasks;

namespace FriendlyCashFlow.API.Recurrencies
{

   partial class RecurrenciesService
   {

      internal async Task<bool> UpdatePortionsAsync(long recurrencyID)
      {
         var startTime = DateTime.Now;
         try
         {
            var user = this.GetService<Helpers.User>();
            var queryPath = "FriendlyCashFlow.ServerApi.Recurrencies.QUERY.UpdatePortions.sql";
            var queryContent = await Helpers.EmbededResource.GetResourceContent(queryPath);
            using (var queryReader = this.GetService<Helpers.DataReaderService>().GetDataReader(queryContent))
            {
               queryReader.AddParameter("@paramResourceID", user.ResourceID);
               queryReader.AddParameter("@paramRecurrencyID", recurrencyID);
               if (!await queryReader.ExecuteReaderAsync()) { return false; }

               var queryResult = await queryReader.GetValueResultAsync<bool>();
               return queryResult?[0] ?? false;
            }
         }
         catch (Exception) { throw; }
         finally { this.TrackMetric("Update Portions of Recurrency", Math.Round(DateTime.Now.Subtract(startTime).TotalMilliseconds, 0)); }
      }

   }

}
