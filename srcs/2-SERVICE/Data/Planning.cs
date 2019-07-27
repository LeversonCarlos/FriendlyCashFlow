using System;
using System.ServiceModel;
using System.Linq;
using System.Collections.Generic;
using FriendCash.Model.Tools;

namespace FriendCash.Service
{
   public class Planning : Base
   {

      #region New
      internal Planning(Model.Login oLogin) : base(oLogin) { }
      internal Planning(Model.Context oContext) : base(oContext) { }
      #endregion

      #region Constants
      public const string TAG_ENTITY = "Planning";
      public const string TAG_ENTITY_LIST = TAG_ENTITY + "s";
      public const string TAG_ENTITY_KEY = "id" + TAG_ENTITY;
      public const string TAG_ENTITY_INDICATORS = TAG_ENTITY + "Indicators";
      public const string TAG_ENTITY_CODE = "Code";
      public const string TAG_ENTITY_TYPE = "Type";
      public const string TAG_ENTITY_PARENT = "idParentRow";
      public const string TAG_ENTITY_JUST_PARENT = "JustParents";
      public const string TAG_DATE = "Date";
      #endregion

      #region GetQuery
      internal IQueryable<Model.Planning> GetQuery(Parameters oParameters)
      {
         IQueryable<Model.Planning> oQuery = null;

         // INITIAL
         oQuery = from DATA in this.Context.Plannings.Include("Childs")
                  where DATA.RowStatusValue == ((short)Model.Base.enRowStatus.Active)
                  select DATA;
         this.ApplyLogin(ref oQuery, oParameters.Login);

         // ID
         long idRow = oParameters.GetNumericData(TAG_ID);
         if (idRow != 0) { oQuery = oQuery.Where(DATA => DATA.idRow == idRow); }

         // KEY
         long idKey = oParameters.GetNumericData(TAG_ENTITY_KEY);
         if (idKey != 0) { oQuery = oQuery.Where(DATA => DATA.idPlanning == idKey); }

         // CODE
         string Code = oParameters.GetTextData(TAG_ENTITY_CODE);
         if (!string.IsNullOrEmpty(Code)) { oQuery = oQuery.Where(DATA => DATA.Description == Code); }

         // TYPE
         if (oParameters.DATA.ContainsKey(TAG_ENTITY_TYPE) && ((Model.Document.enType)oParameters.DATA[TAG_ENTITY_TYPE]) != Model.Document.enType.None)
         {
            Model.Document.enType iType = ((Model.Document.enType)oParameters.DATA[TAG_ENTITY_TYPE]);
            oQuery = oQuery.Where(DATA => DATA.TypeValue == ((short)iType));
         }

         // ID PARENT
         long idParentRow = oParameters.GetNumericData(TAG_ENTITY_PARENT);
         if (idParentRow != 0) { oQuery = oQuery.Where(DATA => DATA.idParentRow == idParentRow); }

         // JUST PARENTS
         bool JustParents = (oParameters.DATA.ContainsKey(TAG_ENTITY_JUST_PARENT) && ((bool)oParameters.DATA[TAG_ENTITY_JUST_PARENT]) == true);
         if (JustParents==true) { oQuery = oQuery.Where(DATA => DATA.idParentRow.HasValue == false || DATA.idParentRow == 0); }

         // SEARCH
         string sSearch = oParameters.GetTextData(TAG_SEARCH);
         if (!string.IsNullOrEmpty(sSearch)) { oQuery = oQuery.Where(DATA => DATA.Description.Contains(sSearch)); }

         // ORDER BY
         oQuery = oQuery.OrderBy(DATA => DATA.TypeValue).ThenBy(DATA => DATA.Description);

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
            { this.ApplyPagination<Service.Planning, Model.Planning>(ref oQuery, oParameters, ref oReturn); }

            // EXECUTE
            oReturn.DATA.Add(TAG_ENTITY_LIST, oQuery.ToList<Model.Planning>());
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

      internal void GetData_BringPlanningTree(Return oReturn, Parameters oParameters)
      {
         try
         {

            // TYPE
            var iType = Model.Document.enType.None;
            if (oParameters.DATA.ContainsKey(TAG_ENTITY_TYPE) && ((Model.Document.enType)oParameters.DATA[TAG_ENTITY_TYPE]) != Model.Document.enType.None)
            { iType = ((Model.Document.enType)oParameters.DATA[TAG_ENTITY_TYPE]); }

            else if (oReturn.DATA.ContainsKey(TAG_ENTITY_LIST) && oReturn.DATA[TAG_ENTITY_LIST] != null)
            {
               Model.Planning oPlanning = ((List<Model.Planning>)oReturn.DATA[TAG_ENTITY_LIST]).SingleOrDefault();
               if (oPlanning != null) { iType = oPlanning.Type; }
             }

            // DATA
            var oPlanningTree = this.GetData_BringPlanningTree_GetData(oParameters.Login, iType);
            if (oPlanningTree != null) { oReturn.DATA.Add("PlanningTree", oPlanningTree); }

         }
         catch { }
      }

      private List<Model.Planning> GetData_BringPlanningTree_GetData(Model.Login oLogin, Model.Document.enType iType)
      {
         List<Model.Planning> oReturn = null;

         try
         {

            // PARAMETERS
            var oParameters = new Parameters();
            oParameters.Login = oLogin;
            oParameters.DATA.Add(TAG_ENTITY_TYPE, iType);
            oParameters.DATA.Add(TAG_ENTITY_JUST_PARENT, true);

            // EXECUTE
            var oQuery = this.GetQuery(oParameters);
            oReturn = oQuery.ToList();

         }
         catch { throw; }

         return oReturn;
      }

      #endregion

      #region GetDataSingle
      internal Model.Planning GetDataSingle(Model.Login oLogin, long idRow, long idKey, string Code, long idParentRow)
      {
         Model.Planning oReturn = null;

         try
         {

            // PARAMETERS
            var oParameters = new Parameters();
            oParameters.DATA.Add(TAG_ID, idRow);
            oParameters.DATA.Add(TAG_ENTITY_KEY, idKey);
            oParameters.DATA.Add(TAG_ENTITY_CODE, Code);
            oParameters.DATA.Add("idParentRow", idParentRow);
            if (oLogin != null) { oParameters.Login = oLogin; }

            // EXECUTE
            var oExecReturn = this.GetData(oParameters, false, false);

            // RESULT
            if (oExecReturn.OK == true && oExecReturn.DATA.ContainsKey(TAG_ENTITY_LIST) && oExecReturn.DATA[TAG_ENTITY_LIST] != null)
            {
               var oList = ((List<Model.Planning>)oExecReturn.DATA[TAG_ENTITY_LIST]);
               oReturn = oList.SingleOrDefault();
            }

         }
         catch { throw; }

         return oReturn;
      }
      #endregion

      #region GetDataList
      internal List<Model.Planning> GetDataList(Model.Login oLogin, string Search)
      {
         List<Model.Planning> oReturn = null;

         // PARAMETERS
         var oParameters = new Parameters();
         oParameters.DATA.Add(TAG_SEARCH, Search);
         oParameters.Login = oLogin;

         // EXECUTE
         var oExecReturn = this.GetData(oParameters, false, false);

         // RESULT
         if (oExecReturn.OK == true && oExecReturn.DATA.ContainsKey(TAG_ENTITY_LIST) && oExecReturn.DATA[TAG_ENTITY_LIST] != null)
         {
            oReturn = ((List<Model.Planning>)oExecReturn.DATA[TAG_ENTITY_LIST]);
         }

         return oReturn;
      }
      #endregion

      #region GetChildren
      internal List<long> GetChildren(long idPlanning, short iType, string idUser)
      {
         List<long> oReturn = null;

         try
         {

            // PARAMETERS
            var sQUERY = "EXEC dbo.v4_PlanningChildren @idUser, @Type, @idPlanning ";
            var oPARAM = new System.Data.SqlClient.SqlParameter[] 
                         { 
                            new System.Data.SqlClient.SqlParameter("idUser", idUser), 
                            new System.Data.SqlClient.SqlParameter("Type", iType), 
                            new System.Data.SqlClient.SqlParameter("idPlanning", idPlanning)
                          };

            // EXECUTE
            var oList = this.Context.Database.SqlQuery<long>(sQUERY, oPARAM);
            oReturn = oList.ToList();
         }
         catch { throw; }

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
            using (var oService = new Service.Planning(oParameters.Login))
            {

               // PARAMETERS
               var idUser = oParameters.Login.idUser;
               var idPlanning = oParameters.GetNumericData(TAG_ENTITY_KEY);
               var dDate = oParameters.GetDateData(TAG_DATE); if (dDate.HasValue == false || dDate.Value == DateTime.MinValue) { dDate = DateTime.Now; }
               var dInitial = dDate.Value; dInitial = new DateTime(dInitial.Year, dInitial.Month, 1);
               var dFinal = dDate.Value.AddMonths(1); dFinal = new DateTime(dFinal.Year, dFinal.Month, 1); dFinal = dFinal.AddDays(-1);
               var iType = (Model.Document.enType)oParameters.GetNumericData(Document.TAG_ENTITY_TYPE);

               // INITIALIZE
               var oViewList = new List<Model.ViewPlanningFlow>();

               // INCOME
               if (iType == Model.Document.enType.Income || iType == Model.Document.enType.None)
               { oService.GetIndicators_Data(idUser, Model.Document.enType.Income, idPlanning, dInitial, dFinal, oViewList); }

               // EXPENSE
               if (iType == Model.Document.enType.Expense || iType == Model.Document.enType.None)
               { oService.GetIndicators_Data(idUser, Model.Document.enType.Expense, idPlanning, dInitial, dFinal, oViewList); }

               // OK
               oReturn.DATA.Add(TAG_ENTITY_INDICATORS, oViewList);
               oReturn.OK = true;

            }

         }
         catch (Exception ex) { oReturn.MSG.Add(new Message() { Exception = ex.Message }); }

         return oReturn;
      }

