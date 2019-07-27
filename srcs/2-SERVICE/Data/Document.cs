using System;
using System.ServiceModel;
using System.Linq;
using System.Collections.Generic;
using System.Data.Entity;
using FriendCash.Model.Tools;

namespace FriendCash.Service
{
   public class Document : Base
   {

      #region New
      internal Document(Model.Login oLogin) : base(oLogin) { }
      internal Document(Model.Context oContext) : base(oContext) { }
      #endregion

      #region Constants
      public const string TAG_ENTITY = "Document";
      public const string TAG_ENTITY_LIST = TAG_ENTITY + "s";
      public const string TAG_ENTITY_KEY = "id" + TAG_ENTITY;
      public const string TAG_ENTITY_CODE = "Code";
      public const string TAG_ENTITY_TYPE = "Type";
      public const string TAG_ENTITY_INDICATORS = TAG_ENTITY + "Indicators";
      #endregion

      #region GetQuery
      private IQueryable<Model.Document> GetQuery(Parameters oParameters)
      {
         IQueryable<Model.Document> oQuery = null;

         // INITIAL
         oQuery = from DATA in this.Context.Documents
                  where DATA.RowStatusValue == ((short)Model.Base.enRowStatus.Active)
                  select DATA;
         this.ApplyLogin(ref oQuery, oParameters.Login);

         // ID
         long idRow = oParameters.GetNumericData(TAG_ID);
         if (idRow != 0) { oQuery = oQuery.Where(DATA => DATA.idRow == idRow); }

         // KEY
         long idKey = oParameters.GetNumericData(TAG_ENTITY_KEY);
         if (idKey != 0) { oQuery = oQuery.Where(DATA => DATA.idDocument == idKey); }

         // CODE
         string Code = oParameters.GetTextData(TAG_ENTITY_CODE);
         if (!string.IsNullOrEmpty(Code)) { oQuery = oQuery.Where(DATA => DATA.Description == Code); }

         // TYPE
         if (oParameters.DATA.ContainsKey(TAG_ENTITY_TYPE) && ((Model.Document.enType)oParameters.DATA[TAG_ENTITY_TYPE]) != Model.Document.enType.None)
         {
            Model.Document.enType iType = ((Model.Document.enType)oParameters.DATA[TAG_ENTITY_TYPE]);
            oQuery = oQuery.Where(DATA => DATA.TypeValue == ((short)iType));
         }

         // SEARCH
         string sSearch = oParameters.GetTextData(TAG_SEARCH);
         if (!string.IsNullOrEmpty(sSearch))
         {

            // SETTLED
            if (sSearch.StartsWith("S0!") || sSearch.StartsWith("S1!"))
            {
               bool bSettled = false; if (sSearch.Substring(0, 3) == "S1!") { bSettled = true; }
               oQuery = oQuery.Where(DATA => DATA.Settled == bSettled);
               if (sSearch.Length == 3) { sSearch = string.Empty; }
               else { sSearch = sSearch.Substring(3); }
            }

            if (!string.IsNullOrEmpty(sSearch))
            {
               oQuery = oQuery.Where(DATA =>
                  DATA.Description.ToLower().Contains(sSearch.ToLower())
                  );
               /*
                * TODO: FIND A WAY TO SEARCH ON THE RELATED DATA AS WELL
                  ||
                  DATA.SupplierDetails.Code.ToLower().Contains(sSearch.ToLower()) ||
                  DATA.SupplierDetails.Description.ToLower().Contains(sSearch.ToLower()   
                */
            }
         }

         // ORDER BY
         oQuery = oQuery.OrderBy(DATA => DATA.Description);

         return oQuery;
      }
      #endregion

      #region GetData

      #region GetData

      private Return GetData(Parameters oParameters, bool bApplyPagination, bool bBringPlanningTree)
      {
         var oReturn = new Return();

         try
         {

            // QUERY
            var oQuery = this.GetQuery(oParameters);

            // PAGINATION
            if (bApplyPagination == true)
            { this.ApplyPagination<Service.Document, Model.Document>(ref oQuery, oParameters, ref oReturn); }

            // EXECUTE
            var oList = oQuery.ToList<Model.Document>();
            oList.ForEach(
               oEach => oEach.HystoryCount = this.Context.Historys.Count(DATA => DATA.idDocument== oEach.idRow && DATA.RowStatusValue == (short)Model.Base.enRowStatus.Active) 
             );
            oReturn.DATA.Add(TAG_ENTITY_LIST, oList);
            oReturn.DATA.Add(TAG_SEARCH, oParameters.GetTextData(TAG_SEARCH));

            // PLANNING TREE 
            if (bBringPlanningTree == true)
            { this.GetData_BringPlanningTree(oReturn, oParameters); }

            // OK
            oReturn.OK = true;

         }
         catch (Exception ex) { oReturn.MSG.Add(new Message() { Exception = ex.Message }); }

         return oReturn;
      }

