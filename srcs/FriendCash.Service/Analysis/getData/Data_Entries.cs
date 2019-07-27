#region Using
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
#endregion

namespace FriendCash.Service.Analysis
{
   partial class AnalysyController
   {

      #region GetData_Entries

      private enum enCommandDateGroup : short { Day, Month, Year }

      private async Task<List<Model.viewEntry>> GetData_Entries(int year, short month, enCommandDateGroup dateGroup)
      {
         try
         {
            var commandText = this.GetData_Entries_Command(year, month, dateGroup);
            var objectResult = await this.DataContext.ObjectContext.ExecuteStoreQueryAsync<Model.viewEntry>(commandText);
            var listResult = objectResult.ToList();
            return listResult;

         }
         catch (Exception ex) { throw ex; }
      }

      private string GetData_Entries_Command(int year, short month, enCommandDateGroup dateGroup)
      {

         // USER
         var idUser = this.GetUserID();

         // DATE GROUP
         short dateSize;
         if (dateGroup == enCommandDateGroup.Year) { dateSize = 4; }
         else if (dateGroup == enCommandDateGroup.Month) { dateSize = 6; }
         else { dateSize = 8; }

         // INTERVAL
         var initialDate = new DateTime(year, month, 1);
         var finalDate = initialDate.AddMonths(1).AddDays(-1);

         // MONTHLY INTERVAL 
         if (dateGroup == enCommandDateGroup.Month)
         {
            finalDate = initialDate.AddMonths(2).AddDays(-1);
            initialDate = initialDate.AddMonths(-11);
         }

         // YEARLY INTERVAL 
         if (dateGroup == enCommandDateGroup.Year)
         {
            finalDate = initialDate.AddYears(1).AddDays(-1);
            initialDate = initialDate.AddYears(-10);
         }

         // INITIAL COMMAND
         var commandText = "" +
            "select " +
               "convert(varchar({0}), SearchDate, 112) As SearchDate, " +
               "idCategory, idPattern, " +
               "coalesce(idAccount,0) As idAccount, " +
               "convert(smallint,(case when convert(varchar(6), RowDate, 112) < convert(varchar(6), SearchDate, 112) then 1 else 0 end)) As Planned, " +
               "convert(smallint,Paid) As Paid, " +
               "convert(smallint,[Type]) As [Type], " +
               "Value " +
            "from v5_dataEntries " +
            "where " +
               "RowStatus=1 And idUser='{1}' And idTransfer is null " +
               "And SearchDate >= '{2}' " +
               "And SearchDate <= '{3}' " +
            "";

         // GROUPED COMMAND
         var commandGroup = "" +
            "select " +
               "SearchDate, " +
               "idCategory, idAccount, idPattern, " +
               "Planned, Paid, [Type], " +
               "sum(Value) As Value " +
            "from (" + string.Format(commandText, dateSize, idUser, initialDate.ToString("yyyy-MM-dd"), finalDate.ToString("yyyy-MM-dd")) + ") sub " +
            "group by " +
               "SearchDate, " +
               "idCategory, idAccount, idPattern, " +
               "Planned, Paid, [Type] " +
            "";

         return commandGroup;
      }

      #endregion

   }
}
