using System;
using System.ServiceModel;
using System.Linq;
using System.Collections.Generic;

namespace FriendCash.Service
{

   #region iDocument
   [ServiceContract]
   public interface iDocument
   {

      [OperationContract]
      Return GetData(Parameters oParameters);

      [OperationContract]
      Return Update(Parameters oParameters);

   }
   #endregion

   #region Document
   public class Document : Base, iDocument
   {

      #region New
      public Document()
      {
         this.GetEntityName += delegate(ref string Value) { Value = "Document"; };
       }
      #endregion

      #region GetMaxID
      private long GetMaxID()
      {
         long iReturn = 0;

         try
         {

            var oExec = (from DATA in this.Context.Documents
                         select DATA.idDocument);
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
      private IQueryable<Model.Document> GetQuery(Parameters oParameters)
      {
         IQueryable<Model.Document> oQuery = null;

         // QUERY
         oQuery = from DATA in this.Context.Documents
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
          { oQuery = oQuery.Where(DATA => DATA.idDocument == Key); }

         // CODE
         string Code = oParameters.GetTextData("Code");
         if (!string.IsNullOrEmpty(Code))
          { oQuery = oQuery.Where(DATA => DATA.Description == Code); }

         // TYPE
         if (oParameters.DATA.ContainsKey("Type") && oParameters.DATA["Type"] != null)
         {
            Model.Document.enType iType = ((Model.Document.enType)oParameters.DATA["Type"]);
            if (iType != Model.Document.enType.None)
            { oQuery = oQuery.Where(DATA => DATA.TypeValue == ((short)iType)); }
          }

         // SEARCH
         string sSearch = oParameters.GetTextData(this.Fields.Search);
         if (!string.IsNullOrEmpty(sSearch))
         {
            if (sSearch.StartsWith("S0!") || sSearch.StartsWith("S1!") )
            {
               bool bSettled = false; if (sSearch.Substring(0, 3) == "S1!") { bSettled = true; }

               // TODO: APPLY SETTLED FILTER

               if (sSearch.Length == 3) { sSearch = string.Empty; }
               else { sSearch = sSearch.Substring(3); }
             }
            if (!string.IsNullOrEmpty(sSearch))
            {
               oQuery = oQuery.Where(DATA =>
                  DATA.Description.ToLower().Contains(sSearch.ToLower()) 
                  );
               /*
                * TODO: FIND A WAY TO SEARCH ON THE RELATED DATA AS WEEL
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

      public Return GetData(Parameters oParameters)
      {
         Return oReturn = new Return();

         try
         {

            // QUERY
            IQueryable<Model.Document> oQuery = this.GetQuery(oParameters);

            // PAGINATION
            this.ApplyPagination<Service.Document, Model.Document>(ref oQuery, oParameters, ref oReturn);

            // EXECUTE
            List<Model.Document> oDATA = oQuery.ToList<Model.Document>();
            oReturn.DATA.Add(this.Fields.List, oDATA);

            // PLANNING TREE 
            this.GetData_BringPlanningTree(ref oReturn, oParameters);

            // OK
            oReturn.OK = true;

         }
         catch (Exception ex) { oReturn.MSG.Add(ex.Message); }

         return oReturn;
      }

      private void GetData_BringPlanningTree(ref Return oReturn, Parameters oParameters)
      {
         try
         {

            Planning oPlanningService = new Planning();
            oPlanningService.GetData_BringPlanningTree(ref oReturn, oParameters);
            oPlanningService.Dispose();
            oPlanningService = null;

          }
         catch (Exception ex) { throw ex; }
       }

      #endregion

      #region GetData_By

      internal Model.Document GetData_ByCode(Model.Login oLogin, string Code)
       { return this.GetData_By(oLogin, 0, 0, Code); }

      internal Model.Document GetData_ByKey(Model.Login oLogin, long idDocument)
       { return this.GetData_By(oLogin, 0, idDocument, string.Empty); }

      internal Model.Document GetData_ByID(long idRow)
       { return this.GetData_By(null, idRow, 0, string.Empty); }

      private Model.Document GetData_By(Model.Login oLogin, long idRow, long idDocument, string Code)
      {
         Model.Document oReturn = null;

         try
         {

            // PARAMETERS
            Service.Parameters oParameters = new Parameters();
            oParameters.DATA.Add(this.Fields.ID, idRow);
            oParameters.DATA.Add(this.Fields.Key, idDocument);
            oParameters.DATA.Add("Code", Code);
            if (oLogin != null) { oParameters.Login = oLogin; }

            // EXECUTE
            Return oExecReturn = this.GetData(oParameters);

            // RESULT
            if (oExecReturn.OK == true && oExecReturn.DATA.ContainsKey(this.Fields.List) && oExecReturn.DATA[this.Fields.List] != null)
            {
               List<Model.Document> oList = ((List<Model.Document>)oExecReturn.DATA[this.Fields.List]);
               oReturn = oList.SingleOrDefault();
             }

         }
         catch (Exception ex) { throw ex; }

         return oReturn;
      }

      #endregion

      #region GetAll
      internal List<Model.Document> GetAll(Model.Login oLogin)
      {
         List<Model.Document> oReturn = null;

         // PARAMETERS
         Service.Parameters oParameters = new Parameters();
         oParameters.Login = oLogin;

         // EXECUTE
         Return oExecReturn = this.GetData(oParameters);

         // RESULT
         if (oExecReturn.OK == true && oExecReturn.DATA.ContainsKey(this.Fields.List) && oExecReturn.DATA[this.Fields.List] != null)
         {
            oReturn = ((List<Model.Document>)oExecReturn.DATA[this.Fields.List]);
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
            Model.Document oValue = ((Model.Document)oParameters.DATA[this.Fields.Entity]);

            // VALIDATE
            if (this.Update_Validate(ref oReturn, ref oValue) == false) { return oReturn; }

            // ORIGINAL ID
            long idOriginal = this.GetOriginalID(oValue);

            // NEW ROW
            Model.Document oNew = oValue;
            if (oNew.idDocument == 0) { oNew.idDocument = this.GetMaxID(); }
            this.ApplyDataToNew(oNew, oLogin, idOriginal);
            this.Context.Documents.Add(oNew);

            // ORIGINAL ROW
            if (idOriginal > 0)
            {
               Model.Document oOld = this.GetData_ByID(idOriginal);
               this.Context.Documents.Attach(oOld);
               this.ApplyDataToOld(oOld, oLogin);
            }

            // SAVE
            if (this.Context.SaveChanges(oReturn.MSG) == true)
             { oReturn.OK = true; }

         }
         catch (Exception ex) { oReturn.MSG.Add(ex.Message); }

         return oReturn;
      }

      private bool Update_Validate(ref Return oReturn, ref Model.Document oValue)
      {

         if (oValue.Description == null || string.IsNullOrEmpty(oValue.Description))
         { oReturn.MSG.Add("Invalid Description"); return false; }

         if (oValue.idSupplier == null || oValue.idSupplier == 0)
          { oReturn.MSG.Add("Invalid Supplier"); return false; }

         if (oValue.idPlanning == null || oValue.idPlanning == 0)
         { oReturn.MSG.Add("Invalid Planning"); return false; }

         return true;
      }

      #endregion

      #region Add

      internal static long AddModel(Model.Login oLogin, List<string> oMSG, Model.Document oModel)
      {
         long idDocument = 0;
         Document oService = new Document();
         idDocument = oService.Add(oLogin, oMSG, oModel);
         oService.Dispose();
         oService = null;
         return idDocument;
       }

      internal long Add(Model.Login oLogin, List<string> oMSG, Model.Document oModel)
      {
         long idDocument = 0;

         try
         {

            // SEARCH
            if (this.Add_Search(ref idDocument, oModel) == true) { return idDocument; }

            // ID
            idDocument = this.GetMaxID();
            oModel.idDocument = idDocument;

            // BASIC DATA
            long idOriginal = this.GetOriginalID(oModel);
            oModel.RowStatus = Model.Base.enRowStatus.Active;
            this.ApplyDataToNew(oModel, oLogin, idOriginal);
            this.Context.Documents.Add(oModel);

            // SAVE
            if (this.Context.SaveChanges(oMSG) == false) { idDocument = 0; }

         }
         catch (Exception ex) { oMSG.Add(ex.Message); }

         return idDocument;
      }

      private bool Add_Search(ref long idDocument, Model.Document oModel)
      {
         bool bReturn = false;

         try
         {

            // PARAMETERS
            Service.Parameters oParameters = new Parameters();
            if (oModel.Type != Model.Document.enType.None) { oParameters.DATA.Add("Type", oModel.Type); }
            if (!string.IsNullOrEmpty(oModel.Description)) { oParameters.DATA.Add(this.Fields.Search, oModel.Description); }

            // EXECUTE
            Return oExecReturn = this.GetData(oParameters);

            // RESULT
            if (oExecReturn.OK == true && oExecReturn.DATA.ContainsKey(this.Fields.List) && oExecReturn.DATA[this.Fields.List] != null)
            {
               List<Model.Document> oList = ((List<Model.Document>)oExecReturn.DATA[this.Fields.List]);
               Model.Document oTemp = oList.SingleOrDefault();
               if (oTemp != null && oTemp.idDocument != 0)
               {
                  idDocument = oTemp.idDocument;
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
