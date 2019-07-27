using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FriendCash.Web.Controllers
{
   [Authorize]
   public class RecurrentController : MasterController
   {

      #region New
      public RecurrentController()
      { this.PageTitle = Resources.Recurrent.PAGE_TITLE; }
      #endregion


      #region Index
      public ActionResult Index()
      {
         ActionResult oResult = null;

         try
         {

            // PARAMETERS
            var oParameters = this.GetParameters();

            // SERVICE CALL
            var oReturn = Service.Recurrent.Index(oParameters);

            // CHECK RESULT
            if (this.CheckResult(oReturn) == true)
            {
               var oList = ((List<Model.Recurrent>)oReturn.DATA[Service.Recurrent.TAG_ENTITY_LIST]);
               oResult = View(oList);
            }
         }
         catch (Exception ex) { this.AddMessageException(ex.Message); }
         finally { if (oResult == null) { oResult = View(); } }

         return oResult;
      }
      #endregion

      #region Create

      public ActionResult NewIncome()
      { return this.New(Model.Document.enType.Income ); }

      public ActionResult NewExpense()
      { return this.New(Model.Document.enType.Expense); }

      private ActionResult New(FriendCash.Model.Document.enType iType)
      {
         ActionResult oResult = null;

         try
         {

            // PARAMETERS
            var oParameters = this.GetParameters();
            oParameters.DATA.Add("Type", ((long)iType));

            // SERVICE CALL
            var oReturn = Service.Recurrent.Create(oParameters);

            // CHECK RESULT
            if (this.CheckResult(oReturn) == true)
            {
               var oEntity = ((Model.Recurrent)oReturn.DATA[Service.Recurrent.TAG_ENTITY]);
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
            oParameters.DATA.Add(Service.Recurrent.TAG_ENTITY_KEY, id);

            // SERVICE CALL
            var oReturn = Service.Recurrent.Edit(oParameters);

            // CHECK RESULT
            if (this.CheckResult(oReturn) == true)
            {
               var oEntity = ((Model.Recurrent)oReturn.DATA[Service.Recurrent.TAG_ENTITY]);
               if (oEntity != null) { this.PageSubTitle = "[" + oEntity.DocumentDetails.Description + "]"; }
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
      public JsonResult Edit(Model.Recurrent model)
      {
         var oReturn = new Model.Tools.Package();

         try
         {

            // PARAMETERS
            var oParameters = this.GetParameters();
            oParameters.DATA.Add(Service.Recurrent.TAG_ENTITY, model);

            // SERVICE
            oReturn = Service.Recurrent.SaveEdit(oParameters);

         }
         catch (Exception ex) { oReturn.MSG.Add(new Model.Tools.Message() { Exception = ex.Message }); }

         return this.GetJson(oReturn, Url.Action("Index"));
      }
      #endregion 

      #region Remove
      [AcceptVerbs(HttpVerbs.Post)]
      [ValidateAntiForgeryToken]
      public JsonResult Remove(Model.Recurrent model)
      {
         var oReturn = new Model.Tools.Package();

         try
         {

            // PARAMETERS
            var oParameters = this.GetParameters();
            oParameters.DATA.Add(Service.Recurrent.TAG_ENTITY, model);

            // SERVICE
            oReturn = Service.Recurrent.SaveRemove(oParameters);

         }
         catch (Exception ex) { oReturn.MSG.Add(new Model.Tools.Message() { Exception = ex.Message }); }

         return this.GetJson(oReturn, Url.Action("Index"));
      }
      #endregion

   }
}
