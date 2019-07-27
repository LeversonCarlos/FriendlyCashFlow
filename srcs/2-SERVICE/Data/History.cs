using System;
using System.ServiceModel;
using System.Linq;
using System.Collections.Generic;
using FriendCash.Model.Tools;

namespace FriendCash.Service
{
   public class History : Base
   {

      #region New
      internal History(Model.Login oLogin) : base(oLogin) { }
      internal History(Model.Context oContext) : base(oContext) { }
      #endregion

      #region Constants
      public const string TAG_ENTITY = "History";
      public const string TAG_ENTITY_LIST = TAG_ENTITY + "s";
      public const string TAG_ENTITY_KEY = "id" + TAG_ENTITY;
      #endregion  

      #region GetQuery
      internal IQueryable<Model.History> GetQuery(Parameters oParameters)
      {
         IQueryable<Model.History> oQuery = null;

         // INITIAL
         oQuery = from DATA in this.Context.Historys
                  where DATA.RowStatusValue == ((short)Model.Base.enRowStatus.Active)
                  select DATA;
         this.ApplyLogin(ref oQuery, oParameters.Login);

         // ID
         long idRow = oParameters.GetNumericData(TAG_ID);
         if (idRow != 0) { oQuery = oQuery.Where(DATA => DATA.idRow == idRow); }

         // KEY
         long idKey = oParameters.GetNumericData(TAG_ENTITY_KEY);
         if (idKey != 0) { oQuery = oQuery.Where(DATA => DATA.idHistory == idKey); }

         // DOCUMENT
         long idDocument = oParameters.GetNumericData(Document.TAG_ENTITY_KEY);
         if (idDocument != 0) { oQuery = oQuery.Where(DATA => DATA.idDocument == idDocument); }

         // SEARCH
         string sSearch = oParameters.GetTextData(TAG_SEARCH);
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

      #region GetDataMaxTransferID
      internal long GetDataMaxTransferID()
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
         catch { throw; }

         return iReturn;
      }
      #endregion     
      
      #region GetData

      private Return GetData(Parameters oParameters, bool bApplyPagination, bool bBringDocument)
      {
         var oReturn = new Return();

         try
         {

            // QUERY
            var oQuery = this.GetQuery(oParameters);

            // PAGINATION
            if (bApplyPagination == true)
            { this.ApplyPagination<Service.History, Model.History>(ref oQuery, oParameters, ref oReturn); }

            // EXECUTE
            oReturn.DATA.Add(TAG_ENTITY_LIST, oQuery.ToList<Model.History>());
            oReturn.DATA.Add(TAG_SEARCH, oParameters.GetTextData(TAG_SEARCH));

            // BRING DOCUMENT
            if (bBringDocument == true)
            { this.GetData_BringDocument(oReturn, oParameters); }

            // OK
            oReturn.OK = true;

         }
         catch (Exception ex) { oReturn.MSG.Add(new Message() { Exception = ex.Message }); }

         return oReturn;
      }

      private void GetData_BringDocument(Return oReturn, Parameters oParameters)
      {
         try
         {

            // ID DOCUMENT
            long idDocument = oParameters.GetNumericData(Document.TAG_ENTITY_KEY);
            if (idDocument == 0 && oReturn.DATA.ContainsKey(TAG_ENTITY_LIST) && oReturn.DATA[TAG_ENTITY_LIST] != null)
            {
               var oHistory = ((List<Model.History>)oReturn.DATA[TAG_ENTITY_LIST]).SingleOrDefault();
               if (oHistory != null) { idDocument = oHistory.idDocument; }
             }
            if (idDocument == 0) { return; }

            var oDocumentService = new Document(oParameters.Login);
            var oDocumentModel = oDocumentService.GetDataSingle_ByKey(oParameters.Login, idDocument);
            if (oDocumentModel != null) { oReturn.DATA.Add(Document.TAG_ENTITY, oDocumentModel); }
            oDocumentService.Dispose();
            oDocumentService = null;

         }
         catch { throw; }
      }

      #endregion

      #region GetDataSingle

      internal Model.History GetDataSingle_ByKey(Model.Login oLogin, long idKey)
      { return this.GetDataSingle(oLogin, 0, idKey); }

      internal Model.History GetDataSingle_ByID(Model.Login oLogin, long idRow)
      { return this.GetDataSingle(oLogin, idRow, 0); }

      private Model.History GetDataSingle(Model.Login oLogin, long idRow, long idKey)
      {
         Model.History oReturn = null;

         try
         {

            // PARAMETERS
            var oParameters = new Parameters();
            oParameters.DATA.Add(TAG_ID, idRow);
            oParameters.DATA.Add(TAG_ENTITY_KEY, idKey);
            if (oLogin != null) { oParameters.Login = oLogin; }
            
            // EXECUTE
            Return oExecReturn = this.GetData(oParameters, false,  false);

            // RESULT
            if (oExecReturn.OK == true && oExecReturn.DATA.ContainsKey(TAG_ENTITY_LIST) && oExecReturn.DATA[TAG_ENTITY_LIST] != null)
            {
               var oList = ((List<Model.History>)oExecReturn.DATA[TAG_ENTITY_LIST]);
               oReturn = oList.SingleOrDefault();
            }

         }
         catch { throw; }

         return oReturn;
      }

      #endregion

