using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace FriendlyCashFlow.API.Import
{
   partial class ImportService
   {

      private async Task<ActionResult<bool>> ClearAsync(ImportVM value)
      {
         try
         {
            if (!value.ClearDataBefore) { return this.OkResponse(true); }

            var queryPath = "FriendlyCashFlow.ServerApi.Import.QUERY.Clear.sql";
            var queryContent = await Helpers.EmbededResource.GetResourceContent(queryPath);
            using (var queryReader = this.GetService<Helpers.DataReaderService>().GetDataReader(queryContent))
            {
               queryReader.AddParameter("@paramResourceID", value.ResourceID);
               if (!await queryReader.ExecuteReaderAsync()) { return this.WarningResponse("data query error"); }

               var queryResult = await queryReader.GetDataResultAsync<bool>();
               return queryResult?[0] ?? false;
            }

         }
         catch (Exception ex) { return this.ExceptionResponse(ex); }
      }

   }
}