      private void GetData_BringPlanningTree(Return oReturn, Parameters oParameters)
      {
         try
         {

            var oPlanningService = new Planning(oParameters.Login);
            oPlanningService.GetData_BringPlanningTree(oReturn, oParameters);
            oPlanningService.Dispose();
            oPlanningService = null;

          }
         catch { throw; }
       }

      #endregion

      #region GetDataSingle

      internal Model.Document GetDataSingle_ByCode(Model.Login oLogin, string Code)
      { return this.GetDataSingle(oLogin, 0, 0, Code); }

      internal Model.Document GetDataSingle_ByKey(Model.Login oLogin, long idKey)
      { return this.GetDataSingle(oLogin, 0, idKey, string.Empty); }

      internal Model.Document GetDataSingle_ByID(Model.Login oLogin, long idRow)
      { return this.GetDataSingle(oLogin, idRow, 0, string.Empty); }

      private Model.Document GetDataSingle(Model.Login oLogin, long idRow, long idKey, string Code)
      {
         Model.Document oReturn = null;

         try
         {

            // PARAMETERS
            var oParameters = new Parameters();
            oParameters.DATA.Add(TAG_ID, idRow);
            oParameters.DATA.Add(TAG_ENTITY_KEY, idKey);
            oParameters.DATA.Add(TAG_ENTITY_CODE, Code);
            if (oLogin != null) { oParameters.Login = oLogin; }

            // EXECUTE
            Return oExecReturn = this.GetData(oParameters, false, false);

            // RESULT
            if (oExecReturn.OK == true && oExecReturn.DATA.ContainsKey(TAG_ENTITY_LIST) && oExecReturn.DATA[TAG_ENTITY_LIST] != null)
            {
               var oList = ((List<Model.Document>)oExecReturn.DATA[TAG_ENTITY_LIST]);
               oReturn = oList.SingleOrDefault();
             }

         }
         catch { throw; }

         return oReturn;
      }

      #endregion

      #region GetDataList
      internal List<Model.Document> GetDataList(Model.Login oLogin, string Search)
      {
         List<Model.Document> oReturn = null;

         // PARAMETERS
         var oParameters = new Parameters();
         oParameters.DATA.Add(TAG_SEARCH, Search);
         oParameters.Login = oLogin;

         // EXECUTE
         Return oExecReturn = this.GetData(oParameters, false, false);

         // RESULT
         if (oExecReturn.OK == true && oExecReturn.DATA.ContainsKey(TAG_ENTITY_LIST) && oExecReturn.DATA[TAG_ENTITY_LIST] != null)
         {
            oReturn = ((List<Model.Document>)oExecReturn.DATA[TAG_ENTITY_LIST]);
         }

         return oReturn;
      }
      #endregion

      #region GetIndicators

      public static Return GetIndicators(Parameters oParameters)
      {
         var oReturn = new Return();

         try
         {

            // INSTANCE
            using (var oService = new Service.Document(oParameters.Login))
            {

               // EXECUTE
               oService.GetIndicators_Data(oParameters, oReturn);

            }

         }
         catch (Exception ex) { oReturn.MSG.Add(new Message() { Exception = ex.Message }); }

         return oReturn;
      }

      private void GetIndicators_Data(Parameters oParameters, Return oReturn)
      {
         try
         {

            // PARAMETERS
            long idKey = oParameters.GetNumericData(TAG_ENTITY_KEY);

            // DOCUMENT
            var oDocuments = this.GetQuery(oParameters);

            // HISTORYS
            var oHistory = 
               from HISTORY in this.Context.Historys
               where HISTORY.idDocument == idKey &&
                     HISTORY.RowStatusValue == (short)Model.Base.enRowStatus.Active
               select HISTORY;

            // INDICATORS
            var oIndicatorsQuery =
               from DOC in oDocuments
               select new Model.DocumentIndicators
               {
                  idDocument = DOC.idDocument,
                  HistoryCount = (from DATA in oHistory select DATA.idRow).Count(),
                  HistoryUnsettled = (from DATA in oHistory where DATA.Settled == false select DATA.idRow).Count(),
                  ValueTotal = (from DATA in oHistory select DATA.Value).DefaultIfEmpty(0).Sum(),
                  ValueSettled = (from DATA in oHistory where DATA.Settled == true select DATA.Value).DefaultIfEmpty(0).Sum(),
                  ValueUnsettled = (from DATA in oHistory where DATA.Settled == false select DATA.Value).DefaultIfEmpty(0).Sum()
               };

            // EXECUTE
            var oIndicators = oIndicatorsQuery.SingleOrDefault();
            if (oIndicators == null) { oIndicators = new Model.DocumentIndicators(); }

            // FLOW
            var dInitial = DateTime.Now.AddMonths(-4); dInitial = new DateTime(dInitial.Year, dInitial.Month, 1);
            var dFinal = DateTime.Now.AddMonths(3); dFinal = new DateTime(dFinal.Year, dFinal.Month, 1); dFinal = dFinal.AddDays(-1);
            var oFlow = 
               (from DATA in oHistory
                where DATA.DueDate >= dInitial && DATA.DueDate <= dFinal
                orderby DATA.DueDate
                select new
                {
                   Date = DATA.DueDate,
                   Value = DATA.Value
                 }).ToList();
            var oFlowFinal =
               (from DATA in oFlow
                group DATA by new { DATA.Date.Year, DATA.Date.Month } into G
                select new Model.MonthlyFlow
                {
                   Date = new DateTime(G.Key.Year, G.Key.Month, 1),
                   Value = G.Sum(D => D.Value)
                }).ToList();
            var iAverage = (from DATA in oFlowFinal
                            where DATA.Date <= DateTime.Now
                            select DATA.Value).DefaultIfEmpty().Average();
            oFlowFinal.ForEach(EACH => EACH.Average = iAverage);
            oIndicators.Flow = oFlowFinal;

            // OK
            oReturn.DATA.Add(TAG_ENTITY_INDICATORS, oIndicators);
            oReturn.OK = true;

         }
         catch { throw; }
      }

