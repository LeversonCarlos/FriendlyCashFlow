using System;
using System.ServiceModel;
using System.Linq;
using System.Collections.Generic;

namespace FriendCash.Service
{

   #region iTransfer
   [ServiceContract]
   public interface iTransfer
   {

      [OperationContract]
      Return GetData(Parameters oParameters);

      [OperationContract]
      Return Update(Parameters oParameters);

   }
   #endregion

   #region Transfer
   public class Transfer : Base, iTransfer
   {

      #region New
      public Transfer()
      {
         this.GetEntityName += delegate(ref string Value) { Value = "Transfer"; };
       }
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

         // LOGIN
         if (oParameters.Login != null && !string.IsNullOrEmpty(oParameters.Login.Code))
         {
            var oLogin = from LoginsExt in this.Context.Logins
                         join LoginsInt in this.Context.Logins on LoginsExt.idUser equals LoginsInt.idUser
                         where LoginsInt.Code == oParameters.Login.Code
                         select LoginsExt.idRow;
            oQuery = oQuery.Where(DATA => oLogin.Contains(DATA.CreatedBy.Value ));
         }

         // ID
         /*
         long idRow = oParameters.GetNumericData(this.Fields.ID);
         if (idRow != 0)
          { oQuery = oQuery.Where(DATA => DATA.idRow == idRow); }
         */ 

         // KEY
         long Key = oParameters.GetNumericData(this.Fields.Key);
         if (Key != 0)
          { oQuery = oQuery.Where(DATA => DATA.idTransfer == Key); }

         // SEARCH
         string sSearch = oParameters.GetTextData(this.Fields.Search);
         if (!string.IsNullOrEmpty(sSearch))
         {
            /*
            oQuery = oQuery.Where(DATA =>
               DATA.Description.ToLower().Contains(sSearch.ToLower()) ||
               DATA.SupplierDetails.Code.ToLower().Contains(sSearch.ToLower()) ||
               DATA.SupplierDetails.Description.ToLower().Contains(sSearch.ToLower())
               );
               */
          }

         // ORDER BY
         oQuery = oQuery.OrderByDescending(DATA => DATA.DueDate);

         return oQuery;
      }
      #endregion


      #region GetData

      #region GetData
      public Return GetData(Parameters oParameters)
      {
         Return oReturn = new Return();

         try
         {

            // QUERY
            IQueryable<Model.Transfer> oQuery = this.GetQuery(oParameters);

            // PAGINATION
            this.ApplyPagination<Service.Transfer, Model.Transfer>(ref oQuery, oParameters, ref oReturn);

            // EXECUTE
            List<Model.Transfer> oDATA = oQuery.ToList<Model.Transfer>();
            oReturn.DATA.Add(this.Fields.List, oDATA);

            // OK
            oReturn.OK = true;

         }
         catch (Exception ex) { oReturn.MSG.Add(ex.Message); }

         return oReturn;
      }
      #endregion

      #endregion 

      #region Update

      public Return Update(Parameters oParameters)
      {
         Return oReturn = new Return();
         History oHistoryService = new History();

         try
         {

            // TRANSACTION
            //this.Context.Database.Connection.BeginTransaction();

            // PARAMETERS
            Model.Login oLogin = oParameters.Login;
            Model.Transfer oValue = ((Model.Transfer)oParameters.DATA[this.Fields.Entity]);

            // VALIDATE
            if (this.Update_Validate(ref oReturn, ref oValue) == false) { return oReturn; }

            // HISTORY
            long idTransfer = oHistoryService.GetMaxTransfer();
            long idHistory = oHistoryService.GetMaxHistory();
            long idDocument = 0;
            if (this.Update_Apply(oValue, Model.Document.enType.Income, ref oReturn, oLogin, oHistoryService, idTransfer, ref idHistory, ref idDocument) == false) { return oReturn; }
            if (this.Update_Apply(oValue, Model.Document.enType.Expense, ref oReturn, oLogin, oHistoryService, idTransfer, ref idHistory, ref idDocument) == false) { return oReturn; }
            
            // SAVE
            if (this.Context.SaveChanges(oReturn.MSG) == true)
             { oReturn.OK = true; }

         }
         catch (Exception ex) { oReturn.MSG.Add(ex.Message); }

         return oReturn;
      }

