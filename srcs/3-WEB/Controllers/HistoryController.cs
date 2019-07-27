using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FriendCash.Web.Controllers
{
   public partial class DocumentsController : MasterController
   {

      #region Property

      #region ServiceHistory
      private Service.History roServiceHistory = null;
      private Service.History ServiceHistory
      {
         get
         {
            if (this.roServiceHistory == null)
            { this.roServiceHistory = new Service.History(); }
            return this.roServiceHistory;
         }
      }
      #endregion

      #endregion 

      #region Data

      #region GetHistoryIndexData
      private bool GetHistoryIndexData(long id, Int16? page)
      {
         bool bReturn = false;
         try
         {

            // PARAMETERS
            Service.Parameters oParameters = this.GetParameters(page);
            oParameters.DATA.Add("idDocument", id);
            oParameters.DATA.Add("Type", this.Type);
            oParameters.DATA.Add("BringDocument", true);

            // SERVICE CALL
            Service.Return oReturn = this.ServiceHistory.GetData(oParameters);

            // CHECK RESULT
            if (this.CheckResult(oReturn) == true)
            {
               ViewData[this.ServiceHistory.Fields.List] = ((List<Model.History>)oReturn.DATA[this.ServiceHistory.Fields.List]);
               if (oReturn.DATA.ContainsKey("Documents") && oReturn.DATA["Documents"] != null)
                { this.PageTitle += " [" + ((FriendCash.Model.Document)oReturn.DATA["Documents"]).Description + "]"; }
               bReturn = true;
             }

         }
         catch (Exception ex) { ViewData["MSG"] = new List<string>() { ex.Message }; }
         return bReturn;
      }
      #endregion

      #region GetHistoryEditData
      public bool GetHistoryEditData(long idHistory, long idDocument)
      {
         bool bReturn = false;
         try
         {

            // PARAMETERS
            Service.Parameters oParameters = this.GetParameters();
            oParameters.DATA.Add("Type", this.Type);
            oParameters.DATA.Add("BringDocument", true);
            oParameters.DATA.Add("idDocument", idDocument);
            oParameters.DATA.Add(this.ServiceHistory.Fields.Key, idHistory);

            // SERVICE CALL
            Service.Return oReturn = this.ServiceHistory.GetData(oParameters);

            // CHECK RESULT
            if (this.CheckResult(oReturn) == true)
            {
               ViewData[this.ServiceHistory.Fields.Entity] = ((List<Model.History>)oReturn.DATA[this.ServiceHistory.Fields.List]).FirstOrDefault();
               if (oReturn.DATA.ContainsKey("Documents") && oReturn.DATA["Documents"] != null)
                { this.PageTitle += " [" + ((FriendCash.Model.Document)oReturn.DATA["Documents"]).Description + "]"; }
               if (ViewData[this.ServiceHistory.Fields.Entity] != null)
                { this.PageTitle += " [" + ((Model.History)ViewData[this.ServiceHistory.Fields.Entity]).idHistory + "]"; }
               bReturn = true;
            }
         }
         catch (Exception ex) { ViewData["MSG"] = new List<string>() { ex.Message }; }
         return bReturn;
      }
      #endregion

      #region UpdateHistoryData
      public bool UpdateHistoryData(Model.History oModel)
      {
         bool bReturn = false;
         try
         {

            // PARAMETERS
            Service.Parameters oParameters = this.GetParameters();
            oParameters.DATA.Add(this.ServiceHistory.Fields.Entity, oModel);

            // SERVICE
            Service.Return oReturn = this.ServiceHistory.Update(oParameters);

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

      #region History

      public ActionResult History(long id, Int16? page)
      {
         if (this.GetHistoryIndexData(id, page) == true)
          { return View("~/Views/History/Index.aspx", ViewData[this.ServiceHistory.Fields.List]); }
         else
          { return View("Error"); }
      }

      [AcceptVerbs(HttpVerbs.Post)]
      public ActionResult HistoryMore(long id, Int16? page)
      {
         if (this.GetHistoryIndexData(id, page) == true)
          { return PartialView("~/Views/History/List.ascx", ViewData[this.ServiceHistory.Fields.List]); }
         else
          { return View("Error"); }
      }

      #endregion

      #region NewHistory
      public ActionResult NewHistory(long? id)
      {
         long iID = (id.HasValue == true ? id.Value : -1);
         if (this.GetHistoryEditData(-1, iID) == true)
         {
            this.PageTitle += " [new]";
            ViewData[this.ServiceHistory.Fields.Entity] = new Model.History() { Type = this.Type, idDocument = iID, DueDate = DateTime.Now };
            return View("~/Views/History/Edit.aspx", ViewData[this.ServiceHistory.Fields.Entity]);
         }
         else
         { return View("Error"); }
      }
      #endregion

      #region Edit

      public ActionResult EditHistory(long id)
      {
         if (this.GetHistoryEditData(id, 0) == true)
          { return View("~/Views/History/Edit.aspx", ViewData[this.ServiceHistory.Fields.Entity]); }
         else
          { return View("Error"); }
      }

      [AcceptVerbs(HttpVerbs.Post)]
      public ActionResult EditHistory(Model.History model)
      {
         if (this.UpdateHistoryData(model) == true && this.Redirect("History", new { id = model.idDocument }) == true)
          { return null; }
         else
         { return View("~/Views/History/Edit.aspx", model); }
      }

      #endregion

      #endregion 

   }
}