      #endregion

      #endregion 

      #region UpdateFromHistory

      internal static void UpdateFromHistory(Model.Context oContext, Model.Login oLogin, long idDocument)
      {
         try
         {

            // SETTLED
            bool bSettled = UpdateFromHistory_Settled_Read(oContext, oLogin, idDocument);
            UpdateFromHistory_Settled_Write(oContext, oLogin, idDocument, bSettled);

         }
         catch (Exception) { }
      }

      private static bool UpdateFromHistory_Settled_Read(Model.Context oContext, Model.Login oLogin, long idDocument)
      {
         bool bSettled = false;

         try
         {

            var oQuery = from DATA in oContext.Historys
                         where
                               DATA.RowStatusValue == ((short)Model.Base.enRowStatus.Active) && 
                               DATA.idDocument == idDocument 
                         group DATA by DATA.idDocument into GROUP
                         select new
                         {
                            Settled = GROUP.Count(x => x.Settled == true),
                            NotSettled = GROUP.Count(x => x.Settled == false)
                         };
            var oExec = oQuery.SingleOrDefault();

            if (oExec.Settled > 0 && oExec.NotSettled == 0)
            { bSettled = true; }

         }
         catch { }

         return bSettled;
      }

      private static void UpdateFromHistory_Settled_Write(Model.Context oContext, Model.Login oLogin, long idDocument, bool bSettled)
      {
         try
         {

            // GET DOCUMENT
            var oQuery = from DATA in oContext.Documents
                         where
                               DATA.RowStatusValue == ((short)Model.Base.enRowStatus.Active) && 
                               DATA.idDocument == idDocument 
                         select DATA;
            var oExec = oQuery.SingleOrDefault();
            if (oExec == null || oExec.idDocument == 0) { return; }

            // UPDATE DATA
            oExec.Settled = bSettled;
            oContext.Documents.Attach(oExec);
            oContext.Entry<Model.Document>(oExec).State = System.Data.Entity.EntityState.Modified;
            oContext.SaveChanges(null);

         }
         catch { }
      }

      #endregion

      #region Executes

