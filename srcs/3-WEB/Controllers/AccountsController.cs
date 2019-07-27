using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FriendCash.Web.Controllers
{
   public class AccountsController : MasterController
   {

      #region Initialize
      public AccountsController()
      {
         this.PageTitle = "FriendCash :: Accounts";
       }
      #endregion

      #region Property

      #region Service
      private Service.Account roService = null;
      private Service.Account Service
      {
         get
         {
            if (this.roService == null)
            { this.roService = new Service.Account(); }
            return this.roService;
         }
      }
      #endregion

      #endregion 

      #region Data

      #region GetIndexData
      public bool GetIndexData(Int16? page, string search)
      {
         bool bReturn = false;
         try
         {

            // PARAMETERS
            Service.Parameters oParameters = this.GetParameters(page, ref search);

            // SERVICE CALL
            Service.Return oReturn = this.Service.GetData(oParameters);

            // CHECK RESULT
            if (this.CheckResult(oReturn) == true)
            {
               ViewData[this.Service.Fields.List] = ((List<Model.Account>)oReturn.DATA[this.Service.Fields.List]);
               ViewData[this.Service.Fields.Search] = search;
               bReturn = true;
            }
         }
         catch (Exception ex) { ViewData["MSG"] = new List<string>() { ex.Message }; }
         return bReturn;
      }
      #endregion 

      #region GetEditData
      public bool GetEditData(long id)
      {
         bool bReturn = false;
         try
         {

            // PARAMETERS
            Service.Parameters oParameters = this.GetParameters();
            oParameters.DATA.Add(this.Service.Fields.Key, id);

            // SERVICE CALL
            Service.Return oReturn = this.Service.GetData(oParameters);

            // CHECK RESULT
            if (this.CheckResult(oReturn) == true)
            {
               ViewData[this.Service.Fields.Entity] = ((List<Model.Account>)oReturn.DATA[this.Service.Fields.List]).FirstOrDefault();
               if (ViewData[this.Service.Fields.Entity] != null)
                { this.PageTitle += " [" + ((Model.Account)ViewData[this.Service.Fields.Entity]).Description + "]"; }
               bReturn = true;
            }
         }
         catch (Exception ex) { ViewData["MSG"] = new List<string>() { ex.Message }; }
         return bReturn;
      }
      #endregion 

      #region UpdateData
      public bool UpdateData(Model.Account oModel)
      {
         bool bReturn = false;
         try
         {

            // PARAMETERS
            Service.Parameters oParameters = this.GetParameters();
            oParameters.DATA.Add(this.Service.Fields.Entity, oModel);

            // SERVICE
            Service.Return oReturn = this.Service.Update(oParameters);

            // CHECK RESULT
            if (this.CheckResult(oReturn) == true)
            {
               bReturn = true;
            }

         }
         catch (Exception ex) { ViewData["MSG"] = new List<string>() { ex.Message }; }

         return bReturn;
      }
      #endregion

      #endregion

      #region Action

      #region Index

      public ActionResult Index(Int16? page, string search)
      {
         if (this.GetIndexData(page, search) == true)
          { return View(ViewData[this.Service.Fields.List]); }
         else
          { return View("Error"); }
       }

      [AcceptVerbs(HttpVerbs.Post)]
      public ActionResult IndexMore(Int16? page, string search)
      {
         if (this.GetIndexData(page, search) == true)
          { return PartialView("List", ViewData[this.Service.Fields.List]); }
         else
          { return View("Error"); }
      }

      [AcceptVerbs(HttpVerbs.Post)]
      public ActionResult Index(FriendCash.Web.Code.MyModels.Search model)
      {
         if (this.Redirect("Index", model) == true)
          { return null; }
         else
          { return View("Error"); }
      }

      #endregion

      #region New
      public ActionResult New()
      {
         if (this.GetEditData(-1) == true)
         {
            this.PageTitle += " [new]";
            ViewData[this.Service.Fields.Entity] = new Model.Account();
            return View("Edit", ViewData[this.Service.Fields.Entity]);
         }
         else
         { return View("Error"); }
      }
      #endregion

      #region Edit

      public ActionResult Edit(long id)
      {
         if (this.GetEditData(id) == true)
          { return View(ViewData[this.Service.Fields.Entity]); }
         else
          { return View("Error"); }
       }

      [AcceptVerbs(HttpVerbs.Post)]
      public ActionResult Edit(Model.Account model)
      {
         if (this.UpdateData(model) == true && this.Redirect("Index") == true)
          { return null; }
         else
          { return View(model); }
      }

      #endregion

      #region AutoComplete
      public JsonResult AutoComplete(string term)
      {
         JsonResult oReturn = null;

         // PARAMETERS
         Service.Parameters oParameters = this.GetParameters(term);

         // SERVICE CALL
         Service.Return oServiceReturn = this.Service.GetData(oParameters);

         // CHECK RESULT
         if (this.CheckResult(oServiceReturn) == true)
         {
            List<Model.Account> oData = ((List<Model.Account>)oServiceReturn.DATA[this.Service.Fields.List]);
            oReturn = Json(oData, JsonRequestBehavior.AllowGet);
         }

         return oReturn;
      }
      #endregion

      #endregion 

   }
}
