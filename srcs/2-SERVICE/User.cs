using System;
using System.ServiceModel;
using System.Linq;

namespace FriendCash.Service
{

   #region iUser
   [ServiceContract]
   public interface iUser
   {

      [OperationContract]
      Return Get_ByID(Parameters oParameters);

      [OperationContract]
      Return Get_ByKey(Parameters oParameters);

      [OperationContract]
      Return Get_All(Parameters oParameters);

      [OperationContract]
      Return Update(Parameters oParameters);

   }
   #endregion

   #region User
   //[AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
   public class User : Base, iUser
   {

      #region GetMaxID
      public long GetMaxID()
      {
         long iReturn = 0;

         try
         {

            var oExec = (from Users in this.Context.Users
                         select Users.idUser);
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

      #region Get_ByID

      public Return Get_ByID(Parameters oParameters)
      {
         Return oReturn = new Return();

         try
         {

            // PARAMETERS
            long idRow = ((long)oParameters.DATA["idRow"]);

            // QUERY
            var oExec = (from Users in this.Context.Users
                         where Users.idRow == idRow
                         select Users);
            oReturn.DATA.Add("User", oExec.SingleOrDefault<Model.User>());

            // OK
            oReturn.OK = true;

         }
         catch (Exception ex) { oReturn.MSG.Add(ex.Message); }

         return oReturn;
       }

      private Model.User Get_ByID(long idRow)
      {
         Model.User oReturn = null;

         try
         {

            Service.Parameters oParameters = new Parameters();
            oParameters.DATA.Add("idRow", idRow);

            Return oExecReturn = this.Get_ByID(oParameters);
            if (oExecReturn.OK == true)
            {
               oReturn = ((Model.User)oExecReturn.DATA["User"]);
            }

         }
         catch (Exception ex) { throw ex; }

         return oReturn;
      }

      #endregion

      #region Get_ByKey
      public Return Get_ByKey(Parameters oParameters)
      {
         Return oReturn = new Return();

         try
         {

            // PARAMETERS
            long idUser = ((long)oParameters.DATA["idUser"]);
            string sLoginCode = oParameters.Login.Code;

            // SUBQUERY
            var oSub = from LoginsExt in this.Context.Logins
                       join LoginsInt in this.Context.Logins on LoginsExt.idUser equals LoginsInt.idUser
                       where LoginsInt.Code == sLoginCode
                       select LoginsExt.idRow;

            // QUERY
            var oExec = (from Users in this.Context.Users
                         where
                               oSub.Contains(Users.CreatedBy.Value) &&
                               Users.idUser == idUser &&
                               Users.RowStatusValue == ((short)Model.Base.enRowStatus.Active)
                         select Users);

            // RESULT
            oReturn.DATA.Add("User", oExec.SingleOrDefault<Model.User>());

            // OK
            oReturn.OK = true;

         }
         catch (Exception ex) { oReturn.MSG.Add(ex.Message); }

         return oReturn;
      }
      #endregion

      #region Get_All
      public Return Get_All(Parameters oParameters)
      {
         Return oReturn = new Return();

         try
         {

            // PARAMETERS
            string sLoginCode = oParameters.Login.Code;

            // QUERY
            var oExec = (from Users in this.Context.Users
                         join Logins in this.Context.Logins on Users.CreatedBy equals Logins.idRow
                         where
                               Logins.Code == sLoginCode &&
                               Users.RowStatusValue == ((short)Model.Base.enRowStatus.Active)
                         select Users);

            // RESULT
            oReturn.DATA.Add("User", oExec.SingleOrDefault<Model.User>());

            // OK
            oReturn.OK = true;

         }
         catch (Exception ex) { oReturn.MSG.Add(ex.Message); }

         return oReturn;
      }
      #endregion

      #region Get_ByAuthentication
      static internal Model.User Get_ByAuthentication(string sUser, string sPass, Model.Context oContext)
      {
         Model.User oReturn = null;

         try
         {

            var oExec = (from Users in oContext.Users
                         where Users.Code == sUser &&
                               Users.Password == sPass &&
                               Users.RowStatusValue == ((short)Model.Base.enRowStatus.Active)
                         select Users);

            oReturn = oExec.SingleOrDefault<Model.User>();

         }
         catch (Exception ex) { throw ex; }

         return oReturn;
      }

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
            Model.User oUser = ((Model.User)oParameters.DATA["User"]);

            // ORIGINAL ID
            long idOriginal = this.GetOriginalID(oUser);

            // NEW ROW
            Model.User oNew = oUser;
            if (oNew.idUser == 0) { oNew.idUser = this.GetMaxID(); }
            this.ApplyDataToNew(oNew, oLogin, idOriginal);
            this.Context.Users.Add(oNew);

            // ORIGINAL ROW
            if (idOriginal > 0)
            {
               Model.User oOld = this.Get_ByID(idOriginal);
               this.Context.Users.Attach(oOld);
               this.ApplyDataToOld(oOld, oLogin);
            }

            // SAVE
            if (this.Context.SaveChanges(oReturn.MSG) == true)
             { oReturn.OK = true; }

         }
         catch (Exception ex) { oReturn.MSG.Add(ex.Message); }

         return oReturn;
      }
      #endregion

      #region Import
      public Return Import(Parameters oParameters)
      {
         Return oReturn = new Return();
         ImportData oImport = null;

         try
         {

            // INITIALIZE
            oImport = new ImportData();
            Model.Login oLogin = oParameters.Login;
            System.IO.Stream oStream = ((System.IO.Stream)oParameters.DATA["STREAM"]);

            // VALIDATE
            if (oImport.Validate(ref oReturn, oStream) == false) { return oReturn; }

            // EXECUTE
            if (oImport.Execute(ref oReturn, oStream, oLogin, oReturn.MSG) == false) { return oReturn; }

         }
         catch (Exception ex) { oReturn.MSG.Add(ex.Message); }
         finally { oImport = null; }

         return oReturn;
      }
      #endregion

    }
   #endregion

}
