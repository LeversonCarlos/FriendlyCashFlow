using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FriendCash.Web.Controllers
{
   [Authorize]
   public class PlanningController : MasterController
   {

      #region New
      public PlanningController()
      { this.PageTitle = Resources.Planning.PAGE_TITLE; }
      #endregion


      #region Index
      public ActionResult Index(Int16? page, string search)
      {
         ActionResult oResult = null;

         try
         {

            // PARAMETERS
            var oParameters = this.GetParameters(page, ref search);
            oParameters.DATA.Add(Service.Planning.TAG_ENTITY_JUST_PARENT, true);

            // SERVICE CALL
            var oReturn = Service.Planning.Index(oParameters);

            // CHECK RESULT
            if (this.CheckResult(oReturn) == true)
            {
               var oList = ((List<Model.Planning>)oReturn.DATA[Service.Planning.TAG_ENTITY_LIST]);
               oResult = View(oList);
            }
         }
         catch (Exception ex) { this.AddMessageException(ex.Message); }
         finally { if (oResult == null) { oResult = View(); } }

         return oResult;
      }
      #endregion

      #region Create

      public ActionResult NewExpense()
      { return this.New(Model.Document.enType.Expense); }

      public ActionResult NewIncome()
      { return this.New(Model.Document.enType.Income); }

      private ActionResult New(Model.Document.enType iType)
      {
         ActionResult oResult = null;

         try
         {

            // PARAMETERS
            var oParameters = this.GetParameters();
            if (iType != Model.Document.enType.None) { oParameters.DATA.Add(Service.Planning.TAG_ENTITY_TYPE, iType); }

            // SERVICE CALL
            var oReturn = Service.Planning.Create(oParameters);

            // CHECK RESULT
            if (this.CheckResult(oReturn) == true)
            {
               var oEntity = ((Model.Planning)oReturn.DATA[Service.Planning.TAG_ENTITY]);
               ViewData["PlanningTree"] = ((List<Model.Planning>)oReturn.DATA["PlanningTree"]);
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
            oParameters.DATA.Add(Service.Planning.TAG_ENTITY_KEY, id);
            oParameters.DATA.Add(Service.Planning.TAG_ENTITY_TYPE, Model.Document.enType.None); // WILL GET FROM MODEL

            // SERVICE CALL
            var oReturn = Service.Planning.Edit(oParameters);

            // CHECK RESULT
            if (this.CheckResult(oReturn) == true)
            {
               var oEntity = ((Model.Planning)oReturn.DATA[Service.Planning.TAG_ENTITY]);
               ViewData["PlanningTree"] = ((List<Model.Planning>)oReturn.DATA["PlanningTree"]);
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
      public JsonResult Edit(Model.Planning model)
      {
         var oReturn = new Model.Tools.Package();

         try
         {

            // PARAMETERS
            var oParameters = this.GetParameters();
            oParameters.DATA.Add(Service.Planning.TAG_ENTITY, model);
            oParameters.DATA.Add(Service.Planning.TAG_ENTITY_TYPE, model.Type);

            // SERVICE
            oReturn = Service.Planning.SaveEdit(oParameters);
            //ViewData["PlanningTree"] = ((List<Model.Planning>)oReturn.DATA["PlanningTree"]); 

         }
         catch (Exception ex) { oReturn.MSG.Add(new Model.Tools.Message() { Exception = ex.Message }); }

         return this.GetJson(oReturn, Url.Action("Index"));
      }
      #endregion 

      #region Remove
      [AcceptVerbs(HttpVerbs.Post)]
      [ValidateAntiForgeryToken]
      public JsonResult Remove(Model.Planning model)
      {
         var oReturn = new FriendCash.Model.Tools.Package();

         try
         {

            // PARAMETERS
            var oParameters = this.GetParameters();
            oParameters.DATA.Add(Service.Planning.TAG_ENTITY, model);

            // SERVICE
            oReturn = Service.Planning.SaveRemove(oParameters);

         }
         catch (Exception ex) { oReturn.MSG.Add(new Model.Tools.Message() { Exception = ex.Message }); }

         return this.GetJson(oReturn, Url.Action("Index"));
      }
      #endregion

   }
}
