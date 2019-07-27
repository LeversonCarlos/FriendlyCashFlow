using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FriendCash.Model.Tools;

namespace FriendCash.Service
{
   public class Flow : Base
   {

      #region New
      internal Flow(Model.Login oLogin) : base(oLogin) { }
      #endregion

      #region Constants
      public const string TAG_ENTITY = "Flow";
      public const string TAG_ENTITY_LIST = TAG_ENTITY + "s";
      public const string TAG_ENTITY_KEY = "id" + TAG_ENTITY;
      #endregion

      #region GetQuery

      #region GetQueryInner
      private IQueryable<Model.History> GetQueryInner(Parameters oParameters)
      {
         var oHistory = from DATA in this.Context.Historys
                        where DATA.RowStatusValue == ((short)Model.Base.enRowStatus.Active)
                        select DATA;

         return oHistory;
       }
      #endregion

      #region GetQueryHistory
      private IQueryable<Model.History> GetQueryHistory(Parameters oParameters)
      {

         // INITIAL
         var oQuery = this.GetQueryInner(oParameters);
         this.ApplyLogin(ref oQuery, oParameters.Login);

         // ACCOUNT
         long idAccount = oParameters.GetNumericData("idAccount");
         if (idAccount != 0) { oQuery = oQuery.Where(DATA => DATA.idAccount == idAccount); }

         // START DATE
         DateTime? oStartDate = oParameters.GetDateData("StartDate");
         if (oStartDate != null && oStartDate.HasValue == true) { oQuery = oQuery.Where(DATA => (DATA.Settled == true && DATA.PayDate.Value >= oStartDate.Value) || (DATA.Settled == false && DATA.DueDate >= oStartDate.Value)); }

         // FINISH DATE
         DateTime? oFinishDate = oParameters.GetDateData("FinishDate");
         if (oFinishDate != null && oFinishDate.HasValue == true) { oQuery = oQuery.Where(DATA => (DATA.Settled == true && DATA.PayDate.Value <= oFinishDate.Value) || (DATA.Settled == false && DATA.DueDate <= oFinishDate.Value)); }

         // JUST NOT SETTLED
         if (oParameters.DATA.ContainsKey("JustNotSettled") && ((bool)oParameters.DATA["JustNotSettled"]) == true)
         { oQuery = oQuery.Where(DATA => DATA.Settled == false); }

         // ORDER BY
         oQuery = oQuery.OrderByDescending(DATA => DATA.Sorting);

         return oQuery;
       }
      #endregion

      #endregion

      #region GetData

      #region GetDataBalance
      public static Return GetDataBalance(Parameters oParameters)
      {
         var oReturn = new Return();

         try
         {

            // INSTANCE
            using (var oService = new Service.Flow(oParameters.Login))
            {

               // HISTORYs
               var oHistory = oService.GetQueryHistory(oParameters);
               var oInner = oService.GetQueryInner(oParameters);

               // PAGINATION
               if (!oParameters.DATA.ContainsKey("JustFirst") || ((bool)oParameters.DATA["JustFirst"]) == false)
               { oService.ApplyPagination<Service.History, Model.History>(ref oHistory, oParameters, ref oReturn); }
               else
               { oHistory = oHistory.Take(1); }

               // FLOW
               var oFlow = from History in oHistory
                           select new Model.Flow()
                           {
                              idHistory = History.idHistory,
                              idDocument = History.idDocument,
                              idTransfer = History.idTransfer,
                              idAccount = History.idAccount,
                              InnerDescription = (from Documents in oService.Context.Documents
                                                  where Documents.idDocument == History.idDocument &&
                                                        Documents.RowStatusValue == ((short)Model.Base.enRowStatus.Active)
                                                  select Documents.Description
                                                  ).FirstOrDefault(),
                              Settled = History.Settled,
                              DueDate = History.DueDate,
                              PayDate = History.PayDate,
                              TypeValue = History.TypeValue,
                              InnerValue = History.Value,
                              BalanceProvided = (from Inner in oInner
                                                 where Inner.idAccount == History.idAccount &&
                                                       Inner.Sorting <= History.Sorting
                                                 select new
                                                   {
                                                      Value = Inner.Value * (Inner.TypeValue == ((short)Model.Document.enType.Income) ? 1 : -1)
                                                   }
                                                 ).Sum(x => x.Value),
                              BalanceActual = (from Inner in oInner
                                               where Inner.idAccount == History.idAccount &&
                                                     Inner.Sorting <= History.Sorting &&
                                                     Inner.Settled == true
                                               select new
                                                 {
                                                    Value = Inner.Value * (Inner.TypeValue == ((short)Model.Document.enType.Income) ? 1 : -1)
                                                 }
                                                 ).Sum(x => x.Value),
                              Sorting = History.Sorting
                           };

               // EXECUTE
               oReturn.DATA.Add(TAG_ENTITY_LIST, oFlow.ToList<Model.Flow>());

               // OK
               oReturn.OK = true;

            }

         }
         catch (Exception ex) { oReturn.MSG.Add(new Message() { Exception = ex.Message }); }

         return oReturn;
      }
      #endregion

