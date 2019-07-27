using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FriendCash.Web.Controllers
{
   public partial class DocumentsController : MasterController
   {

      #region Initialize
      protected DocumentsController(FriendCash.Model.Document.enType iType)
      {
         this.Type = iType;
         this.PageTitle = "FriendCash :: " + this.Type.ToString() + "s";
       }
      #endregion

      #region Property

      #region Type
      protected FriendCash.Model.Document.enType Type { get; private set; }
      #endregion

      #region ServiceDocument
      private Service.Document roServiceDocument = null;
      private Service.Document ServiceDocument
      {
         get
         {
            if (this.roServiceDocument == null)
            { this.roServiceDocument = new Service.Document(); }
            return this.roServiceDocument;
         }
      }
      #endregion

      #endregion

      #region Data

      #region GetDocumentIndexData
      private bool GetDocumentIndexData(Int16? page, string search)
      {
         bool bReturn = false;
         try
         {

            // PARAMETERS
            Service.Parameters oParameters = this.GetParameters(page, ref search);
            oParameters.DATA.Add("Type", this.Type);

            // SERVICE CALL
            Service.Return oReturn = this.ServiceDocument.GetData(oParameters);

            // CHECK RESULT
            if (this.CheckResult(oReturn) == true)
            {
               ViewData[this.ServiceDocument.Fields.List] = ((List<Model.Document>)oReturn.DATA[this.ServiceDocument.Fields.List]);
               ViewData[this.ServiceDocument.Fields.Search] = search;
               bReturn = true;
             }

          }
         catch (Exception ex) { ViewData["MSG"] = new List<string>() { ex.Message }; }
         return bReturn;
       }
      #endregion

      #region GetDocumentEditData
      public bool GetDocumentEditData(long id)
      {
         bool bReturn = false;
         try
         {

            // PARAMETERS
            Service.Parameters oParameters = this.GetParameters();
            oParameters.DATA.Add("BringPlanningTree", true);
            oParameters.DATA.Add("Type", this.Type);
            oParameters.DATA.Add(this.ServiceDocument.Fields.Key, id);

            // SERVICE CALL
            Service.Return oReturn = this.ServiceDocument.GetData(oParameters);

            // CHECK RESULT
            if (this.CheckResult(oReturn) == true)
            {
               ViewData[this.ServiceDocument.Fields.Entity] = ((List<Model.Document>)oReturn.DATA[this.ServiceDocument.Fields.List]).FirstOrDefault();
               if (ViewData[this.ServiceDocument.Fields.Entity] != null)
                { this.PageTitle += " [" + ((Model.Document)ViewData[this.ServiceDocument.Fields.Entity]).Description + "]"; }
               ViewData["PlanningTree"] = ((List<Model.Planning>)oReturn.DATA["PlanningTree"]);
               bReturn = true;
            }
         }
         catch (Exception ex) { ViewData["MSG"] = new List<string>() { ex.Message }; }
         return bReturn;
      }
      #endregion

      #region UpdateDocumentData
      public bool UpdateDocumentData(Model.Document oModel)
      {
         bool bReturn = false;
         try
         {

            // PARAMETERS
            Service.Parameters oParameters = this.GetParameters();
            oModel.Type = this.Type;
            oParameters.DATA.Add(this.ServiceDocument.Fields.Entity, oModel);

            // SERVICE
            Service.Return oReturn = this.ServiceDocument.Update(oParameters);

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
         if (this.GetDocumentIndexData(page, search) == true)
          { return View("~/Views/Documents/Index.aspx", ViewData[this.ServiceDocument.Fields.List]); }
         else
          { return View("Error"); }
       }

      [AcceptVerbs(HttpVerbs.Post)]
      public ActionResult IndexMore(Int16? page, string search)
      {
         if (this.GetDocumentIndexData(page, search) == true)
         { return PartialView("~/Views/Documents/List.ascx", ViewData[this.ServiceDocument.Fields.List]); }
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
         if (this.GetDocumentEditData(-1) == true)
         {
            this.PageTitle += " [new]";
            ViewData[this.ServiceDocument.Fields.Entity] = new Model.Document();
            return View("~/Views/Documents/Edit.aspx", ViewData[this.ServiceDocument.Fields.Entity]);
          }
         else
         { return View("Error"); }
       }
      #endregion

      #region Edit

      public ActionResult Edit(long id)
      {
         if (this.GetDocumentEditData(id) == true)
         { return View("~/Views/Documents/Edit.aspx", ViewData[this.ServiceDocument.Fields.Entity]); }
         else
          { return View("Error"); }
      }

      [AcceptVerbs(HttpVerbs.Post)]
      public ActionResult Edit(Model.Document model)
      {
         if (this.UpdateDocumentData(model) == true && this.Redirect("Index") == true)
          { return null; }
         else
          { return View("~/Views/Documents/Edit.aspx", model); }
       }

      #endregion

      #region AutoComplete
      public JsonResult AutoComplete(string term)
      {
         JsonResult oReturn = null;

         // PARAMETERS
         Service.Parameters oParameters = this.GetParameters(term);
         oParameters.DATA.Add("Type", this.Type);

         // SERVICE CALL
         Service.Return oServiceReturn = this.ServiceDocument.GetData(oParameters);

         // CHECK RESULT
         if (this.CheckResult(oServiceReturn) == true)
         {
            List<Model.Document> oData = ((List<Model.Document>)oServiceReturn.DATA[this.ServiceDocument.Fields.List]);
            oReturn = Json(oData, JsonRequestBehavior.AllowGet);
          }

         return oReturn;
      }
      #endregion

      #endregion

   }
 }
