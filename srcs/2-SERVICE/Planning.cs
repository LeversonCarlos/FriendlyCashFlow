using System;
using System.ServiceModel;
using System.Linq;
using System.Collections.Generic;

namespace FriendCash.Service
{

   #region iPlanning
   [ServiceContract]
   public interface iPlanning
   {

      [OperationContract]
      Return GetData(Parameters oParameters);

      [OperationContract]
      Return Update(Parameters oParameters);
    }
   #endregion

   #region
   public class Planning : Base, iPlanning
   {

      #region New
      public Planning()
      {
         this.GetEntityName += delegate(ref string Value) { Value = "Planning"; };
       }
      #endregion

      #region GetMaxID
      private long GetMaxID()
      {
         long iReturn = 0;

         try
         {

            var oExec = (from DATA in this.Context.Plannings
                         select DATA.idPlanning);
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
      private IQueryable<Model.Planning> GetQuery(Parameters oParameters)
      {
         IQueryable<Model.Planning> oQuery = null;

         // QUERY
         oQuery = from DATA in this.Context.Plannings
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
          { oQuery = oQuery.Where(DATA => DATA.idPlanning == Key); }

         // CODE
         string Code = oParameters.GetTextData("Code");
         if (!string.IsNullOrEmpty(Code))
          { oQuery = oQuery.Where(DATA => DATA.Description == Code); }

         // TYPE
         if (oParameters.DATA.ContainsKey("Type") && oParameters.DATA["Type"] != null)
         {
            Model.Document.enType iType = ((Model.Document.enType)oParameters.DATA["Type"]);
            oQuery = oQuery.Where(DATA => DATA.TypeValue == ((short)iType)); 
          }

         // ID PARENT
         long idParentRow = oParameters.GetNumericData("idParentRow");
         if (idParentRow != 0)
         { oQuery = oQuery.Where(DATA => DATA.idParentRow == idParentRow); }

         // JUST PARENTS
         if (oParameters.DATA.ContainsKey("JustParents") && oParameters.DATA["JustParents"] != null)
         {
            bool bJustParents = ((bool)oParameters.DATA["JustParents"]);
            if (bJustParents == true) { oQuery = oQuery.Where(DATA => DATA.idParentRow.HasValue == false || DATA.idParentRow == 0); }
          }

         // SEARCH
         string sSearch = oParameters.GetTextData(this.Fields.Search);
         if (!string.IsNullOrEmpty(sSearch))
          { oQuery = oQuery.Where(DATA => DATA.Description.Contains(sSearch)); }

         // ORDER BY
         oQuery = oQuery.OrderBy(DATA => DATA.TypeValue).ThenBy(DATA => DATA.Description);

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
            IQueryable<Model.Planning> oQuery = this.GetQuery(oParameters);

            // EXECUTE
            List<Model.Planning> oDATA = oQuery.ToList<Model.Planning>();
            oReturn.DATA.Add(this.Fields.List, oDATA);

            // PLANNING TREE 
            this.GetData_BringPlanningTree(ref oReturn, oParameters);

            oReturn.OK = true;

         }
         catch (Exception ex) { oReturn.MSG.Add(ex.Message); }

         return oReturn;
      }

      internal void GetData_BringPlanningTree(ref Return oReturn, Parameters oParameters)
      {
         try
         {

            if (!oParameters.DATA.ContainsKey("BringPlanningTree") || oParameters.DATA["BringPlanningTree"] == null || ((bool)oParameters.DATA["BringPlanningTree"]) == false)
             { return; }

            Model.Document.enType iType = Model.Document.enType.None;
            if (oParameters.DATA.ContainsKey("Type") && oParameters.DATA["Type"] != null)
             { iType = ((Model.Document.enType)oParameters.DATA["Type"]); }

            else if (oReturn.DATA.ContainsKey(this.Fields.List) && oReturn.DATA[this.Fields.List] != null)
            {
               Model.Planning oPlanning = ((List<Model.Planning>)oReturn.DATA[this.Fields.List]).SingleOrDefault();
               if (oPlanning != null) { iType = oPlanning.Type; }
             }

            List<Model.Planning> oPlanningTree = this.GetData_BringPlanningTree_GetData(iType);
            if (oPlanningTree != null)
             { oReturn.DATA.Add("PlanningTree", oPlanningTree); }

         }
         catch (Exception) { }
      }

      private List<Model.Planning> GetData_BringPlanningTree_GetData(Model.Document.enType iType)
      {
         List<Model.Planning> oReturn = null;

         try
         {

            // PARAMETERS
            Service.Parameters oParameters = new Parameters();
            oParameters.DATA.Add("Type", iType);
            oParameters.DATA.Add("JustParents", true);

            // EXECUTE
            Return oExecReturn = this.GetData(oParameters);

            // RESULT
            if (oExecReturn.OK == true && oExecReturn.DATA.ContainsKey(this.Fields.List) && oExecReturn.DATA[this.Fields.List] != null)
            {
               oReturn = ((List<Model.Planning>)oExecReturn.DATA[this.Fields.List]);
             }

         }
         catch (Exception ex) { throw ex; }

         return oReturn;
      }

      #endregion

      #region GetData_By

      internal Model.Planning GetData_ByCode(string Code, long idParentRow)
       { return this.GetData_By(0, 0, Code, idParentRow); }

      internal Model.Planning GetData_ByKey(long idDocument)
       { return this.GetData_By(0, idDocument, string.Empty, 0); }

      internal Model.Planning GetData_ByID(long idRow)
       { return this.GetData_By(idRow, 0, string.Empty, 0); }

      private Model.Planning GetData_By(long idRow, long idDocument, string Code, long idParentRow)
      {
         Model.Planning oReturn = null;

         try
         {

            // PARAMETERS
            Service.Parameters oParameters = new Parameters();
            oParameters.DATA.Add(this.Fields.ID, idRow);
            oParameters.DATA.Add(this.Fields.Key, idDocument);
            oParameters.DATA.Add("Code", Code);
            oParameters.DATA.Add("idParentRow", idParentRow);

            // EXECUTE
            Return oExecReturn = this.GetData(oParameters);

            // RESULT
            if (oExecReturn.OK == true && oExecReturn.DATA.ContainsKey(this.Fields.List) && oExecReturn.DATA[this.Fields.List] != null)
            {
               List<Model.Planning> oList = ((List<Model.Planning>)oExecReturn.DATA[this.Fields.List]);
               oReturn = oList.SingleOrDefault();
            }

         }
         catch (Exception ex) { throw ex; }

         return oReturn;
      }

      #endregion

      #region GetAll
      internal List<Model.Planning> GetAll(Model.Login oLogin)
      {
         List<Model.Planning> oReturn = null;

         // PARAMETERS
         Service.Parameters oParameters = new Parameters();
         oParameters.Login = oLogin;

         // EXECUTE
         Return oExecReturn = this.GetData(oParameters);

         // RESULT
         if (oExecReturn.OK == true && oExecReturn.DATA.ContainsKey(this.Fields.List) && oExecReturn.DATA[this.Fields.List] != null)
         {
            oReturn = ((List<Model.Planning>)oExecReturn.DATA[this.Fields.List]);
         }

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
            Model.Planning oValue = ((Model.Planning)oParameters.DATA[this.Fields.Entity]);

            // VALIDATE
            if (this.Update_Validate(ref oReturn, ref oValue) == false) { return oReturn; }

            // ORIGINAL ID
            long idOriginal = this.GetOriginalID(oValue);

            // NEW ROW
            Model.Planning oNew = oValue;
            if (oNew.idPlanning == 0) { oNew.idPlanning = this.GetMaxID(); }
            this.ApplyDataToNew(oNew, oLogin, idOriginal);
            this.Context.Plannings.Add(oNew);

            // ORIGINAL ROW
            if (idOriginal > 0)
            {
               Model.Planning oOld = this.GetData_ByID(idOriginal);
               this.Context.Plannings.Attach(oOld);
               this.ApplyDataToOld(oOld, oLogin);
            }

            // SAVE
            if (this.Context.SaveChanges(oReturn.MSG) == true)
            { oReturn.OK = true; }

         }
         catch (Exception ex) { oReturn.MSG.Add(ex.Message); }

         return oReturn;
      }

