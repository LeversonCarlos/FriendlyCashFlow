using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FriendCash.Web.Controllers
{
   public partial class DocumentsController : MasterController
   {

      #region History
      public ActionResult History(long id, Int16? page)
      {
         ActionResult oResult = null;

         try
         {

            // PARAMETERS
            var oParameters = this.GetParameters(page);
            oParameters.DATA.Add(Service.Document.TAG_ENTITY_KEY, id);
            oParameters.DATA.Add(Service.Document.TAG_ENTITY_TYPE, this.Type);

            // SERVICE CALL
            var oReturn = Service.History.Index(oParameters);

            // CHECK RESULT
            if (this.CheckResult(oReturn) == true)
            {
               var oList = ((List<Model.History>)oReturn.DATA[Service.History.TAG_ENTITY_LIST]);
               var oDocument = ((Model.Document)oReturn.DATA[Service.Document.TAG_ENTITY]);
               if (oDocument != null) { this.PageTitle = oDocument.Description; }
               this.PageSubTitle = Resources.Base.LIST; 
               ViewData[FriendCash.Service.Document.TAG_ENTITY_KEY] = id;
               oResult = View("~/Views/History/Index.cshtml", oList);
            }

         }
         catch (Exception ex) { this.AddMessageException(ex.Message); }
         finally { if (oResult == null) { oResult = View("~/Views/History/Index.cshtml"); } }

         return oResult;
      }
      #endregion

      #region Create
      public PartialViewResult NewHistory(long? id, string r)
      {
         PartialViewResult oResult = null;

         try
         {

            // PARAMETERS
            long idDocument = (id.HasValue == true ? id.Value : -1);
            var oParameters = this.GetParameters();
            oParameters.DATA.Add(Service.Document.TAG_ENTITY_TYPE, this.Type);
            oParameters.DATA.Add(Service.Document.TAG_ENTITY_KEY, idDocument);
            ViewData["SuccessRedirect"] = r;

            // SERVICE CALL
            var oReturn = Service.History.Create(oParameters);

            // CHECK RESULT
            if (this.CheckResult(oReturn) == true)
            {
               var oEntity = ((Model.History)oReturn.DATA[Service.History.TAG_ENTITY]);
               var oDocument = ((Model.Document)oReturn.DATA[Service.Document.TAG_ENTITY]);
               if (oDocument != null) { this.PageTitle = oDocument.Description; }
               this.PageSubTitle = Resources.Base.NEW;
               ViewData[FriendCash.Service.Document.TAG_ENTITY_KEY] = id;
               oResult = PartialView("~/Views/History/Edit.cshtml", oEntity);
            }
         }
         catch (Exception ex) { this.AddMessageException(ex.Message); }
         finally { if (oResult == null) { oResult = PartialView("~/Views/History/Edit.cshtml"); } }

         return oResult;
      }
      #endregion

      #region Edit
      public PartialViewResult EditHistory(long id, string r)
      {
         PartialViewResult oResult = null;

         try
         {

            // PARAMETERS
            var oParameters = this.GetParameters();
            oParameters.DATA.Add(Service.Document.TAG_ENTITY_TYPE, this.Type);
            oParameters.DATA.Add(Service.History.TAG_ENTITY_KEY, id);
            ViewData["SuccessRedirect"] = r;

            // SERVICE CALL
            var oReturn = Service.History.Edit(oParameters);

            // CHECK RESULT
            if (this.CheckResult(oReturn) == true)
            {
               var oEntity = ((Model.History)oReturn.DATA[Service.History.TAG_ENTITY]);
               var oDocument = ((Model.Document)oReturn.DATA[Service.Document.TAG_ENTITY]);
               if (oDocument != null) { this.PageTitle = oDocument.Description; ViewData[FriendCash.Service.Document.TAG_ENTITY_KEY] = oDocument.idDocument; }
               if (oEntity != null) { this.PageSubTitle = oEntity.idHistory.ToString(); }
               oResult = PartialView("~/Views/History/Edit.cshtml", oEntity);
            }
         }
         catch (Exception ex) { this.AddMessageException(ex.Message); }
         finally { if (oResult == null) { oResult = PartialView("~/Views/History/Edit.cshtml"); } }

         return oResult;
      }
      #endregion

      #region Save
      [AcceptVerbs(HttpVerbs.Post)]
      [ValidateAntiForgeryToken]
      public JsonResult EditHistory(Model.History model, string SuccessRedirect)
      {
         var oReturn = new Model.Tools.Package();

         try
         {

            // SUCCESS REDIRECT
            if (string.IsNullOrEmpty(SuccessRedirect)) { SuccessRedirect = Url.Action("History", new { id = model.idDocument }); }

            // PARAMETERS
            var oParameters = this.GetParameters();
            oParameters.DATA.Add(Service.History.TAG_ENTITY, model);

            // SERVICE
            oReturn = Service.History.SaveEdit(oParameters);

         }
         catch (Exception ex) { oReturn.MSG.Add(new Model.Tools.Message() { Exception = ex.Message }); }

         return this.GetJson(oReturn, SuccessRedirect );
      }
      #endregion 

      #region Remove
      [AcceptVerbs(HttpVerbs.Post)]
      [ValidateAntiForgeryToken]
      public JsonResult RemoveHistory(Model.History oModel)
      {
         var oReturn = new Model.Tools.Package();

         try
         {

            // PARAMETERS
            var oParameters = this.GetParameters();
            oParameters.DATA.Add(Service.History.TAG_ENTITY, oModel);

            // SERVICE
            oReturn = Service.History.SaveRemove(oParameters);

         }
         catch (Exception ex) { oReturn.MSG.Add(new Model.Tools.Message() { Exception = ex.Message }); }

         return this.GetJson(oReturn, Url.Action("History", new { id = oModel.idDocument }));
      }
      #endregion

   }
}
