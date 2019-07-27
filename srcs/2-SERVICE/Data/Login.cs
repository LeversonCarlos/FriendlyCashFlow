using System;
using System.ServiceModel;
using System.Linq;
using FriendCash.Model.Tools;

namespace FriendCash.Service
{
   public class Login: Base
   {

      #region New
      internal Login(Model.Login oLogin) : base(oLogin) { }
      #endregion

      #region Enter
      public static Return Enter(Model.Login oTry, bool CreateNew)
      {
         var oReturn = new Return();
         Model.Login oLogin = null;

         try
         {

            // INSTANCE
            using (var oService = new Service.Login(oTry))
            {

               // CHECK ACTIVE LOGIN
               if (CreateNew == false && oService.HasActiveLogin(ref oLogin, oTry.idUser) == true)
               {
                  oReturn.DATA.Add("Login", oLogin);
                  oReturn.OK = true;
               }

               // REGISTER NEW LOGIN
               else
               {
                  oLogin = new Model.Login();
                  oLogin.idUser = oTry.idUser;
                  oLogin.Code = System.Guid.NewGuid().ToString();
                  oLogin.RowStatus = Model.Base.enRowStatus.Active;
                  oService.Context.Logins.Add(oLogin);
                  if (oService.Context.SaveChanges(oReturn.MSG) == true)
                  {
                     oReturn.DATA.Add("Login", oLogin);
                     oReturn.OK = true;
                  }
               }

            }

          }
         catch (Exception ex) { oReturn.MSG.Add(new Message() { Exception = ex.Message }); }

         return oReturn;
       }
      #endregion

      #region HasActiveLogin
      private bool HasActiveLogin(ref Model.Login oLogin, string idUser)
      {
         bool bReturn = false;

         try
         {

            // QUERY
            var oExec = (from Logins in this.Context.Logins
                         where Logins.idUser == idUser &&
                               Logins.RowStatusValue == ((short)Model.Base.enRowStatus.Active)
                         select Logins);
            oExec = oExec.OrderByDescending(DATA => DATA.CreatedIn);

            // EXECUTE
            oLogin = oExec.FirstOrDefault<Model.Login>();
            if (oLogin != null && oLogin.idRow != 0)
            {
               TimeSpan oDif = DateTime.Now.Subtract(oLogin.CreatedIn);
               if (oDif.TotalDays <= 1)
                { bReturn = true; }
             }

         }
         catch { }

         return bReturn;
      }
      #endregion

   }
}
