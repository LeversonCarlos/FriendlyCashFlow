using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using FriendCash.Model.Tools;

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
          get { return ViewBag.PageTitle; }
          set { ViewBag.PageTitle = value; }
        }

       public string PageSubTitle
       {
          get { return ViewBag.PageSubTitle; }
          set { ViewBag.PageSubTitle = value; }
       }

       #endregion

       #region MyLogin

       protected Model.Login MyLogin
       {
          get
          {

             // REFRESH DATA
             if (System.Web.HttpContext.Current.Session["MyLogin"] == null)
             {
                if (this.Request.IsAuthenticated && !string.IsNullOrEmpty(this.User.Identity.Name))
                 { System.Web.HttpContext.Current.Session["MyLogin"] = this.GetRegisteredLogin(this.User.Identity.Name, false); }
              }

             // RETURN DATA
             if (System.Web.HttpContext.Current.Session["MyLogin"] != null)
             { return ((Model.Login)System.Web.HttpContext.Current.Session["MyLogin"]); }
             else { return null; }

          }
          set { System.Web.HttpContext.Current.Session["MyLogin"] = value; }
       }

       protected Model.Login GetRegisteredLogin(string sUserName, bool bCreateNew)
       {
          Model.Login oLogin = null;

          try
          {

             // TEMP DATA
             string idUser = User.Identity.GetUserId();
             oLogin = new Model.Login() { idUser = idUser };
             this.GetRegisteredLogin_ConnStr(oLogin);

             var oReturn = Service.Login.Enter(oLogin, bCreateNew);

             if (oReturn != null && oReturn.OK)
             {
                oLogin = ((Model.Login)oReturn.DATA["Login"]);
                this.GetRegisteredLogin_ConnStr(oLogin);
             }

          }
          catch { throw; }
          return oLogin;
       }

       private void GetRegisteredLogin_ConnStr(Model.Login oLogin)
       {
          /*
          var oSqlCeBuilder = new System.Data.SqlServerCe.SqlCeConnectionStringBuilder();
          oSqlCeBuilder.DataSource = "|DataDirectory|" + oLogin.idUser + ".sdf";
          oLogin.ConnStr = oSqlCeBuilder.ToString();
          */ 
        }

       #endregion

       #region AddMessage

       protected List<Message> MSG
       {
          get 
          {
             if (ViewData["MSG"] == null) { ViewData["MSG"] = new List<Message>(); }
             return ((List<Message>)ViewData["MSG"]);
           }
        }

       protected void AddMessage(string sText, Message.enType iType)
       {
          this.MSG.Add(new Message() { Text = sText, Type = iType });
          ModelState.AddModelError(iType.ToString(), sText);
          TempData["ModelState"] = ModelState;
       }

       protected void AddMessage(Message oMSG)
       { this.AddMessage(oMSG.Text, oMSG.Type); }

       protected void AddMessage(string sText)
       { this.AddMessage(sText, Message.enType.Message); }

       protected void AddMessageInfo(string sText)
       { this.AddMessage(sText, Message.enType.Information); }

       protected void AddMessageWarning(string sText)
       { this.AddMessage(sText, Message.enType.Warning); }

       protected void AddMessageException(string sText)
       { this.AddMessage(sText, Message.enType.Exception); }

       protected override void OnActionExecuted(ActionExecutedContext filterContext)
       {
          if (TempData["ModelState"] != null && !ModelState.Equals(TempData["ModelState"]))
          { ModelState.Merge((ModelStateDictionary)TempData["ModelState"]); }
          base.OnActionExecuted(filterContext);
       }

       #endregion

       #region GetParameters

       protected Parameters GetParameters()
       {
          var oReturn = new Parameters();
          oReturn.Login = this.MyLogin;
          return oReturn;
       }

       protected Parameters GetParameters(string search)
       {
          Int16? page = 1;
          return this.GetParameters(page, ref search);
       }

       protected Parameters GetParameters(Int16? page)
       {
          string search = string.Empty;
          return this.GetParameters(page, ref search);
       }

       protected Parameters GetParameters(Int16? page, ref string search, bool UseSearchSettled)
       {
          if (UseSearchSettled == true)
          {
             var oSearch = new FriendCash.Web.Search(search, true);
             search = oSearch.Value;
           }
          return this.GetParameters(page, ref search);
        }

       protected Parameters GetParameters(Int16? page, ref string search)
       {
          search = (string.IsNullOrEmpty(search) ? "" : search);
          var oReturn = this.GetParameters();
          oReturn.DATA.Add("Page", page);
          oReturn.DATA.Add("Search", search);
          return oReturn;
       }
       #endregion

       #region CheckResult
       protected bool CheckResult(Return oReturn)
       {
          bool bReturn = false;

          // INITIALIZE
          ViewData["NextPage"] = new Int16?();

          if (oReturn != null)
          {

             if (oReturn.DATA.ContainsKey("NextPage"))
             { ViewData["NextPage"] = ((Int16?)oReturn.DATA["NextPage"]); }

             if (oReturn.DATA.ContainsKey("HasMorePages"))
             { ViewData["HasMorePages"] = ((bool)oReturn.DATA["HasMorePages"]); }

             if (oReturn.DATA.ContainsKey("Pages"))
             { ViewData["Pages"] = ((List<Service.Pages>)oReturn.DATA["Pages"]); }

             if (oReturn.DATA.ContainsKey(Service.Base.TAG_SEARCH))
             { ViewData[Service.Base.TAG_SEARCH] = oReturn.DATA[Service.Base.TAG_SEARCH].ToString(); }

             // MSGs
             ViewData["MSG"] = oReturn.MSG;
             /*
             var oWarnings = oReturn.MSG.Where(DATA => DATA.Type == Message.enType.Exception || DATA.Type == Message.enType.Warning);
             if (oWarnings.Count() > 0)
             {
                foreach (var oMSG in oWarnings)
                {
                   this.AddMessage(oMSG.Text, oMSG.Type);
                }
                return bReturn;
             }
            */ 

             bReturn = oReturn.OK;
          }

          return bReturn;
       }
       #endregion

       #region GetJson

       protected JsonResult GetJson(Package oReturn, string sRedirect)
       { return GetJson(oReturn, sRedirect, false); }

       protected JsonResult GetJson(Package oReturn, bool bIncludeData)
       { return GetJson(oReturn, string.Empty, bIncludeData); }

       protected JsonResult GetJson(Package oReturn, string sRedirect, bool bIncludeData)
       {
          var oJson = FriendCash.Web.Code.Converter.GetPackageJson(oReturn, sRedirect, bIncludeData);
          return Json(oJson, JsonRequestBehavior.AllowGet);
        }

       #endregion

       #region RedirectTo
       protected bool RedirectTo(string sAction)
       {
          return this.RedirectTo(sAction: sAction, oValues: null);
       }
       protected bool RedirectTo(string sAction, FriendCash.Web.Search oModel)
       {
          bool bReturn = false;
          try
          {
             Int16? page = 1;
             string search = oModel.Value;
             bReturn = this.RedirectTo(sAction: sAction, oValues: new { page, search });
          }
          catch (Exception ex) { this.AddMessageException(ex.Message); }
          return bReturn;
       }
       protected bool RedirectTo(string sAction, object oValues)
       {
          bool bReturn = false;
          try
          {
             this.Response.RedirectToRoute(sAction, oValues);
             bReturn = true;
          }
          catch (Exception ex) { this.AddMessageException(ex.Message); }
          return bReturn;
       }
       #endregion

    }
}