      private bool Update_Validate(ref Return oReturn, ref Model.Planning oValue)
      {

         if (oValue.Description == null || string.IsNullOrEmpty(oValue.Description))
          { oReturn.MSG.Add("Invalid Description"); return false; }

         if (oValue.idParentRow.HasValue == true && oValue.idParentRow.Value == 0)
          { oValue.idParentRow = new long?(); }

         return true;
      }

      #endregion

      #region Add

      internal static long AddModel(Model.Login oLogin, List<string> oMSG, Model.Planning oModel)
      {
         long idPlanning = 0;
         Planning oService = new Planning();
         idPlanning = oService.Add(oLogin, oMSG, oModel);
         oService.Dispose();
         oService = null;
         return idPlanning;
      }

      internal long Add(Model.Login oLogin, List<string> oMSG, Model.Planning oModel)
      {
         long idPlanning = 0;

         try
         {

            // SEARCH
            if (this.Add_Search(ref idPlanning, oModel) == true) { return idPlanning; }

            // ID
            idPlanning = this.GetMaxID();
            oModel.idPlanning = idPlanning;

            // BASIC DATA
            long idOriginal = this.GetOriginalID(oModel);
            oModel.RowStatus = Model.Base.enRowStatus.Active;
            this.ApplyDataToNew(oModel, oLogin, idOriginal);
            this.Context.Plannings.Add(oModel);

            // SAVE
            if (this.Context.SaveChanges(oMSG) == false) { idPlanning = 0; }

         }
         catch (Exception ex) { oMSG.Add(ex.Message); }

         return idPlanning;
      }

      private bool Add_Search(ref long idPlanning, Model.Planning oModel)
      {
         bool bReturn = false;

         try
         {

            // PARAMETERS
            Service.Parameters oParameters = new Parameters();
            if (!string.IsNullOrEmpty(oModel.Description)) { oParameters.DATA.Add(this.Fields.Search, oModel.Description); }

            // EXECUTE
            Return oExecReturn = this.GetData(oParameters);

            // RESULT
            if (oExecReturn.OK == true && oExecReturn.DATA.ContainsKey(this.Fields.List) && oExecReturn.DATA[this.Fields.List] != null)
            {
               List<Model.Planning> oList = ((List<Model.Planning>)oExecReturn.DATA[this.Fields.List]);
               Model.Planning oTemp = oList.SingleOrDefault();
               if (oTemp != null && oTemp.idPlanning != 0)
               {
                  idPlanning = oTemp.idPlanning;
                  bReturn = true;
               }
            }

         }
         catch (Exception) { throw; }

         return bReturn;
      }

      #endregion

    }
   #endregion

}
