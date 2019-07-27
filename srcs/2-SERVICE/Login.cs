using System;
using System.ServiceModel;
using System.Linq;

namespace FriendCash.Service
{

   #region iLogin
   [ServiceContract]
   public interface iLogin
   {

      [OperationContract]
      Return Get_ByCode(string sCode);

      [OperationContract]
      Return Enter(string sUser, string sPass);

   }
   #endregion

   #region Login
   public class Login: Base, iLogin
   {

      #region Enter
      public Return Enter(string sUser, string sPass)
      {
         Return oReturn = new Return();

         try
         {

            // AUTHENTICATE USER
            Model.User oUser = Service.User.Get_ByAuthentication(sUser, sPass, this.Context);
            if (oUser == null || oUser.idUser == 0) 
            {
               oReturn.MSG.Add("Authentication Failed");
               return oReturn; 
             }

            // REGISTER LOGIN
            Model.Login oLogin = new Model.Login();
            oLogin.idUser = oUser.idUser;
            oLogin.Code = System.Guid.NewGuid().ToString();
            oLogin.RowStatus = Model.Base.enRowStatus.Active;
            this.Context.Logins.Add(oLogin);

            // SAVE
            if (this.Context.SaveChanges(oReturn.MSG) == true)
            {
               oReturn.DATA.Add("Login", oLogin);
               oReturn.OK = true;
             }

          }
         catch (Exception ex) {oReturn.MSG.Add(ex.Message); }

         return oReturn;
       }
      #endregion

      #region Get_ByCode
      public Return Get_ByCode(string sCode)
      {
         Return oReturn = new Return();

         try
         {

            // QUERY
            var oExec = (from Logins in this.Context.Logins
                         where Logins.Code == sCode &&
                               Logins.RowStatusValue == ((short)Model.Base.enRowStatus.Active)
                         select Logins);
            oReturn.DATA.Add("Login", oExec.SingleOrDefault<Model.Login>());

            // OK
            oReturn.OK = true;

         }
         catch (Exception ex) { oReturn.MSG.Add(ex.Message); }

         return oReturn;
      }
      #endregion

   }
   #endregion 

}