      #region GetDataList
      internal List<Model.History> GetDataList(Model.Login oLogin, string Search)
      {
         List<Model.History> oReturn = null;

         // PARAMETERS
         var oParameters = new Parameters();
         oParameters.DATA.Add(TAG_SEARCH, Search);
         oParameters.Login = oLogin;

         // EXECUTE
         Return oExecReturn = this.GetData(oParameters, false, false);

         // RESULT
         if (oExecReturn.OK == true && oExecReturn.DATA.ContainsKey(TAG_ENTITY_LIST) && oExecReturn.DATA[TAG_ENTITY_LIST] != null)
         {
            oReturn = ((List<Model.History>)oExecReturn.DATA[TAG_ENTITY_LIST]);
         }

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
            using (var oService = new Service.History(oParameters.Login))
            {

               // EXECUTE
               oReturn = oService.GetData(oParameters, true, true);

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

            // INSTANCE
            using (var oService = new Service.History(oParameters.Login))
            {

               // PARAMETERS
               long idDocument = oParameters.GetNumericData(Document.TAG_ENTITY_KEY);
               var iType = ((Model.Document.enType)oParameters.DATA[Document.TAG_ENTITY_TYPE]);

               // EMPTY MODEL
               var oData = new Model.History() { Type = iType, idDocument = idDocument, DueDate = DateTime.Now };
               oData.DocumentDetails = oService.Context.Documents.Where(DATA => DATA.idDocument == idDocument && DATA.RowStatusValue == (short)Model.Base.enRowStatus.Active).SingleOrDefault();
               oReturn.DATA.Add(TAG_ENTITY, oData);

               // BRING DOCUMENT
               oService.GetData_BringDocument(oReturn, oParameters);

               // OK
               oReturn.OK = true;

            }

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
            using (var oService = new Service.History(oParameters.Login))
            {

               // EXECUTE
               oReturn = oService.GetData(oParameters, false, true);

               // RESULT
               if (oReturn.OK == true && oReturn.DATA.ContainsKey(TAG_ENTITY_LIST) && oReturn.DATA[TAG_ENTITY_LIST] != null)
               {
                  var oList = ((List<Model.History>)oReturn.DATA[TAG_ENTITY_LIST]);
                  if (oList.Count == 0) { oReturn.MSG.Add(new Message() { Warning = Resources.History.MSG_WARNING_NOT_FOUND }); oReturn.OK = false; }
                  if (oList.Count >= 2) { oReturn.MSG.Add(new Message() { Warning = Resources.History.MSG_WARNING_DUPLICITY }); oReturn.OK = false; }
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
            using (var oService = new Service.History(oParameters.Login))
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
         var oData = ((Model.History)oParameters.DATA[TAG_ENTITY]);

         // DOCUMENT
         if (oData.idDocument == 0)
         { oReturn.MSG.Add(new Message() { Warning = Resources.History.MSG_REQUIRED_DOCUMENT }); return false; }

         // DUE DATE
         if (oData.DueDate == null || oData.DueDate == DateTime.MinValue)
         { oReturn.MSG.Add(new Message() { Warning = Resources.History.MSG_REQUIRED_DUE_DATE }); return false; }

         // VALUE
         if (oData.Value <= 0)
         { oReturn.MSG.Add(new Message() { Warning = Resources.History.MSG_REQUIRED_VALUE }); return false; }

         // SETTLED
         if (oData.Settled == true)
         {
            if (oData.PayDate == null || oData.PayDate.HasValue == false || oData.PayDate.Value == null || oData.PayDate.Value == DateTime.MinValue)
            { oReturn.MSG.Add(new Message() { Warning = Resources.History.MSG_REQUIRED_PAY_DATE }); return false; }
            if (oData.idAccount == null || oData.idAccount == 0)
            { oReturn.MSG.Add(new Message() { Warning = Resources.History.MSG_REQUIRED_ACCOUNT }); return false; }
         }
         else if (oData.Settled == false)
         {
            oData.PayDate = new DateTime?();
         }

         // OK
         return true;
      }

      private bool SaveEdit_Apply(Return oReturn, Parameters oParameters)
      {
         bool bReturn = false;

         try
         {

            // DATA
            var oData = ((Model.History)oParameters.DATA[TAG_ENTITY]);

            // APPLY
            var oAfterSave = new Action<Model.History>(DATA => { if (DATA.idHistory == 0) { DATA.idHistory = DATA.idRow; } });
            if (this.ApplySave(oData, oParameters.Login, oReturn.MSG, oAfterSave) == false) { return bReturn; }

            // SORTING
            oData.SortingRefresh();
            if (this.Context.SaveChanges(oReturn.MSG) == false) { return bReturn; }

            // DOCUMENT HEADER
            Document.UpdateFromHistory(this.Context, oParameters.Login, oData.idDocument);

            // OK
            bReturn = true;

         }
         catch { throw; }

         return bReturn;
      }

      #endregion

      #region SaveRemove

      public static Return SaveRemove(Parameters oParameters)
      {
         var oReturn = new Return();

         try
         {

            // INSTANCE
            using (var oService = new Service.History(oParameters.Login))
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
               oReturn.MSG.Add(new Message() { Warning = Resources.History.MSG_WARNING_NOT_FOUND });
               return bReturn;
            }

            // CHECK EXISTENCE
            var oDATA = ((Model.History)oParameters.DATA[TAG_ENTITY]);
            if (this.Context.Historys.Where(DATA => DATA.idRow == oDATA.idRow).Count() == 0)
            {
               oReturn.MSG.Add(new Message() { Warning = Resources.History.MSG_WARNING_NOT_FOUND });
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
            var oData = ((Model.History)oParameters.DATA[TAG_ENTITY]);

            // APPLY
            if (this.ApplyRemove(oData, oParameters.Login, oReturn.MSG) == false) { return bReturn; }

            // DOCUMENT HEADER
            Document.UpdateFromHistory(this.Context, oParameters.Login, oData.idDocument);

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
