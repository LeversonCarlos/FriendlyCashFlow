using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FriendCash.Web.Controllers
{
   [Authorize]
   public class AccountsController : MasterController
   {

      #region New
      public AccountsController()
      { this.PageTitle = Resources.Accounts.PAGE_TITLE; }
      #endregion


      #region Index

      public ActionResult Index(Int16? page, string search)
      {
         ActionResult oResult = null;

         try
         {

            // PARAMETERS
            var oParameters = this.GetParameters(page, ref search);

            // SERVICE CALL
            var oReturn = Service.Account.Index(oParameters);

            // CHECK RESULT
            if (this.CheckResult(oReturn) == true)
            {
               var oList = ((List<Model.Account>)oReturn.DATA[Service.Account.TAG_ENTITY_LIST]);
               oResult = View(oList);
            }

         }
         catch (Exception ex) { this.AddMessageException(ex.Message); }
         finally { if (oResult == null) { oResult = View(); } }

         return oResult;
       }

      [AcceptVerbs(HttpVerbs.Post)]
      [ValidateAntiForgeryToken]
      public ActionResult Index(FriendCash.Web.Search model)
      {
         Int16? page = 1;
         string search = ""; if (model != null) { search = model.Value; }
         return this.Index(page, search); 
      }

      #endregion

      #region Create
      public ActionResult New()
      {
         ActionResult oResult = null;

         try
         {

            // PARAMETERS
            var oParameters = this.GetParameters();

            // SERVICE CALL
            var oReturn = Service.Account.Create(oParameters);

            // CHECK RESULT
            if (this.CheckResult(oReturn) == true)
            {
               var oEntity = ((Model.Account)oReturn.DATA[Service.Account.TAG_ENTITY]);
               this.PageSubTitle = "[new]";
               oResult = View("Edit", oEntity);
            }

         }
         catch (Exception ex) { this.AddMessageException(ex.Message); }
         finally { if (oResult == null) { oResult = View("Error");  } }

         return oResult;
      }
      #endregion

      #region Edit
      public ActionResult Edit(long id)
      {
         ActionResult oResult = null;

         try
         {

            // PARAMETERS
            var oParameters = this.GetParameters();
            oParameters.DATA.Add(Service.Account.TAG_ENTITY_KEY, id);

            // SERVICE CALL
            var oReturn = Service.Account.Edit(oParameters);

            // CHECK RESULT
            if (this.CheckResult(oReturn) == true)
            {
               var oEntity = ((Model.Account)oReturn.DATA[Service.Account.TAG_ENTITY]);
               if (oEntity != null) { this.PageSubTitle = "[" + oEntity.Description + "]"; }
               oResult = View(oEntity);
            }

         }
         catch (Exception ex) { this.AddMessageException(ex.Message); }
         finally { if (oResult == null) { oResult = View(); } }

         return oResult;
      }
      #endregion

      #region Save
      [AcceptVerbs(HttpVerbs.Post)]
      [ValidateAntiForgeryToken]
      public JsonResult Edit(Model.Account model)
      {
         var oReturn = new Model.Tools.Package();

         try
         {

            // PARAMETERS
            var oParameters = this.GetParameters();
            oParameters.DATA.Add(Service.Account.TAG_ENTITY, model);

            // SERVICE
            oReturn = Service.Account.SaveEdit(oParameters);

         }
         catch (Exception ex) { oReturn.MSG.Add(new Model.Tools.Message() { Exception = ex.Message }); }

         return this.GetJson(oReturn, Url.Action("Index"));
      }
      #endregion

      #region Remove
      [AcceptVerbs(HttpVerbs.Post)]
      [ValidateAntiForgeryToken]
      public JsonResult Remove(Model.Account oModel)
      {
         var oReturn = new Model.Tools.Package();

         try
         {

            // PARAMETERS
            var oParameters = this.GetParameters();
            oParameters.DATA.Add(Service.Account.TAG_ENTITY, oModel);

            // SERVICE
            oReturn = Service.Account.SaveRemove(oParameters);

         }
         catch (Exception ex) { oReturn.MSG.Add(new Model.Tools.Message() { Exception = ex.Message }); }

         return this.GetJson(oReturn, Url.Action("Index"));
      }
      #endregion

      #region AutoComplete
      public JsonResult AutoComplete(string term)
      {
         JsonResult oReturn = null;

         // PARAMETERS
         var oParameters = this.GetParameters(term);

         // SERVICE CALL
         var oPackage = Service.Account.AutoComplete(oParameters);

         // CHECK RESULT
         if (this.CheckResult(oPackage) == true)
         {
            var oData = ((List<Model.Tools.AutoCompleteData>)oPackage.DATA[Model.Tools.AutoCompleteData.TAG_LIST_NAME]);
            oReturn = Json(oData, JsonRequestBehavior.AllowGet);
         }

         return oReturn;
      }
      #endregion

      #region Details
      [AcceptVerbs(HttpVerbs.Post)]
      public JsonResult Details(long id)
      {
         var oReturn = new Model.Tools.Package();

         try
         {

            // PARAMETERS
            var oParameters = this.GetParameters();
            oParameters.DATA.Add(Service.Account.TAG_ENTITY_KEY, id);

            // SERVICE CALL
            oReturn = Service.Account.Edit(oParameters);

         }
         catch (Exception ex) { oReturn.MSG.Add(new Model.Tools.Message() { Exception = ex.Message }); }

         return this.GetJson(oReturn, true);
      }
      #endregion

   }
}
