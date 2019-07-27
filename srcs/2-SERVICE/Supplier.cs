using System;
using System.ServiceModel;
using System.Linq;
using System.Collections.Generic;

namespace FriendCash.Service
{

   #region iSupplier
   [ServiceContract]
   public interface iSupplier
   {

      [OperationContract]
      Return GetData(Parameters oParameters);

      [OperationContract]
      Return Update(Parameters oParameters);

   }
   #endregion

   #region Supplier
   public class Supplier : Base, iSupplier
   {

      #region New
      public Supplier()
      {
         this.GetEntityName += delegate(ref string Value) { Value = "Supplier"; };
       }
      #endregion

      #region GetMaxID
      private long GetMaxID()
      {
         long iReturn = 0;

         try
         {

            var oExec = (from DATA in this.Context.Suppliers
                         select DATA.idSupplier);
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
      private IQueryable<Model.Supplier> GetQuery(Parameters oParameters)
      {
         IQueryable<Model.Supplier> oQuery = null;

         // QUERY
         oQuery = from DATA in this.Context.Suppliers
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
          { oQuery = oQuery.Where(DATA => DATA.idSupplier == Key); }

         // CODE
         string Code = oParameters.GetTextData("Code");
         if (! string.IsNullOrEmpty(Code))
          { oQuery = oQuery.Where(DATA => DATA.Code == Code); }

         // SEARCH
         string sSearch = oParameters.GetTextData(this.Fields.Search); 
         if (!string.IsNullOrEmpty(sSearch))
         {
            oQuery = oQuery.Where(DATA =>
               DATA.Code.ToLower().Contains(sSearch.ToLower()) ||
               DATA.Description.ToLower().Contains(sSearch.ToLower())
               );
            }

         // ORDER BY
         oQuery = oQuery.OrderBy(DATA => DATA.Code);

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
            IQueryable<Model.Supplier> oQuery = this.GetQuery(oParameters);

            // PAGINATION
            this.ApplyPagination<Service.Supplier, Model.Supplier>(ref oQuery, oParameters, ref oReturn);

            // EXECUTE
            oReturn.DATA.Add(this.Fields.List, oQuery.ToList<Model.Supplier>());

            // OK
            oReturn.OK = true;

         }
         catch (Exception ex) { oReturn.MSG.Add(ex.Message); }

         return oReturn;
       }
      #endregion

      #region GetData_By

      internal Model.Supplier GetData_ByCode(Model.Login oLogin, string Code)
      { return this.GetData_By(oLogin, 0, 0, Code); }

      internal Model.Supplier GetData_ByKey(Model.Login oLogin, long idDocument)
      { return this.GetData_By(oLogin, 0, idDocument, string.Empty); }

      internal Model.Supplier GetData_ByID(long idRow)
      { return this.GetData_By(null, idRow, 0, string.Empty); }

      private Model.Supplier GetData_By(Model.Login oLogin, long idRow, long idDocument, string Code)
      {
         Model.Supplier oReturn = null;

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
               List<Model.Supplier> oList = ((List<Model.Supplier>)oExecReturn.DATA[this.Fields.List]);
               oReturn = oList.SingleOrDefault();
             }

         }
         catch (Exception ex) { throw ex; }

         return oReturn;
      }

      #endregion

      #region GetAll
      internal List<Model.Supplier> GetAll(Model.Login oLogin)
      {
         List<Model.Supplier> oReturn = null;

         // PARAMETERS
         Service.Parameters oParameters = new Parameters();
         oParameters.Login = oLogin;

         // EXECUTE
         Return oExecReturn = this.GetData(oParameters);

         // RESULT
         if (oExecReturn.OK == true && oExecReturn.DATA.ContainsKey(this.Fields.List) && oExecReturn.DATA[this.Fields.List] != null)
         {
            oReturn = ((List<Model.Supplier>)oExecReturn.DATA[this.Fields.List]);
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
            Model.Supplier oValue = ((Model.Supplier)oParameters.DATA[this.Fields.Entity]);

            // VALIDATE
            if (this.Update_Validate(ref oReturn, ref oValue) == false) { return oReturn; }

            // ORIGINAL ID
            long idOriginal = this.GetOriginalID(oValue);

            // NEW ROW
            Model.Supplier oNew = oValue;
            if (oNew.idSupplier == 0) { oNew.idSupplier = this.GetMaxID(); }
            this.ApplyDataToNew(oNew, oLogin, idOriginal);
            this.Context.Suppliers.Add(oNew);

            // ORIGINAL ROW
            if (idOriginal > 0)
            {
               Model.Supplier oOld = this.GetData_ByID(idOriginal);
               this.Context.Suppliers.Attach(oOld);
               this.ApplyDataToOld(oOld, oLogin);
             }

            // SAVE
            if (this.Context.SaveChanges(oReturn.MSG) == true)
             { oReturn.OK = true; }

         }
         catch (Exception ex) { oReturn.MSG.Add(ex.Message); }

         return oReturn;
      }

      private bool Update_Validate(ref Return oReturn, ref Model.Supplier oValue)
      {

         if (oValue.Code == null || string.IsNullOrEmpty(oValue.Code))
          { oReturn.MSG.Add("Invalid Code"); return false; }

         if (oValue.Description == null || string.IsNullOrEmpty(oValue.Description))
          { oReturn.MSG.Add("Invalid Description"); return false; }

         return true;
      }

      #endregion

      #region Add

      internal static long AddModel(Model.Login oLogin, List<string> oMSG, Model.Supplier oModel)
      {
         long idSupplier = 0;
         Supplier oService = new Supplier();
         idSupplier = oService.Add(oLogin, oMSG, oModel);
         oService.Dispose();
         oService = null;
         return idSupplier;
      }

      internal long Add(Model.Login oLogin, List<string> oMSG, Model.Supplier oModel)
      {
         long idSupplier = 0;

         try
         {

            // SEARCH
            if (this.Add_Search(ref idSupplier, oModel) == true) { return idSupplier; }

            // ID
            idSupplier = this.GetMaxID();
            oModel.idSupplier = idSupplier;

            // BASIC DATA
            long idOriginal = this.GetOriginalID(oModel);
            oModel.RowStatus = Model.Base.enRowStatus.Active;
            this.ApplyDataToNew(oModel, oLogin, idOriginal);
            this.Context.Suppliers.Add(oModel);

            // SAVE
            if (this.Context.SaveChanges(oMSG) == false) { idSupplier = 0; }

         }
         catch (Exception ex) { oMSG.Add(ex.Message); }

         return idSupplier;
      }

      private bool Add_Search(ref long idSupplier, Model.Supplier oModel)
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
               List<Model.Supplier> oList = ((List<Model.Supplier>)oExecReturn.DATA[this.Fields.List]);
               Model.Supplier oTemp = oList.SingleOrDefault();
               if (oTemp != null && oTemp.idSupplier != 0)
               {
                  idSupplier = oTemp.idSupplier;
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
