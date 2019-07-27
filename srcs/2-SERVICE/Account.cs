using System;
using System.ServiceModel;
using System.Linq;
using System.Collections.Generic;

namespace FriendCash.Service
{

   #region iAccount
   [ServiceContract]
   public interface iAccount
   {

      [OperationContract]
      Return GetData(Parameters oParameters);

      [OperationContract]
      Return Update(Parameters oParameters);
   }
   #endregion

   #region Account
   public class Account : Base, iAccount
   {

      #region New
      public Account()
      {
         this.GetEntityName += delegate(ref string Value) { Value = "Account"; };
       }
      #endregion

      #region GetMaxID
      private long GetMaxID()
      {
         long iReturn = 0;

         try
         {

            var oExec = (from DATA in this.Context.Accounts
                         select DATA.idAccount);
            if (oExec != null && oExec.Count<long>() >0)
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
      private IQueryable<Model.Account> GetQuery(Parameters oParameters)
      {
         IQueryable<Model.Account> oQuery = null;

         // QUERY
         oQuery = from DATA in this.Context.Accounts
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
          { oQuery = oQuery.Where(DATA => DATA.idAccount == Key); }

         // SEARCH
         string sSearch = oParameters.GetTextData(this.Fields.Search); 
         if (!string.IsNullOrEmpty(sSearch))
         {
            oQuery = oQuery.Where(DATA =>
               DATA.Code.Contains(sSearch) ||
               DATA.Description.Contains(sSearch)
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
            IQueryable<Model.Account> oQuery = this.GetQuery(oParameters);

            // PAGINATION
            this.ApplyPagination<Service.Account, Model.Account>(ref oQuery, oParameters, ref oReturn);

            // EXECUTE
            List<Model.Account> oDATA = oQuery.ToList<Model.Account>();
            oReturn.DATA.Add(this.Fields.List, oDATA);

            // OK
            oReturn.OK = true;

         }
         catch (Exception ex) { oReturn.MSG.Add(ex.Message); }

         return oReturn;
      }
      #endregion

      #region GetData_By

      internal Model.Account GetData_ByKey(long idDocument)
       { return this.GetData_By(0, idDocument); }

      internal Model.Account GetData_ByID(long idRow)
       { return this.GetData_By(idRow, 0); }

      private Model.Account GetData_By(long idRow, long idDocument)
      {
         Model.Account oReturn = null;

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
               List<Model.Account> oList = ((List<Model.Account>)oExecReturn.DATA[this.Fields.List]);
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
            Model.Account oValue = ((Model.Account)oParameters.DATA[this.Fields.Entity]);

            // VALIDATE
            if (this.Update_Validate(ref oReturn, ref oValue) == false) { return oReturn; }

            // ORIGINAL ID
            long idOriginal = this.GetOriginalID(oValue);

            // NEW ROW
            Model.Account oNew = oValue;
            if (oNew.idAccount == 0) { oNew.idAccount = this.GetMaxID(); }
            this.ApplyDataToNew(oNew, oLogin, idOriginal);
            this.Context.Accounts.Add(oNew);

            // ORIGINAL ROW
            if (idOriginal > 0)
            {
               Model.Account oOld = this.GetData_ByID(idOriginal);
               this.Context.Accounts.Attach(oOld);
               this.ApplyDataToOld(oOld, oLogin);
             }

            // SAVE
            if (this.Context.SaveChanges(oReturn.MSG) == true)
             { oReturn.OK = true; }

         }
         catch (Exception ex) { oReturn.MSG.Add(ex.Message); }

         return oReturn;
      }

      private bool Update_Validate(ref Return oReturn, ref Model.Account oValue)
      {

         if (oValue.Code == null || string.IsNullOrEmpty(oValue.Code))
         { oReturn.MSG.Add("Invalid Code"); return false; }

         if (oValue.Description == null || string.IsNullOrEmpty(oValue.Description))
         { oReturn.MSG.Add("Invalid Description"); return false; }

         if (
             oValue.Type == Model.Account.enType.CreditCard &&
             (oValue.DueDay==null || oValue.DueDay.HasValue == false || oValue.DueDay.Value <= 0 || oValue.DueDay.Value > 31)
            )
          { oReturn.MSG.Add("Invalid Due Day"); return false; }

         return true;
      }

      #endregion

   }
   #endregion

 }
