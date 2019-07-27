using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace FriendCash.Web.Controllers
{
   [Authorize]
   public class ToolsController : MasterController
   {

      #region New
      public ToolsController()
      {
         this.PageTitle = "Tools";
      }
      #endregion

      #region Constants
      private const string TAG_STREAM_FILE = "STREAM_FILE";
      #endregion


      #region Import

      #region Get

      public ActionResult Import()
      {
         if (this.Import_GetData() == true)
         { return View(ViewData[Service.Import.TAG_ENTITY_LIST]); }
         else
         { return View("Error"); }
       }

      private bool Import_GetData()
      {
         bool bReturn = false;
         try
         {

            // PARAMETERS
            var oParameters = this.GetParameters();

            // SERVICE CALL
            var oReturn = Service.Import.GetDataGrouped(oParameters);

            // CHECK RESULT
            if (this.CheckResult(oReturn) == true)
            {
               ViewData[Service.Import.TAG_ENTITY_LIST] = oReturn.DATA[Service.Import.TAG_ENTITY_LIST];
               ViewData["ImportExecuting"] = oReturn.DATA["ImportExecuting"];
               bReturn = true;
            }
         }
         catch (Exception ex) { this.AddMessageException(ex.Message); }
         return bReturn;
      }

      #endregion

      #region Post

      [AcceptVerbs(HttpVerbs.Post)]
      [ValidateAntiForgeryToken]
      public ActionResult Import(FormCollection collection)
      {
         try
         {
            if (Request.Files.Count > 0 && Request.Files[0] != null && Request.Files[0].ContentLength > 0)
            {
               string sFilePath = "";
               if (this.Import_Upload(((HttpPostedFileBase)Request.Files[0]).InputStream, ref sFilePath) == true)
               {
                  if (this.Import_UpdateData(sFilePath) == true) 
                  {
                     System.Threading.Thread.Sleep(500);
                     return RedirectToAction("Import");
                  }
               }
            }
         }
         catch { throw; }
         return View("Error"); 
      }

      private bool Import_Upload(System.IO.Stream oStream, ref string sFilePath)
      {
         bool bReturn = false;

         try
         {

            // DEFINE TEMPORARY FILE
            sFilePath = "~/TEMP/STREAM";
            sFilePath = Server.MapPath(sFilePath);
            if (!System.IO.Directory.Exists(sFilePath)) { System.IO.Directory.CreateDirectory(sFilePath); }
            sFilePath += "\\";
            sFilePath += System.IO.Path.GetRandomFileName();

            // SAVE STREAM
            using (var oFile = System.IO.File.Create(sFilePath))
            {
               oStream.CopyTo(oFile);
             }
            bReturn = true;

         }
         catch (Exception ex) { this.AddMessageException(ex.Message); }

         return bReturn;
      }

      private bool Import_UpdateData(string sFilePath)
      {
         bool bReturn = false;

         try
         {

            // PARAMETERS
            var oParameters = this.GetParameters();
            oParameters.DATA.Add(TAG_STREAM_FILE, sFilePath);

            // SERVICE
            var oReturn = Service.Import.Start(oParameters);

            // CHECK RESULT
            if (this.CheckResult(oReturn) == true)
            {
               ViewData[Service.Import.TAG_ENTITY_KEY] = ((long)oReturn.DATA[Service.Import.TAG_ENTITY_KEY]);
               bReturn = true;
             }

         }
         catch (Exception ex) { this.AddMessageException(ex.Message); }

         return bReturn;
      }

      #endregion

      #endregion

      #region ImportProgressStep
      public JsonResult ImportProgressStep(long id)
      {
         JsonResult oResult = null;

         try
         {
            // PARAMETERS
            var oParameters = this.GetParameters();
            oParameters.DATA.Add(Service.Import.TAG_ENTITY_KEY, id);

            // SERVICE CALL
            var oReturn = Service.Import.GetDataGrouped(oParameters);

            // CHECK RESULT
            if (this.CheckResult(oReturn) == true)
            {
               var oData = ((List<Model.ViewImport>)oReturn.DATA[Service.Import.TAG_ENTITY_LIST]);
               oResult = Json(oData.SingleOrDefault(), JsonRequestBehavior.AllowGet);
            }
         }
         catch { throw; }

         return oResult;
      }
      #endregion

   }
}
