using System;
using System.ServiceModel;
using System.Linq;
using System.Collections.Generic;
using System.Data.Entity;
using FriendCash.Model.Tools;

namespace FriendCash.Service
{
   public class Transfer : Base
   {

      #region New
      internal Transfer(Model.Login oLogin) : base(oLogin) { }
      #endregion

      #region Constants
      public const string TAG_ENTITY = "Transfer";
      public const string TAG_ENTITY_LIST = TAG_ENTITY + "s";
      public const string TAG_ENTITY_KEY = "id" + TAG_ENTITY;
      #endregion  

      #region GetQuery
      private IQueryable<Model.Transfer> GetQuery(Parameters oParameters)
      {
         IQueryable<Model.Transfer> oQuery = null;

         // INCOME
         var oINCOME = from DATA in this.Context.Historys
                       where                      
                          DATA.RowStatusValue == ((short)FriendCash.Model.Base.enRowStatus.Active) &&
                          DATA.TypeValue == ((short)FriendCash.Model.Document.enType.Income) &&
                          DATA.idTransfer != null && DATA.idTransfer != 0
                       select DATA;

         // EXPENSE
         var oEXPENSE = from DATA in this.Context.Historys
                        where
                           DATA.RowStatusValue == ((short)FriendCash.Model.Base.enRowStatus.Active) &&
                           DATA.TypeValue == ((short)FriendCash.Model.Document.enType.Expense) &&
                           DATA.idTransfer != null && DATA.idTransfer != 0
                        select DATA;

         // HISTORY
         var oHISTORY = from IncomeDATA in oINCOME
                        join ExpenseDATA in oEXPENSE on IncomeDATA.idTransfer equals ExpenseDATA.idTransfer 
                        select new
                        {
                           idTransfer = IncomeDATA.idTransfer.Value,
                           DueDate = IncomeDATA.DueDate, Value = IncomeDATA.Value,
                           Settled = IncomeDATA.Settled, PayDate = IncomeDATA.PayDate,
                           idRowIncome = IncomeDATA.idRow, idAccountIncome = IncomeDATA.idAccount,
                           idRowExpense = ExpenseDATA.idRow, idAccountExpense = ExpenseDATA.idAccount,
                           idDocument = IncomeDATA.idDocument
                         };

         // SUB
         var oSUB = from DocumentDATA in this.Context.Documents
                    join HistoryDATA in oHISTORY on DocumentDATA.idDocument equals HistoryDATA.idDocument  
                    where 
                       DocumentDATA.RowStatusValue == ((short)FriendCash.Model.Base.enRowStatus.Active) &&
                       DocumentDATA.TypeValue == ((short)FriendCash.Model.Document.enType.Transfer) 
                    select new 
                    {
                       idTransfer = HistoryDATA.idTransfer,
                       DueDate = HistoryDATA.DueDate, Value = HistoryDATA.Value,
                       Settled = HistoryDATA.Settled, PayDate = HistoryDATA.PayDate,
                       idRowIncome = HistoryDATA.idRowIncome, idAccountIncome = HistoryDATA.idAccountIncome,
                       idRowExpense = HistoryDATA.idRowExpense, idAccountExpense = HistoryDATA.idAccountExpense,
                       CreatedBy = DocumentDATA.CreatedBy,
                       idDocument = DocumentDATA.idDocument 
                     };

         // QUERY
         oQuery = from DATA in oSUB
                  select new Model.Transfer()
                  {
                     idTransfer = DATA.idTransfer,
                     DueDate = DATA.DueDate, Value = DATA.Value,
                     Settled = DATA.Settled, PayDate = DATA.PayDate,
                     idRowIncome = DATA.idRowIncome, idAccountIncome = DATA.idAccountIncome,
                     idRowExpense = DATA.idRowExpense, idAccountExpense = DATA.idAccountExpense,
                     CreatedBy = DATA.CreatedBy,
                     idDocument = DATA.idDocument
                  };
         this.ApplyLogin(ref oQuery, oParameters.Login);

         // ID
         /*
         long idRow = oParameters.GetNumericData(TAG_ID);
         if (idRow != 0) { oQuery = oQuery.Where(DATA => DATA.idRow == idRow); }
         */ 

         // KEY
         long idKey = oParameters.GetNumericData(TAG_ENTITY_KEY);
         if (idKey != 0) { oQuery = oQuery.Where(DATA => DATA.idTransfer == idKey); }

         // SEARCH
         string sSearch = oParameters.GetTextData(TAG_SEARCH);
         if (!string.IsNullOrEmpty(sSearch))
         {
            /*
            oQuery = oQuery.Where(DATA =>
               DATA.AccountExpenseDetails.Description.Contains(sSearch) ||
               DATA.AccountIncomeDetails.Description.Contains(sSearch.ToLower())
               );
            */ 
          }

         // ORDER BY
         oQuery = oQuery.OrderByDescending(DATA => DATA.DueDate);

         // RELATED
         /*
         oQuery = ((System.Data.Entity.Core.Objects.ObjectQuery<Model.Transfer>)oQuery)
            .Include(DATA => DATA.AccountExpenseDetails);
         */ 

         return oQuery;
      }
      #endregion

