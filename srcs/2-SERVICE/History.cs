using System;
using System.ServiceModel;
using System.Linq;
using System.Collections.Generic;

namespace FriendCash.Service
{

   #region iHistory
   [ServiceContract]
   public interface iHistory
   {

      [OperationContract]
      Return GetData(Parameters oParameters);

      [OperationContract]
      Return Update(Parameters oParameters);

   }
   #endregion

   #region History
   public class History : Base, iHistory
   {

      #region New
      public History()
      {
         this.GetEntityName += delegate(ref string Value) { Value = "History"; };
       }
      #endregion

      #region GetMaxID

      internal long GetMaxHistory()
      {
         long iReturn = 0;

         try
         {

            var oExec = (from DATA in this.Context.Historys
                         select DATA.idHistory);
            if (oExec != null && oExec.Count<long>() > 0)
            {
               iReturn = oExec.Max<long>();
            }
            iReturn += 1;

         }
         catch (Exception ex) { throw ex; }

         return iReturn;
      }

      internal long GetMaxTransfer()
      {
         long iReturn = 0;

         try
         {

            var oExec = (from DATA in this.Context.Historys
                         where DATA.idTransfer.HasValue == true
                         select DATA.idTransfer.Value);
            if (oExec != null && oExec.Count<long>() > 0)
            {
               iReturn = oExec.Max<long>();
             }
            iReturn += 1;

         }
         catch (Exception ex) { throw ex; }

         return iReturn;
      }

      #endregion

      #region GetQuery
      private IQueryable<Model.History> GetQuery(Parameters oParameters)
      {
         IQueryable<Model.History> oQuery = null;

         // QUERY
         oQuery = from DATA in this.Context.Historys
                  where DATA.RowStatusValue == ((short)Model.Base.enRowStatus.Active)
                  select DATA;

         // LOGIN
         if (oParameters.Login != null && !string.IsNullOrEmpty(oParameters.Login.Code))
         {
            var oLogin = from LoginsExt in this.Context.Logins
                         join LoginsInt in this.Context.Logins on LoginsExt.idUser equals LoginsInt.idUser
                         where LoginsInt.Code == oParameters.Login.Code
                         select LoginsExt.idRow;
            oQuery = oQuery.Where(DATA => oLogin.Contains(DATA.CreatedBy.Value));
         }

         // ID
         long idRow = oParameters.GetNumericData(this.Fields.ID);
         if (idRow != 0)
          { oQuery = oQuery.Where(DATA => DATA.idRow == idRow); }

         // KEY
         long Key = oParameters.GetNumericData(this.Fields.Key);
         if (Key != 0)
          { oQuery = oQuery.Where(DATA => DATA.idHistory == Key); }

         // DOCUMENT
         if (oParameters.DATA.ContainsKey("idDocument") && oParameters.DATA["idDocument"] != null)
         {
            long idDocument = ((long)oParameters.DATA["idDocument"]);
            if (idDocument != 0)
             { oQuery = oQuery.Where(DATA => DATA.idDocument == ((long)idDocument)); }
          }

         // SEARCH
         string sSearch = oParameters.GetTextData(this.Fields.Search);
         if (!string.IsNullOrEmpty(sSearch))
         {
            if (sSearch.Substring(0, 3) == "S0!" || sSearch.Substring(0, 3) == "S1!")
            {
               bool bSettled = false; if (sSearch.Substring(0, 3) == "S1!") { bSettled = true; }

               // TODO: APPLY SETTLED FILTER

               if (sSearch.Length == 3) { sSearch = string.Empty; }
               else { sSearch = sSearch.Substring(3); }
            }
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
            IQueryable<Model.History> oQuery = this.GetQuery(oParameters);

            // PAGINATION
            this.ApplyPagination<Service.History, Model.History>(ref oQuery, oParameters, ref oReturn);

            // EXECUTE
            List<Model.History> oDATA = oQuery.ToList<Model.History>();
            oReturn.DATA.Add(this.Fields.List, oDATA);

            // BRING DOCUMENT
            this.GetData_BringDocument(ref oReturn, oParameters);

            // OK
            oReturn.OK = true;

         }
         catch (Exception ex) { oReturn.MSG.Add(ex.Message); }

         return oReturn;
      }

