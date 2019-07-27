using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FriendCash.Web.Controllers
{
   [Authorize]
   public class TransfersController : MasterController
   {

      #region New
      public TransfersController()
      { this.PageTitle = Resources.Transfer.PAGE_TITLE; }
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
            var oReturn = Service.Transfer.Index(oParameters);

            // CHECK RESULT
            if (this.CheckResult(oReturn) == true)
            {
               var oList = ((List<Model.Transfer>)oReturn.DATA[Service.Transfer.TAG_ENTITY_LIST]);
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
            var oReturn = Service.Transfer.Create(oParameters);

            // CHECK RESULT
            if (this.CheckResult(oReturn) == true)
            {
               var oEntity = ((Model.Transfer)oReturn.DATA[Service.Transfer.TAG_ENTITY]);
               this.PageSubTitle = "[new]";
               oResult = View("Edit", oEntity);
            }
         }
         catch (Exception ex) { this.AddMessageException(ex.Message); }
         finally { if (oResult == null) { oResult = View("Edit"); } }

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
            oParameters.DATA.Add(Service.Transfer.TAG_ENTITY_KEY, id);

            // SERVICE CALL
            var oReturn = Service.Transfer.Edit(oParameters);

            // CHECK RESULT
            if (this.CheckResult(oReturn) == true)
            {
               var oEntity = ((Model.Transfer)oReturn.DATA[Service.Transfer.TAG_ENTITY]);
               if (oEntity != null && oEntity.DocumentDetails != null) { this.PageSubTitle = "[" + oEntity.DocumentDetails.Description + "]"; }
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
      public JsonResult Edit(Model.Transfer model)
      {
         var oReturn = new Model.Tools.Package();

         try
         {

            // PARAMETERS
            var oParameters = this.GetParameters();
            oParameters.DATA.Add(Service.Transfer.TAG_ENTITY, model);

            // SERVICE
            oReturn = Service.Transfer.SaveEdit(oParameters);

         }
         catch (Exception ex) { oReturn.MSG.Add(new Model.Tools.Message() { Exception = ex.Message }); }

         return this.GetJson(oReturn, Url.Action("Index"));
      }
      #endregion 

      #region Remove
      [AcceptVerbs(HttpVerbs.Post)]
      [ValidateAntiForgeryToken]
      public JsonResult Remove(Model.Transfer model)
      {
         var oReturn = new Model.Tools.Package();

         try
         {

            // PARAMETERS
            var oParameters = this.GetParameters();
            oParameters.DATA.Add(Service.Transfer.TAG_ENTITY, model);

            // SERVICE
            oReturn = Service.Transfer.SaveRemove(oParameters);

         }
         catch (Exception ex) { oReturn.MSG.Add(new Model.Tools.Message() { Exception = ex.Message }); }

         return this.GetJson(oReturn, Url.Action("Index"));
      }
      #endregion

   }
}