      #region GetData

      #region GetData
      private Return GetData(Parameters oParameters, bool bApplyPagination)
      {
         var oReturn = new Return();

         try
         {

            // QUERY
            var oQuery = this.GetQuery(oParameters);

            // PAGINATION
            if (bApplyPagination == true)
            { this.ApplyPagination<Service.Transfer, Model.Transfer>(ref oQuery, oParameters, ref oReturn); }

            // EXECUTE
            var oList = oQuery.ToList<Model.Transfer>();

            // EAGERLOAD
            var oAction = new Action<Model.Transfer>
               (oEach => 
                  {
                     oEach.AccountExpenseDetails = this.Context.Accounts.Where(DATA => DATA.idRow == oEach.idAccountExpense).SingleOrDefault();
                     //this.Context.ObjectContext.LoadProperty<Model.Transfer>(oEach, p => p.AccountIncomeDetails);
                     oEach.AccountIncomeDetails = this.Context.Accounts.Where(DATA => DATA.idRow == oEach.idAccountIncome).SingleOrDefault();
                   }
                );
            oList.ForEach(oAction);

            // STORE 
            oReturn.DATA.Add(TAG_ENTITY_LIST, oList);
            oReturn.DATA.Add(TAG_SEARCH, oParameters.GetTextData(TAG_SEARCH));

            // OK
            oReturn.OK = true;

         }
         catch (Exception ex) { oReturn.MSG.Add(new Message() { Exception = ex.Message }); }

         return oReturn;
      }
      #endregion

      #endregion 

      #region Executes

      #region Index
      public static Return Index(Parameters oParameters)
      {
         Return oReturn = null;

         try
         {

            // INSTANCE
            using (var oService = new Service.Transfer(oParameters.Login))
            {

               // EXECUTE
               oReturn = oService.GetData(oParameters, true);

            }

         }
         catch (Exception ex) { oReturn.MSG.Add(new Message() { Exception = ex.Message }); }

         return oReturn;
      }
      #endregion

      #region Create
      public static Return Create(Parameters oParameters)
      {
         var oReturn = new Return();

         try
         {

            // EMPTY MODEL
            oReturn.DATA.Add(TAG_ENTITY, new Model.Transfer() { DueDate = DateTime.Now, Settled = true, PayDate = DateTime.Now });

            // OK
            oReturn.OK = true;

         }
         catch (Exception ex) { oReturn.MSG.Add(new Message() { Exception = ex.Message }); }

         return oReturn;
      }
      #endregion

      #region Edit
      public static Return Edit(Parameters oParameters)
      {
         Return oReturn = null;

         try
         {

            // INSTANCE
            using (var oService = new Service.Transfer(oParameters.Login))
            {

               // EXECUTE
               oReturn = oService.GetData(oParameters, false);

               // RESULT
               if (oReturn.OK == true && oReturn.DATA.ContainsKey(TAG_ENTITY_LIST) && oReturn.DATA[TAG_ENTITY_LIST] != null)
               {
                  var oList = ((List<Model.Transfer>)oReturn.DATA[TAG_ENTITY_LIST]);
                  if (oList.Count == 0) { oReturn.MSG.Add(new Message() { Warning = Resources.Transfer.MSG_WARNING_NOT_FOUND }); oReturn.OK = false; }
                  if (oList.Count >= 2) { oReturn.MSG.Add(new Message() { Warning = Resources.Transfer.MSG_WARNING_DUPLICITY }); oReturn.OK = false; }
                  else { oReturn.DATA.Add(TAG_ENTITY, oList.SingleOrDefault()); }
               }

            }

         }
         catch (Exception ex) { oReturn.MSG.Add(new Message() { Exception = ex.Message }); }

         return oReturn;
      }
      #endregion

