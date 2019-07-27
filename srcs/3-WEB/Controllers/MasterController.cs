using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FriendCash.Web.Controllers
{
    public abstract class MasterController : Controller
    {

       #region New
       public MasterController()
       {
          this.PageTitle = "FriendCash";
        }
       #endregion

       #region PageTitle
       public string PageTitle
       {
          get { return ((string)ViewData["PageTitle"]); }
          set { ViewData["PageTitle"] = value; }
       }
       #endregion

       #region MyLogin

       #region MyLoginCode
       protected string MyLoginCode
       {
          get
          {
             string sReturn = string.Empty;
             if (this.Request.IsAuthenticated && !string.IsNullOrEmpty(this.User.Identity.Name))
             {
                sReturn = this.User.Identity.Name;
              }
             return sReturn;
           }
        }
       #endregion

       #region MyLogin
       protected Model.Login MyLogin
       {
          get
          {
             Model.Login oMyLogin = null;

             if (System.Web.HttpContext.Current.Session["MyLogin"] == null && ! string.IsNullOrEmpty(this.MyLoginCode) )
             {

                Service.Login oService = new Service.Login();
                Service.Return oReturn = oService.Get_ByCode(this.MyLoginCode);
                if (oReturn != null && oReturn.OK)
                {
                   System.Web.HttpContext.Current.Session["MyLogin"] = ((Model.Login)oReturn.DATA["Login"]);
                 }

              }

             if (System.Web.HttpContext.Current.Session["MyLogin"] != null)
             {
                oMyLogin = ((Model.Login)System.Web.HttpContext.Current.Session["MyLogin"]);
              }

             return oMyLogin;
           }
          set { System.Web.HttpContext.Current.Session["MyLogin"] = value; }
        }
       #endregion

       #endregion

       #region GetParameters

       protected Service.Parameters GetParameters()
       {
          Service.Parameters oReturn = new Service.Parameters();
          oReturn.Login = this.MyLogin;
          return oReturn;
        }

       protected Service.Parameters GetParameters(string search)
       {
          Int16? page = 1;
          return this.GetParameters(page, ref search);
        }

       protected Service.Parameters GetParameters(Int16? page)
       {
          string search = string.Empty;
          return this.GetParameters(page, ref search);
        }

       protected Service.Parameters GetParameters(Int16? page, ref string search)
       {
          search = (string.IsNullOrEmpty(search) ? "" : search);
          Service.Parameters oReturn = this.GetParameters();
          oReturn.DATA.Add("Page", page);
          oReturn.DATA.Add("Search", search);
          return oReturn;
        }
       #endregion

       #region CheckResult
       protected bool CheckResult(Service.Return oReturn)
       {
          bool bReturn = false;

          // INITIALIZE
          ViewData["NextPage"] = new Int16?();

          if (oReturn != null)
          {
             ViewData["MSG"] = oReturn.MSG;

             if (oReturn.DATA.ContainsKey("NextPage"))
              { ViewData["NextPage"] = ((Int16?)oReturn.DATA["NextPage"]); }

             if (oReturn.DATA.ContainsKey("HasMorePages"))
              { ViewData["HasMorePages"] = ((bool)oReturn.DATA["HasMorePages"]); }

             bReturn = oReturn.OK;
           }

          return bReturn;
        }
       #endregion

       #region Redirect
       protected bool Redirect(string sAction)
       {
          return this.Redirect(sAction: sAction, oValues: null);
        }
       protected bool Redirect(string sAction, FriendCash.Web.Code.MyModels.Search oModel)
       {
          bool bReturn = false;
          try
          {
             Int16? page = 1;
             string search = oModel.Value;
             bReturn = this.Redirect(sAction: sAction, oValues: new { page, search });
          }
          catch (Exception ex) { ViewData["MSG"] = new List<string>() { ex.Message }; }
          return bReturn;
       }
       protected bool Redirect(string sAction, object oValues)
       {
          bool bReturn = false;
          try
          {
             this.Response.RedirectToRoute(sAction, oValues);
             bReturn = true;
           }
          catch (Exception ex) { ViewData["MSG"] = new List<string>() { ex.Message }; }
          return bReturn;
        }
       #endregion

    }
}