      private bool Update_Validate(ref Return oReturn, ref Model.Transfer oValue)
      {

         if (oValue.DueDate == null || oValue.DueDate == DateTime.MinValue)
         { oReturn.MSG.Add("Invalid Due Date"); return false; }

         if (oValue.Value == null || oValue.Value <= 0)
         { oReturn.MSG.Add("Invalid Value"); return false; }

         if (oValue.Settled == true)
         {
            if (oValue.PayDate == null || oValue.PayDate.HasValue == false || oValue.PayDate.Value == null || oValue.PayDate.Value == DateTime.MinValue)
             { oReturn.MSG.Add("Invalid Pay Date"); return false; }
            if (oValue.idAccountIncome == null || oValue.idAccountIncome == 0)
             { oReturn.MSG.Add("Invalid Account"); return false; }
            if (oValue.idAccountExpense == null || oValue.idAccountExpense == 0)
             { oReturn.MSG.Add("Invalid Account"); return false; }
         }

         if (oValue.Settled == false)
         {
            oValue.PayDate = new DateTime?();
            oValue.idAccountIncome = new long?();
            oValue.idAccountExpense= new long?();
          }

         return true;
      }

      private bool Update_Apply(Model.Transfer oValue, Model.Document.enType iType,
                                ref Return oReturn, Model.Login oLogin, History oHistoryService,
                                long idTransfer, ref long idHistory, ref long idDocument)
      {

         // ORIGINAL ID
         long idOriginal = 0;
         if (iType == Model.Document.enType.Income) { idOriginal = oValue.idRowIncome; }
         else if (iType == Model.Document.enType.Expense) { idOriginal = oValue.idRowExpense; }

         // CREATE ROW
         Model.History oNew = new Model.History();

         // BASIC DATA
         oNew.idTransfer = idTransfer;
         oNew.idDocument = idDocument;
         oNew.DueDate = oValue.DueDate;
         oNew.Value = oValue.Value;
         oNew.Type = iType;
         oNew.RowStatus = Model.Base.enRowStatus.Active;

         // ORIGINAL ROW
         if (idOriginal > 0)
         {
            Model.History oOld = oHistoryService.GetData_ByID(idOriginal);
            this.Context.Historys.Attach(oOld);
            this.ApplyDataToOld(oOld, oLogin);
            oNew.idHistory = oOld.idHistory;
            oNew.idTransfer = oOld.idTransfer;
            oNew.idDocument = oOld.idDocument;
            oNew.idOriginal = idOriginal;
          }

         // DOCUMENT
         if (idDocument == 0)
         {
            if (oNew.idDocument == 0)
            {
               oNew.idDocument = Document.AddModel(oLogin, oReturn.MSG, 
                  new Model.Document() { Description = "Transfer", Type = Model.Document.enType.Transfer });
             }
            idDocument = oNew.idDocument;
          }

         // SETTLED
         if (oValue.Settled == true)
         {
            oNew.Settled = oValue.Settled;
            oNew.PayDate = oValue.PayDate;
            if (iType == Model.Document.enType.Income) { oNew.idAccount = oValue.idAccountIncome; }
            else if (iType == Model.Document.enType.Expense) { oNew.idAccount = oValue.idAccountExpense; }
          }
         
         // ID
         if (oNew.idHistory == 0)
          { oNew.idHistory = idHistory; idHistory += 1; }
         oNew.SortingRefresh();

         // ADD
         this.ApplyDataToNew(oNew, oLogin, idOriginal);
         this.Context.Historys.Add(oNew);

         return true;
      }

      #endregion


    }
   #endregion

}