      #region SaveEdit

      public static Return SaveEdit(Parameters oParameters)
      {
         var oReturn = new Return();

         try
         {

            // INSTANCE
            using (var oService = new Service.Transfer(oParameters.Login))
            {

               // VALIDATE
               if (oService.SaveEdit_Validate(oReturn, oParameters) == false) { return oReturn; }

               // APPLY
               if (oService.SaveEdit_Apply(oReturn, oParameters) == false) { return oReturn; }

               // OK
               oReturn.OK = true;

            }

         }
         catch (Exception ex) { oReturn.MSG.Add(new Message() { Exception = ex.Message }); }

         return oReturn;
      }

      private bool SaveEdit_Validate(Return oReturn, Parameters oParameters)
      {

         // DATA
         var oData = ((Model.Transfer)oParameters.DATA[TAG_ENTITY]);

         // DUE DATE
         if (oData.DueDate == null || oData.DueDate == DateTime.MinValue)
         { oReturn.MSG.Add(new Message() { Warning = Resources.Transfer.MSG_REQUIRED_DUE_DATE }); return false; }

         // VALUE
         if (oData.Value <= 0)
         { oReturn.MSG.Add(new Message() { Warning = Resources.Transfer.MSG_REQUIRED_VALUE }); return false; }

         // SETTLED
         if (oData.Settled == true)
         {
            if (oData.PayDate == null || oData.PayDate.HasValue == false || oData.PayDate.Value == null || oData.PayDate.Value == DateTime.MinValue)
            { oReturn.MSG.Add(new Message() { Warning = Resources.Transfer.MSG_REQUIRED_PAY_DATE }); return false; }
            if (oData.idAccountIncome == null || oData.idAccountIncome == 0)
            { oReturn.MSG.Add(new Message() { Warning = Resources.Transfer.MSG_REQUIRED_ACCOUNT_INCOME }); return false; }
            if (oData.idAccountExpense == null || oData.idAccountExpense == 0)
            { oReturn.MSG.Add(new Message() { Warning = Resources.Transfer.MSG_REQUIRED_ACCOUNT_EXPENSE }); return false; }
         }
         else if (oData.Settled == false)
         {
            oData.PayDate = new DateTime?();
            oData.idAccountIncome = new long?();
            oData.idAccountExpense = new long?();
         }

         // OK
         return true;
      }

      private bool SaveEdit_Apply(Return oReturn, Parameters oParameters)
      {
         bool bReturn = false;

         try
         {

            // PARAMETERS
            var oData = ((Model.Transfer)oParameters.DATA[TAG_ENTITY]);
            var oHistoryService = new History(oParameters.Login);
            long idTransfer = oHistoryService.GetDataMaxTransferID(); if (oData.idTransfer != 0) { idTransfer = oData.idTransfer; }
            long idDocument = oData.idDocument;

            // HISTORY
            if (this.SaveEdit_Apply(oData, Model.Document.enType.Income, oReturn, oParameters, oHistoryService, idTransfer, ref idDocument) == false) { return bReturn; }
            if (this.SaveEdit_Apply(oData, Model.Document.enType.Expense, oReturn, oParameters, oHistoryService, idTransfer, ref idDocument) == false) { return bReturn; }

            // SAVE
            if (this.Context.SaveChanges(oReturn.MSG) == false) { return bReturn; }

            // OK
            bReturn = true;

         }
         catch { throw; }

         return bReturn;
      }