      #region GetDataParcels
      public static Return GetDataParcels(Parameters oParameters)
      {
         var oReturn = new Return();

         try
         {

            // INSTANCE
            using (var oService = new Service.Flow(oParameters.Login))
            {

               // HISTORYs
               var oHistory = oService.GetQueryHistory(oParameters);
               var oInner = oService.GetQueryInner(oParameters);

               // FLOW
               DateTime oLimit = DateTime.Now.AddDays(7);
               var oFlow = from History in oHistory
                           where History.Settled == false &&
                                 History.DueDate <= oLimit
                           orderby History.DueDate, History.Value
                           select new Model.Flow()
                           {
                              idHistory = History.idHistory,
                              idDocument = History.idDocument,
                              idTransfer = History.idTransfer,
                              idAccount = History.idAccount,
                              InnerDescription = (from Documents in oService.Context.Documents
                                                  where Documents.idDocument == History.idDocument &&
                                                        Documents.RowStatusValue == ((short)Model.Base.enRowStatus.Active)
                                                  select Documents.Description
                                                  ).FirstOrDefault(),
                              Settled = History.Settled,
                              DueDate = History.DueDate,
                              PayDate = History.PayDate,
                              TypeValue = History.TypeValue,
                              InnerValue = History.Value,
                              ParcelQuantity = (from Inner in oInner
                                                where Inner.idDocument == History.idDocument
                                                select new { ID = Inner.idRow }
                                                ).Count(),
                              ParcelItem = (from Inner in oInner
                                            where Inner.idDocument == History.idDocument &&
                                                  Inner.Sorting <= History.Sorting
                                            select new { ID = Inner.idRow }
                                                ).Count(),
                              Sorting = History.Sorting
                           };

               // EXECUTE
               oReturn.DATA.Add(TAG_ENTITY_LIST, oFlow.ToList<Model.Flow>());

               // OK
               oReturn.OK = true;

            }

         }
         catch (Exception ex) { oReturn.MSG.Add(new Message() { Exception = ex.Message }); }

         return oReturn;
      }
      #endregion

      #region GetMonthlyFlow
      public static Return GetMonthlyFlow(Parameters oParameters)
      {
         var oReturn = new Return();

         try
         {

            // INSTANCE
            using (var oService = new Service.Flow(oParameters.Login))
            {

               // HISTORYs
               var oHistory = oService.GetQueryHistory(oParameters);

               // FLOW
               var oFlow =
                  (from H in oHistory
                   where H.idTransfer == null
                   group H by new
                   {
                      TypeValue = H.TypeValue,
                      DateYear = (H.Settled ? H.PayDate.Value.Year : H.DueDate.Year),
                      DateMonth = (H.Settled ? H.PayDate.Value.Month : H.DueDate.Month)
                   } into G
                   orderby G.Key.TypeValue, G.Key.DateYear, G.Key.DateMonth
                   select new 
                   {
                      Type = (Model.Document.enType)G.Key.TypeValue,
                      DateYear = G.Key.DateYear,
                      DateMonth = G.Key.DateMonth,
                      Value = G.Sum(x => x.Value)
                   }).ToList();

               var oFlowMissingIncome =
                  (from E in oFlow.Where(e => e.Type == Model.Document.enType.Expense)
                   join fromI in oFlow.Where(i => i.Type == Model.Document.enType.Income) on new { E.DateYear, E.DateMonth } equals new { fromI.DateYear, fromI.DateMonth } into joinI from I in joinI.DefaultIfEmpty()
                   where I == null
                   select new {
                      Type = Model.Document.enType.Income, 
                      DateYear = E.DateYear,
                      DateMonth = E.DateMonth,
                      Value = (double)0
                   }).ToList();
               oFlow.AddRange(oFlowMissingIncome);
               

               // MONTHLY FLOW
               var oMonthlyFlow =
                  (from I in oFlow
                   join E in oFlow on new { I.DateYear, I.DateMonth } equals new { E.DateYear, E.DateMonth }
                   where I.Type == Model.Document.enType.Income &&
                         E.Type == Model.Document.enType.Expense
                   select new Model.ViewMonthlyFlow()
                   {
                      Date = new DateTime(I.DateYear, I.DateMonth, 1),
                      IncomeValue = I.Value,
                      ExpenseValue = E.Value
                   }).ToList();

               // INITIAL BALANCE
               var oInitialBalanceQuery = oService.GetQueryInner(oParameters);
               oService.ApplyLogin(ref oInitialBalanceQuery, oParameters.Login);
               var oInitialBalanceDate = oMonthlyFlow.Min(x => x.Date);
               var oInitialBalanceValue =
                  (from DATA in oInitialBalanceQuery
                   where DATA.idTransfer == null &&
                         (
                           (DATA.Settled == true && DATA.PayDate < oInitialBalanceDate) ||
                           (DATA.Settled == false && DATA.DueDate < oInitialBalanceDate)
                          )
                   select new
                   {
                      Value = DATA.Value * (DATA.TypeValue == ((short)Model.Document.enType.Income) ? 1 : -1)
                   }).ToList().Sum(x => x.Value);


               // APPLY BALANCE
               foreach(var oData in oMonthlyFlow)
               {
                  oInitialBalanceValue += (oData.IncomeValue - oData.ExpenseValue);
                  oData.BalanceValue = oInitialBalanceValue;
               }

               // EXECUTE
               oReturn.DATA.Add(TAG_ENTITY_LIST, oMonthlyFlow);

               // OK
               oReturn.OK = true;

            }

         }
         catch (Exception ex) { oReturn.MSG.Add(new Message() { Exception = ex.Message }); }

         return oReturn;
      }
      #endregion

      #endregion

   }
}