      #region Index
      public static Return Index(Parameters oParameters)
      {
         Return oReturn = null;

         try
         {

            // INSTANCE
            using (var oService = new Service.Document(oParameters.Login))
            {

               // EXECUTE
               oReturn = oService.GetData(oParameters, true, false);

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
            using (var oService = new Service.Document(oParameters.Login))
            {

               // TYPE
               var iType = ((Model.Document.enType)oParameters.DATA[TAG_ENTITY_TYPE]);

               // EMPTY MODEL
               oReturn.DATA.Add(TAG_ENTITY, new Model.Document() { Type = iType });

               // PLANNING TREE
               oService.GetData_BringPlanningTree(oReturn, oParameters);

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
            using (var oService = new Service.Document(oParameters.Login))
            {

               // EXECUTE
               oReturn = oService.GetData(oParameters, false, true);

               // RESULT
               if (oReturn.OK == true && oReturn.DATA.ContainsKey(TAG_ENTITY_LIST) && oReturn.DATA[TAG_ENTITY_LIST] != null)
               {
                  var oList = ((List<Model.Document>)oReturn.DATA[TAG_ENTITY_LIST]);
                  if (oList.Count == 0) { oReturn.MSG.Add(new Message() { Warning = Resources.Document.MSG_WARNING_NOT_FOUND }); oReturn.OK = false; }
                  if (oList.Count >= 2) { oReturn.MSG.Add(new Message() { Warning = Resources.Document.MSG_WARNING_DUPLICITY }); oReturn.OK = false; }
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
            using (var oService = new Service.Document(oParameters.Login))
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
         var oData = ((Model.Document)oParameters.DATA[TAG_ENTITY]);

         // DESCRIPTION
         if (oData.Description == null || string.IsNullOrEmpty(oData.Description))
         { oReturn.MSG.Add(new Message() { Warning = Resources.Document.MSG_REQUIRED_DESCRIPTION }); return false; }

         // SUPPLIER
         if (oData.idSupplier == null || oData.idSupplier == 0)
         { oReturn.MSG.Add(new Message() { Warning = Resources.Document.MSG_REQUIRED_SUPPLIER }); return false; }

         // PLANNING
         if (oData.idPlanning == null || oData.idPlanning == 0)
         { oReturn.MSG.Add(new Message() { Warning = Resources.Document.MSG_REQUIRED_PLANNING }); return false; }
         var oPlanning = this.Context.Plannings.Where(DATA => DATA.idRow == oData.idPlanning).SingleOrDefault();
         if (oPlanning == null) { oReturn.MSG.Add(new Message() { Warning = Resources.Planning.MSG_WARNING_NOT_FOUND }); return false; }
         if (oPlanning.Childs.Count > 0) { oReturn.MSG.Add(new Message() { Warning = Resources.Document.MSG_WARNING_PARENT_PLANNING }); return false; }

         // OK
         return true;
      }

      private bool SaveEdit_Apply(Return oReturn, Parameters oParameters)
      {
         bool bReturn = false;

         try
         {

            // DATA
            var oData = ((Model.Document)oParameters.DATA[TAG_ENTITY]);

            // APPLY
            var oAfterSave = new Action<Model.Document>(DATA => { if (DATA.idDocument == 0) { DATA.idDocument = DATA.idRow; } });
            if (this.ApplySave(oData, oParameters.Login, oReturn.MSG, oAfterSave) == false) { return bReturn; }

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
            using (var oService = new Service.Document(oParameters.Login))
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
               oReturn.MSG.Add(new Message() { Warning = Resources.Document.MSG_WARNING_NOT_FOUND });
               return bReturn;
            }

            // CHECK EXISTENCE
            var oDATA = ((Model.Document)oParameters.DATA[TAG_ENTITY]);
            if (this.Context.Documents.Where(DATA => DATA.idRow == oDATA.idRow).Count() == 0)
            {
               oReturn.MSG.Add(new Message() { Warning = Resources.Document.MSG_WARNING_NOT_FOUND });
               return bReturn;
            }

            // CHECK FOREIGN KEYS
            if (this.Context.Historys.Where(DATA => DATA.idDocument == oDATA.idRow && DATA.RowStatusValue == (short)Model.Base.enRowStatus.Active).Count() != 0 || 
                this.Context.Recurrents.Where(DATA => DATA.idDocument == oDATA.idRow && DATA.RowStatusValue == (short)Model.Base.enRowStatus.Active).Count() != 0)
            {
               oReturn.MSG.Add(new Message() { Warning = Resources.Document.MSG_WARNING_DATA_BEING_RELATED });
               return bReturn;
            }

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
            var oData = ((Model.Document)oParameters.DATA[TAG_ENTITY]);

            // APPLY
            if (this.ApplyRemove(oData, oParameters.Login, oReturn.MSG) == false) { return bReturn; }

            // RETURN 
            oReturn.DATA.Add(TAG_ENTITY, oData);

            // OK
            bReturn = true;

         }
         catch { throw; }

         return bReturn;
      }

      #endregion
      
      #region AutoComplete
      public static Return AutoComplete(Parameters oParameters)
      {
         var oReturn = new Return();

         try
         {

            // INSTANCE
            using (var oService = new Service.Document(oParameters.Login))
            {

               // QUERY
               var oList = oService.GetDataList(oParameters.Login, oParameters.GetTextData(TAG_SEARCH));

               // QUERY: AUTO COMPLETE
               var oResultQuery =
                  from DATA in oList
                  select new AutoCompleteData
                  {
                     Value = DATA.idDocument.ToString(),
                     Description = DATA.Description,
                     Details =
                        "<div class='row'>" +
                           "<span class='col-sm-8'>" + DATA.Description + "</span>" +
                           "<span class='hidden-xs col-sm-4'>" + (DATA.SupplierDetails == null ? string.Empty : DATA.SupplierDetails.Description) + "</span>" +
                        "</div>"
                  };

               // EXECUTE
               var oResultData = oResultQuery.ToList<AutoCompleteData>();
               oReturn.DATA.Add(AutoCompleteData.TAG_LIST_NAME, oResultData);

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