      private void GetIndicators_Data(string idUser, Model.Document.enType iType, long idPlanning, DateTime dInitial, DateTime dFinal, List<Model.ViewPlanningFlow> oViewList)
      {
         try
         {

            // FLOW
            var oPlanningIDs = this.GetChildren(idPlanning, (short)iType, idUser);
            var oFlow = (
               from DOCUMENT in this.Context.Documents
               join HISTORY in this.Context.Historys on DOCUMENT.idDocument equals HISTORY.idDocument
               where
                  oPlanningIDs.Contains(DOCUMENT.idPlanning.Value) &&
                  DOCUMENT.RowStatusValue == (short)Model.Base.enRowStatus.Active &&
                  HISTORY.RowStatusValue == (short)Model.Base.enRowStatus.Active &&
                  HISTORY.DueDate >= dInitial && HISTORY.DueDate <= dFinal
               group new { DOCUMENT, HISTORY } by new { idPlanning = DOCUMENT.idPlanning.Value, Year = HISTORY.DueDate.Year, Month = HISTORY.DueDate.Month } into GROUP
               select new Model.ViewPlanningFlow
               {
                  idPlanning = GROUP.Key.idPlanning, 
                  DateYear = GROUP.Key.Year,
                  DateMonth = GROUP.Key.Month,
                  Value = GROUP.Sum(x => x.HISTORY.Value)
               }).ToList();

            // PLANNING
            var oPlannings = this.Context.Plannings.Where(PLANNING =>
               (
                  (idPlanning != 0 && PLANNING.idPlanning == idPlanning) ||
                  (idPlanning == 0 && PLANNING.idParentRow == null)
                ) && PLANNING.RowStatusValue == (short)Model.Base.enRowStatus.Active
            ).ToList();

            // VALUES
            this.GetIndicators_Values(idUser, oViewList, oPlannings, oFlow);

         }
         catch { throw; }
      }