      private bool SaveEdit_Apply(Model.Transfer oData, Model.Document.enType iType,
                                  Return oReturn, Parameters oParameters, History oHistoryService,
                                  long idTransfer, ref long idDocument)
      {
         var bReturn = false;

         try
         {

            // CREATE HISTORY
            var oHistory = new Model.History();
            oHistory.idTransfer = idTransfer;
            oHistory.DueDate = oData.DueDate;
            oHistory.Value = oData.Value;
            oHistory.Type = iType;

            // ORIGINAL ID
            long idRow = 0;
            if (iType == Model.Document.enType.Income) { idRow = oData.idRowIncome; }
            else if (iType == Model.Document.enType.Expense) { idRow = oData.idRowExpense; }
            oHistory.idHistory = idRow;
            oHistory.idRow = idRow;

            // DOCUMENT
            if (idDocument == 0) { idDocument = this.SaveEdit_Apply_Document(oReturn, oParameters); }
            oHistory.idDocument = idDocument;

            // SETTLED
            if (oData.Settled == true)
            {
               oHistory.Settled = oData.Settled;
               oHistory.PayDate = oData.PayDate;
               if (iType == Model.Document.enType.Income) { oHistory.idAccount = oData.idAccountIncome; }
               else if (iType == Model.Document.enType.Expense) { oHistory.idAccount = oData.idAccountExpense; }
            }

            // APPLY
            var oAfterSave = new Action<Model.History>(DATA => { if (DATA.idHistory == 0) { DATA.idHistory = DATA.idRow; } });
            if (this.ApplySave(oHistory, oParameters.Login, oReturn.MSG, oAfterSave) == false) { return bReturn; }

            // SORTING
            oHistory.SortingRefresh();
            if (this.Context.SaveChanges(oReturn.MSG) == false) { return bReturn; }

            // OK
            bReturn = true;

         }
         catch { throw; }

         return bReturn;
      }

      private long SaveEdit_Apply_Document(Return oReturn, Parameters oParameters)
      {

         // MODEL
         var oData = new Model.Document() { Description = "Transfer", Type = Model.Document.enType.Transfer };

         // APPLY
         var oAfterSave = new Action<Model.Document>(DATA => { if (DATA.idDocument == 0) { DATA.idDocument = DATA.idRow; } });
         if (this.ApplySave(oData, oParameters.Login, oReturn.MSG, oAfterSave) == false) { return 0; }

         return oData.idDocument;
       }

      #endregion

      #region SaveRemove

      public static Return SaveRemove(Parameters oParameters)
      {
         var oReturn = new Return();

         try
         {

            // INSTANCE
            using (var oService = new Service.Transfer(oParameters.Login))
            {

               // VALIDATE
               if (oService.SaveRemove_Validate(oReturn, oParameters) == false) { return oReturn; }

               // APPLY
               if (oService.SaveRemove_Apply(oReturn, oParameters) == false) { return oReturn; }

               // OK
               oReturn.OK = true;

            }

         }
         catch (Exception ex) { oReturn.MSG.Add(new Message() { Exception = ex.Message }); }

         return oReturn;
      }

      private bool SaveRemove_Validate(Return oReturn, Parameters oParameters)
      {
         bool bReturn = false;

         try
         {

            // CHECK DATA
            if (oParameters.DATA.ContainsKey(TAG_ENTITY) == false)
            {
               oReturn.MSG.Add(new Message() { Warning = Resources.Transfer.MSG_WARNING_NOT_FOUND });
               return bReturn;
            }

            // CHECK EXISTENCE
            var oDATA = ((Model.Transfer)oParameters.DATA[TAG_ENTITY]);
            if ((this.Context.Historys.Where(DATA => DATA.idRow == oDATA.idRowIncome).Count() == 0) ||
                (this.Context.Historys.Where(DATA => DATA.idRow == oDATA.idRowExpense).Count() == 0))
            {
               oReturn.MSG.Add(new Message() { Warning = Resources.Transfer.MSG_WARNING_NOT_FOUND });
               return bReturn;
            }

            // CHECK FOREIGN KEYS
            // TODO

            // OK
            bReturn = true;

         }
         catch { throw; }

         return bReturn;
      }

      private bool SaveRemove_Apply(Return oReturn, Parameters oParameters)
      {
         bool bReturn = false;

         try
         {

            // PARAMETER
            var oData = ((Model.Transfer)oParameters.DATA[TAG_ENTITY]);
            var oHistoryService = new Service.History(oParameters.Login);

            // INCOME
            var oIncome = oHistoryService.GetDataSingle_ByID(oParameters.Login, oData.idRowIncome);
            if (this.ApplyRemove(oIncome, oParameters.Login, oReturn.MSG) == false) { return bReturn; }

            // EXPENSE
            var oExpense = oHistoryService.GetDataSingle_ByID(oParameters.Login, oData.idRowExpense);
            if (this.ApplyRemove(oExpense, oParameters.Login, oReturn.MSG) == false) { return bReturn; }

            // RETURN 
            oReturn.DATA.Add(TAG_ENTITY, oData);

            // OK
            bReturn = true;

         }
         catch { throw; }

         return bReturn;
      }

      #endregion

      #endregion

   }
}
