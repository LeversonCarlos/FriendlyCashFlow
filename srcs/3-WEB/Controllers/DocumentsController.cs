using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FriendCash.Web.Controllers
{
   [Authorize]
   public partial class DocumentsController : MasterController
   {

      #region New
      protected DocumentsController(FriendCash.Model.Document.enType iType)
      {
         this.Type = iType;
         ViewBag.PageController = this.Type.ToString() + "s";
         if (this.Type == Model.Document.enType.Expense) { ViewBag.PageTitle = Resources.Document.ENUM_TYPE_EXPENSE; }
         else if (this.Type == Model.Document.enType.Income) { ViewBag.PageTitle = Resources.Document.ENUM_TYPE_INCOME; }
       }
      protected FriendCash.Model.Document.enType Type { get; private set; }
      #endregion


      #region Index

      public ActionResult Index(Int16? page, string search)
      {
         ActionResult oResult = null;

         try
         {

            // PARAMETERS
            var oParameters = this.GetParameters(page, ref search, true);
            oParameters.DATA.Add(Service.Document.TAG_ENTITY_TYPE, this.Type);

            // SERVICE CALL
            var oReturn = Service.Document.Index(oParameters);

            // CHECK RESULT
            if (this.CheckResult(oReturn) == true)
            {
               var oList = ((List<Model.Document>)oReturn.DATA[Service.Document.TAG_ENTITY_LIST]);
               oResult = View("~/Views/Documents/Index.cshtml", oList);
            }

         }
         catch (Exception ex) { this.AddMessageException(ex.Message); }
         finally { if (oResult == null) { oResult = View("~/Views/Documents/Index.cshtml"); } }

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
            oParameters.DATA.Add(Service.Document.TAG_ENTITY_TYPE, this.Type);

            // SERVICE CALL
            var oReturn = Service.Document.Create(oParameters);

            // CHECK RESULT
            if (this.CheckResult(oReturn) == true)
            {
               var oEntity = ((Model.Document)oReturn.DATA[Service.Document.TAG_ENTITY]);
               ViewData["PlanningTree"] = ((List<Model.Planning>)oReturn.DATA["PlanningTree"]);
               this.PageSubTitle = "[new]";
               oResult = View("~/Views/Documents/Edit.cshtml", oEntity);
            }
         }
         catch (Exception ex) { this.AddMessageException(ex.Message); }
         finally { if (oResult == null) { oResult = View("~/Views/Documents/Edit.cshtml"); } }

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
            oParameters.DATA.Add(Service.Document.TAG_ENTITY_TYPE, this.Type);
            oParameters.DATA.Add(Service.Document.TAG_ENTITY_KEY, id);

            // SERVICE CALL
            var oReturn = Service.Document.Edit(oParameters);

            // CHECK RESULT
            if (this.CheckResult(oReturn) == true)
            {
               var oEntity = ((Model.Document)oReturn.DATA[Service.Document.TAG_ENTITY]);
               ViewData["PlanningTree"] = ((List<Model.Planning>)oReturn.DATA["PlanningTree"]);
               if (oEntity != null) { this.PageSubTitle = "[" + oEntity.Description + "]"; }
               oResult = View("~/Views/Documents/Edit.cshtml", oEntity);
            }
         }
         catch (Exception ex) { this.AddMessageException(ex.Message); }
         finally { if (oResult == null) { oResult = View("~/Views/Documents/Edit.cshtml"); } }

         return oResult;
      }
      #endregion

      #region Save
      [AcceptVerbs(HttpVerbs.Post)]
      [ValidateAntiForgeryToken]
      public JsonResult Edit(Model.Document model)
      {
         var oReturn = new Model.Tools.Package();

         try
         {

            // PARAMETERS
            var oParameters = this.GetParameters();
            oParameters.DATA.Add(Service.Document.TAG_ENTITY, model);

            // SERVICE
            oReturn = Service.Document.SaveEdit(oParameters);
            //ViewData["PlanningTree"] = ((List<Model.Planning>)oReturn.DATA["PlanningTree"]);

         }
         catch (Exception ex) { oReturn.MSG.Add(new Model.Tools.Message() { Exception = ex.Message }); }

         return this.GetJson(oReturn, Url.Action("Index"));
      }
      #endregion

      #region Remove
      [AcceptVerbs(HttpVerbs.Post)]
      [ValidateAntiForgeryToken]
      public JsonResult Remove(Model.Document oModel)
      {
         var oReturn = new Model.Tools.Package();

         try
         {

            // PARAMETERS
            var oParameters = this.GetParameters();
            oParameters.DATA.Add(Service.Document.TAG_ENTITY, oModel);

            // SERVICE
            oReturn = Service.Document.SaveRemove(oParameters);

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
         oParameters.DATA.Add(Service.Document.TAG_ENTITY_TYPE, this.Type);

         // SERVICE CALL
         var oServiceReturn = Service.Document.AutoComplete(oParameters);

         // CHECK RESULT
         if (this.CheckResult(oServiceReturn) == true)
         {
            var oData = ((List<Model.Tools.AutoCompleteData>)oServiceReturn.DATA[Model.Tools.AutoCompleteData.TAG_LIST_NAME]);
            oReturn = Json(oData, JsonRequestBehavior.AllowGet);
          }

         return oReturn;
      }
      #endregion

      #region Indicators
      public PartialViewResult Indicators(long id)
      {
         PartialViewResult oResult = null;

         try
         {

            // PARAMETERS
            var oParameters = this.GetParameters();
            oParameters.DATA.Add(Service.Document.TAG_ENTITY_TYPE, this.Type);
            oParameters.DATA.Add(Service.Document.TAG_ENTITY_KEY, id);

            // SERVICE CALL
            var oReturn = Service.Document.GetIndicators(oParameters);

            // CHECK RESULT
            if (this.CheckResult(oReturn) == true)
            {
               var oEntity = ((Model.DocumentIndicators)oReturn.DATA[Service.Document.TAG_ENTITY_INDICATORS]);
               oResult = PartialView("~/Views/Documents/Indicators.cshtml", oEntity);
            }
         }
         catch (Exception ex) { this.AddMessageException(ex.Message); }
         finally { if (oResult == null) { oResult = PartialView("~/Views/Documents/Indicators.cshtml"); } }

         return oResult;
      }
      #endregion

   }
 }