      private void GetIndicators_Values(string idUser, List<Model.ViewPlanningFlow> oViewList, ICollection<Model.Planning> oPlannings, List<Model.ViewPlanningFlow> oFlow)
      {
         try
         {

            // LOOP THROUGH PLANNINGS
            foreach (var oPlanning in oPlannings)
            {

               // SUMMARIZE 
               var oChildren = this.GetChildren(oPlanning.idPlanning, oPlanning.TypeValue, idUser);
               var oSum = (from FLOW in oFlow
                           where oChildren.Contains(FLOW.idPlanning)
                           group FLOW by new { Year = FLOW.DateYear, Month = FLOW.DateMonth } into GROUP
                           select new
                           {
                              Date = new DateTime(GROUP.Key.Year, GROUP.Key.Month, 1),
                              Value = GROUP.Sum(x => x.Value)
                           }).ToList();

               // ROW
               try
               {
                  if (oSum.Count > 0)
                  {
                     var oView = new Model.ViewPlanningFlow();
                     oView.idPlanning = oPlanning.idPlanning;
                     oView.Description = oPlanning.Description;
                     oView.Type = oPlanning.Type;
                     oView.Date = oSum.FirstOrDefault().Date;
                     oView.Value = oSum.Sum(x => x.Value);
                     oViewList.Add(oView);
                     if (oPlanning.Childs.Count != 0)
                     {
                        oView.Childrens = new List<Model.ViewPlanningFlow>();
                        this.GetIndicators_Values(idUser, oView.Childrens, oPlanning.Childs, oFlow);
                     }
                  }
               }
               catch { throw; }

            }
         }
         catch { throw; }
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
            using (var oService = new Service.Planning(oParameters.Login))
            {

               // EXECUTE
               oReturn = oService.GetData(oParameters, false, false);

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
            using (var oService = new Service.Planning(oParameters.Login))
            {

               // TYPE
               var iType = ((Model.Document.enType)oParameters.DATA[TAG_ENTITY_TYPE]);

               // EMPTY MODEL
               oReturn.DATA.Add(TAG_ENTITY, new Model.Planning() { Type = iType });

               // PLANNING TREE
               var oPlanningTree = oService.GetData_BringPlanningTree_GetData(oParameters.Login, iType);
               if (oPlanningTree != null) { oReturn.DATA.Add("PlanningTree", oPlanningTree); }

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
            using (var oService = new Service.Planning(oParameters.Login))
            {

               // EXECUTE
               oReturn = oService.GetData(oParameters, false, true);

               // RESULT
               if (oReturn.OK == true && oReturn.DATA.ContainsKey(TAG_ENTITY_LIST) && oReturn.DATA[TAG_ENTITY_LIST] != null)
               {
                  var oList = ((List<Model.Planning>)oReturn.DATA[TAG_ENTITY_LIST]);
                  if (oList.Count == 0) { oReturn.MSG.Add(new Message() { Warning = Resources.Planning.MSG_WARNING_NOT_FOUND }); oReturn.OK = false; }
                  if (oList.Count >= 2) { oReturn.MSG.Add(new Message() { Warning = Resources.Planning.MSG_WARNING_DUPLICITY }); oReturn.OK = false; }
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
         Return oReturn = new Return();

         try
         {

            // INSTANCE
            using (var oService = new Service.Planning(oParameters.Login))
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
         var oData = ((Model.Planning)oParameters.DATA[TAG_ENTITY]);

         // DESCRIPTION
         if (oData.Description == null || string.IsNullOrEmpty(oData.Description))
         { oReturn.MSG.Add(new Message() { Warning = Resources.Planning.MSG_REQUIRED_DESCRIPTION }); return false; }

         // CHECK CODE DUPLICITY
         if (this.ModelSet<Model.Planning>(oParameters.Login).Where(DATA => 
                  DATA.Description == oData.Description &&
                  DATA.RowStatusValue == (short)Model.Base.enRowStatus.Active &&
                  DATA.idParentRow == oData.idParentRow &&
                  DATA.idPlanning != oData.idPlanning).Count() != 0)
         { oReturn.MSG.Add(new Message() { Warning = Resources.Planning.MSG_WARNING_DUPLICITY }); return false; }

         // CHECK PARENTHOOD
         if (oData.idParentRow.HasValue == true && oData.idParentRow.Value != 0 && oData.idPlanning != 0 &&
             this.SaveEdit_Validate_Parenthood(oData.idPlanning, oData.idParentRow.Value) == false)
         { oReturn.MSG.Add(new Message() { Warning = Resources.Planning.MSG_WARNING_PARENTHOOD_COLLAPSE }); return false; }

         // PARENT ROW
         if (oData.idParentRow.HasValue == true && oData.idParentRow.Value == 0)
         { oData.idParentRow = new long?(); }

         // OK
         return true;
      }

      private bool SaveEdit_Validate_Parenthood(long idPlanning, long idParentRow)
      {

         // GET PARENT ROW
         var oList = this.Context.Plannings.Where(DATA => DATA.idRow == idParentRow && DATA.RowStatusValue == (short)Model.Base.enRowStatus.Active);
         if (oList == null || oList.Count() == 0) { return true; }

         // CHECK PARENT OF PARENT
         var oParent = oList.SingleOrDefault();
         if (oParent.idParentRow.HasValue == false || oParent.idParentRow.Value == 0) { return true; }
         if (idPlanning == oParent.idParentRow.Value) { return false; }

         // RECURRENT CHECK
         return this.SaveEdit_Validate_Parenthood(idPlanning, oParent.idParentRow.Value);
       }

      private bool SaveEdit_Apply(Return oReturn, Parameters oParameters)
      {
         bool bReturn = false;

         try
         {

            // DATA
            var oData = ((Model.Planning)oParameters.DATA[TAG_ENTITY]);

            // APPLY
            var oAfterSave = new Action<Model.Planning>(DATA => { if (DATA.idPlanning == 0) { DATA.idPlanning = DATA.idRow; } });
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
            using (var oService = new Service.Planning(oParameters.Login))
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
               oReturn.MSG.Add(new Message() { Warning = Resources.Planning.MSG_WARNING_NOT_FOUND });
               return bReturn;
            }

            // CHECK EXISTENCE
            var oDATA = ((Model.Planning)oParameters.DATA[TAG_ENTITY]);
            if (this.Context.Plannings.Where(DATA => DATA.idRow == oDATA.idRow).Count() == 0)
            {
               oReturn.MSG.Add(new Message() { Warning = Resources.Planning.MSG_WARNING_NOT_FOUND });
               return bReturn;
            }

            // CHECK SONS
            if (this.Context.Plannings.Where(DATA => DATA.idParentRow == oDATA.idRow && DATA.RowStatusValue == (short)Model.Base.enRowStatus.Active).Count() != 0)
            {
               oReturn.MSG.Add(new Message() { Warning = Resources.Planning.MSG_WARNING_DATA_BEING_RELATED_AS_PARENT });
               return bReturn;
            }

            // CHECK FOREIGN KEYS
            if (this.Context.Documents.Where(DATA => DATA.idPlanning == oDATA.idRow && DATA.RowStatusValue == (short)Model.Base.enRowStatus.Active).Count() != 0)
            {
               oReturn.MSG.Add(new Message() { Warning = Resources.Planning.MSG_WARNING_DATA_BEING_RELATED });
               return false;
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
            var oData = ((Model.Planning)oParameters.DATA[TAG_ENTITY]);

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

      #endregion

   }
}
