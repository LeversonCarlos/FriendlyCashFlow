using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FriendCash.Web.Controllers
{
   public class PlanningController : MasterController
   {

      #region Initialize
      public PlanningController()
      {
         this.PageTitle = "FriendCash :: Planning";
       }
      #endregion

      #region Property

      #region Service
      private Service.Planning roService = null;
      private Service.Planning Service
      {
         get
         {
            if (this.roService == null)
            { this.roService = new Service.Planning(); }
            return this.roService;
         }
      }
      #endregion

      #endregion 

      #region Data

      #region GetIndexData
      private bool GetIndexData(Int16? page, string search)
      {
         bool bReturn = false;
         try
         {

            // PARAMETERS
            Service.Parameters oParameters = this.GetParameters(page, ref search);
            oParameters.DATA.Add("JustParents", true);

            // SERVICE CALL
            Service.Return oReturn = this.Service.GetData(oParameters);

            // CHECK RESULT
            if (this.CheckResult(oReturn) == true)
            {
               ViewData[this.Service.Fields.List] = ((List<Model.Planning>)oReturn.DATA[this.Service.Fields.List]);
               ViewData[this.Service.Fields.Search] = search;
               bReturn = true;
            }
         }
         catch (Exception ex) { ViewData["MSG"] = new List<string>() { ex.Message }; }
         return bReturn;
      }
      #endregion

      #region GetEditData
      private bool  GetEditData(long id, Model.Document.enType iType)
      {
         bool bReturn = false;
         try
         {

            // PARAMETERS
            Service.Parameters oParameters = this.GetParameters();
            oParameters.DATA.Add(this.Service.Fields.Key, id);
            oParameters.DATA.Add("BringPlanningTree", true);
            if (iType != Model.Document.enType.None) { oParameters.DATA.Add("Type", iType); }

            // SERVICE CALL
            Service.Return oReturn = this.Service.GetData(oParameters);

            // CHECK RESULT
            if (this.CheckResult(oReturn) == true)
            {
               ViewData[this.Service.Fields.Entity] = ((List<Model.Planning>)oReturn.DATA[this.Service.Fields.List]).FirstOrDefault();
               if (ViewData[this.Service.Fields.Entity] != null)
                { this.PageTitle += " [" + ((Model.Planning)ViewData[this.Service.Fields.Entity]).Description + "]"; }
               ViewData["PlanningTree"] = ((List<Model.Planning>)oReturn.DATA["PlanningTree"]);
               bReturn = true;
            }
         }
         catch (Exception ex) { ViewData["MSG"] = new List<string>() { ex.Message }; }
         return bReturn;
      }
      #endregion

      #region UpdateData
      public bool UpdateData(Model.Planning oModel)
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
      public ActionResult Index(FriendCash.Web.Code.MyModels.Search model)
      {
         if (this.Redirect("Index", model) == true)
          { return null; }
         else
          { return View("Error"); }
      }

      #endregion

      #region New

      public ActionResult NewExpense()
      { return this.New(Model.Document.enType.Expense); }

      public ActionResult NewIncome()
      { return this.New(Model.Document.enType.Income); }

      private ActionResult New(Model.Document.enType iType)
      {
         if (this.GetEditData(-1, iType) == true)
         {
            this.PageTitle += " [new]";
            ViewData[this.Service.Fields.Entity] = new Model.Planning() { Type = iType };
            return View("Edit", ViewData[this.Service.Fields.Entity]);
         }
         else
         { return View("Error"); }
      }

      #endregion

      #region Edit

      public ActionResult Edit(long id)
      {
         if (this.GetEditData(id, Model.Document.enType.None) == true)
          { return View(ViewData[this.Service.Fields.Entity]); }
         else
          { return View("Error"); }
      }

      [AcceptVerbs(HttpVerbs.Post)]
      public ActionResult Edit(Model.Planning model)
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
            List<Model.Planning> oData = ((List<Model.Planning>)oServiceReturn.DATA[this.Service.Fields.List]);
            oReturn = Json(oData, JsonRequestBehavior.AllowGet);
         }

         return oReturn;
      }
      #endregion

      #endregion 

   }
}
