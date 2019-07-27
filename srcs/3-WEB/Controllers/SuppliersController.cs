using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FriendCash.Web.Controllers
{
   [Authorize]
   public class SuppliersController : MasterController
   {

      #region New
      public SuppliersController()
      { this.PageTitle = Resources.Suppliers.PAGE_TITLE; }
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
            var oReturn = Service.Supplier.Index(oParameters);

            // CHECK RESULT
            if (this.CheckResult(oReturn) == true)
            {
               var oList = ((List<Model.Supplier>)oReturn.DATA[Service.Supplier.TAG_ENTITY_LIST]);
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
            var oReturn = Service.Supplier.Create(oParameters);

            // CHECK RESULT
            if (this.CheckResult(oReturn) == true)
            {
               var oEntity = ((Model.Supplier)oReturn.DATA[Service.Supplier.TAG_ENTITY]);
               this.PageSubTitle = "[new]";
               oResult = View("Edit", oEntity);
            }

         }
         catch (Exception ex) { this.AddMessageException(ex.Message); }
         finally { if (oResult == null) { oResult = View("Error"); } }

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
            oParameters.DATA.Add(Service.Supplier.TAG_ENTITY_KEY, id);

            // SERVICE CALL
            var oReturn = Service.Supplier.Edit(oParameters);

            // CHECK RESULT
            if (this.CheckResult(oReturn) == true)
            {
               var oEntity = ((Model.Supplier)oReturn.DATA[Service.Supplier.TAG_ENTITY]);
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
      public ActionResult Edit(Model.Supplier model)
      {
         var oReturn = new Model.Tools.Package();

         try
         {

            // PARAMETERS
            var oParameters = this.GetParameters();
            oParameters.DATA.Add(Service.Supplier.TAG_ENTITY, model);

            // SERVICE
            oReturn = Service.Supplier.SaveEdit(oParameters);

         }
         catch (Exception ex) { oReturn.MSG.Add(new Model.Tools.Message() { Exception = ex.Message }); }

         return this.GetJson(oReturn, Url.Action("Index"));
      }
      #endregion

      #region Remove
      [AcceptVerbs(HttpVerbs.Post)]
      [ValidateAntiForgeryToken]
      public JsonResult Remove(Model.Supplier model)
      {
         var oReturn = new Model.Tools.Package();

         try
         {

            // PARAMETERS
            var oParameters = this.GetParameters();
            oParameters.DATA.Add(Service.Supplier.TAG_ENTITY, model);

            // SERVICE
            oReturn = Service.Supplier.SaveRemove(oParameters);


         }
         catch (Exception ex) { oReturn.MSG.Add(new Model.Tools.Message() { Exception = ex.Message }); }

         return this.GetJson(oReturn, Url.Action("Index"));
      }
      #endregion

      #region AutoComplete
      public JsonResult AutoComplete(string term)
      {
         JsonResult oReturn = null;

         try
         {

            // PARAMETERS
            var oParameters = this.GetParameters(term);

            // SERVICE CALL
            var oPackage = Service.Supplier.AutoComplete(oParameters);

            // CHECK RESULT
            if (this.CheckResult(oPackage) == true)
            {
               var oData = ((List<Model.Tools.AutoCompleteData>)oPackage.DATA[Model.Tools.AutoCompleteData.TAG_LIST_NAME]);
               oReturn = Json(oData, JsonRequestBehavior.AllowGet);
            }
         }
         catch { }

         return oReturn;
      }
      #endregion

   }
 }