      private void GetData_BringDocument(ref Return oReturn, Parameters oParameters)
      {
         try
         {

            // CHECK CONFIG
            if (!oParameters.DATA.ContainsKey("BringDocument") || oParameters.DATA["BringDocument"] == null || ((bool)oParameters.DATA["BringDocument"]) == false)
             { return; }

            // ID DOCUMENT
            long idDocument = 0;
            if (oParameters.DATA.ContainsKey("idDocument") && oParameters.DATA["idDocument"] != null)
             { idDocument = ((long)oParameters.DATA["idDocument"]); }
            if (idDocument == 0 && oReturn.DATA.ContainsKey(this.Fields.List) && oReturn.DATA[this.Fields.List] != null)
            {
               Model.History oHistory = ((List<Model.History>)oReturn.DATA[this.Fields.List]).SingleOrDefault();
               if (oHistory != null) { idDocument = oHistory.idDocument; }
             }
            if (idDocument == 0) { return; }

            Document oDocumentService = new Document();
            Model.Document oDocumentModel = oDocumentService.GetData_ByKey(oParameters.Login, idDocument);
            if (oDocumentModel != null) { oReturn.DATA.Add(oDocumentService.Fields.List, oDocumentModel); }
            oDocumentService.Dispose();
            oDocumentService = null;

         }
         catch (Exception ex) { throw ex; }
      }

      #endregion

      #region GetData_By

      internal Model.History GetData_ByKey(long idDocument)
      { return this.GetData_By(0, idDocument); }

      internal Model.History GetData_ByID(long idRow)
      { return this.GetData_By(idRow, 0); }

      private Model.History GetData_By(long idRow, long idDocument)
      {
         Model.History oReturn = null;

         try
         {

            // PARAMETERS
            Service.Parameters oParameters = new Parameters();
            oParameters.DATA.Add(this.Fields.ID, idRow);
            oParameters.DATA.Add(this.Fields.Key, idDocument);

            // EXECUTE
            Return oExecReturn = this.GetData(oParameters);

            // RESULT
            if (oExecReturn.OK == true && oExecReturn.DATA.ContainsKey(this.Fields.List) && oExecReturn.DATA[this.Fields.List] != null)
            {
               List<Model.History> oList = ((List<Model.History>)oExecReturn.DATA[this.Fields.List]);
               oReturn = oList.SingleOrDefault();
            }

         }
         catch (Exception ex) { throw ex; }

         return oReturn;
      }

      #endregion

      #endregion 

      #region Update

      public Return Update(Parameters oParameters)
      {
         Return oReturn = new Return();

         try
         {

            // TRANSACTION
            //this.Context.Database.Connection.BeginTransaction();

            // PARAMETERS
            Model.Login oLogin = oParameters.Login;
            Model.History oValue = ((Model.History)oParameters.DATA[this.Fields.Entity]);

            // VALIDATE
            if (this.Update_Validate(ref oReturn, ref oValue) == false) { return oReturn; }

            // ORIGINAL ID
            long idOriginal = this.GetOriginalID(oValue);

            // NEW ROW
            Model.History oNew = oValue;
            if (oNew.idHistory == 0) { oNew.idHistory = this.GetMaxHistory(); }
            oNew.SortingRefresh();
            this.ApplyDataToNew(oNew, oLogin, idOriginal);
            this.Context.Historys.Add(oNew);

            // ORIGINAL ROW
            if (idOriginal > 0)
            {
               Model.History oOld = this.GetData_ByID(idOriginal);
               this.Context.Historys.Attach(oOld);
               this.ApplyDataToOld(oOld, oLogin);
             }

            // SAVE
            if (this.Context.SaveChanges(oReturn.MSG) == true)
             { oReturn.OK = true; }

         }
         catch (Exception ex) { oReturn.MSG.Add(ex.Message); }

         return oReturn;
      }

      private bool Update_Validate(ref Return oReturn, ref Model.History oValue)
      {

         if (oValue.idDocument == null || oValue.idDocument == 0)
          { oReturn.MSG.Add("Invalid Document"); return false; }

         if (oValue.DueDate == null || oValue.DueDate == DateTime.MinValue)
          { oReturn.MSG.Add("Invalid Due Date"); return false; }

         if (oValue.Value == null || oValue.Value <= 0)
          { oReturn.MSG.Add("Invalid Value"); return false; }

         if (oValue.Settled == true)
         {
            if (oValue.PayDate == null || oValue.PayDate.HasValue == false || oValue.PayDate.Value == null || oValue.PayDate.Value == DateTime.MinValue)
             { oReturn.MSG.Add("Invalid Pay Date"); return false; }
            if (oValue.idAccount == null || oValue.idAccount == 0)
             { oReturn.MSG.Add("Invalid Account"); return false; }
         }

         if (oValue.Settled == false)
         {
            oValue.PayDate = new DateTime?();
            oValue.idAccount = new long?();
          }

         return true;
       }

      #endregion

    }
   #endregion

}
