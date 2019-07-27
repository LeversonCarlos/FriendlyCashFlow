using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FriendCash.Web.Controllers
{
    public class HomeController : MasterController
    {

       #region Home
       public ActionResult Index()
       {
            return View();
        }
       #endregion

       #region Login

       public ActionResult Login()
       {
          this.PageTitle = "FriendCash :: Login";
          return View();
        }

       [AcceptVerbs(HttpVerbs.Post)]
       public ActionResult Login(FormCollection collection)
       {
          try
          {

             // RETRIEVE DATA
             string sUser = collection["oModel.Code"].ToString();
             string sPass = collection["oModel.Password"].ToString();

             // EXECUTE SERVICE
             Service.Login oLoginService = new Service.Login();
             Service.Return oReturn = oLoginService.Enter(sUser, sPass);

             // CHECK RESULT
             if (oReturn != null && oReturn.OK)
             {
                this.MyLogin = ((Model.Login)oReturn.DATA["Login"]);
                System.Web.Security.FormsAuthentication.SetAuthCookie(this.MyLogin.Code, true);
                //System.Web.Security.FormsAuthentication.RedirectFromLoginPage(this.MyLogin.Code, true);
                return View("Index");
              }
             ViewData["MSG"] = oReturn.MSG;

           }
          catch (Exception ex) { throw ex; }

          // OTHERWISE
          return View();
       }

       #endregion

       #region Register

       public ActionResult Register()
       {
          this.PageTitle = "FriendCash :: Register";
          return View();
       }

       [AcceptVerbs(HttpVerbs.Post)]
       public ActionResult Register(FormCollection collection)
       {
          try
          {

             // PREPARE MODEL
             Model.User oUser = new Model.User();
             oUser.Description = collection["oModel.Description"].ToString();
             oUser.Code = collection["oModel.Code"].ToString();
             oUser.Password = collection["oModel.Password"].ToString();
             oUser.RowStatus = Model.Base.enRowStatus.Active;

             // PARAMETERS
             Service.Parameters oParameters = new Service.Parameters();
             oParameters.DATA.Add("User", oUser);

             // EXECUTE SERVICE
             Service.User oService = new Service.User();
             Service.Return oReturn = oService.Update(oParameters);

             // CHECK RESULT
             if (oReturn != null && oReturn.OK)
             {
                return this.Index();
             }
             ViewData["MSG"] = oReturn.MSG;

          }
          catch (Exception ex) { throw ex; }

          // OTHERWISE
          return View();
       }

       #endregion

    }
}
