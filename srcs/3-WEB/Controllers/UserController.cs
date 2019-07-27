using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FriendCash.Web.Controllers
{
   public class UserController : MasterController
   {

      #region Initialize
      public UserController()
      {
         this.PageTitle = "FriendCash :: User";
       }
      #endregion

      #region Property

      #region Service
      private Service.User roService = null;
      private Service.User Service
      {
         get
         {
            if (this.roService == null)
            { this.roService = new Service.User(); }
            return this.roService;
         }
      }
      #endregion

      #endregion 

      #region Data

      #region ImportData
      private bool ImportData(FormCollection oCollection)
      {
         bool bReturn = false;

         try
         {
            if (Request.Files.Count > 0 && Request.Files[0] != null && Request.Files[0].ContentLength > 0)
            {

               // PARAMETERS
               System.IO.Stream oStream = ((HttpPostedFileBase)Request.Files[0]).InputStream;
               //string sClear = oCollection["ImportClear"].ToString();
               Service.Parameters oParameters = this.GetParameters();
               oParameters.DATA.Add("STREAM", oStream);

               // SERVICE
               Service.Return oReturn = this.Service.Import(oParameters);

               // CHECK RESULT
               if (this.CheckResult(oReturn) == true)
               {
                  bReturn = true;
                }

              }
          }
         catch (Exception ex) { ViewData["MSG"] = new List<string>() { ex.Message }; }

         return bReturn;
       }
      #endregion

      #endregion

      #region Action

      #region Index

      public ActionResult Index()
      {
          return View(); 
       }

      #endregion 

      #region Import

      public ActionResult Import()
      {
         return View(); 
       }

      [AcceptVerbs(HttpVerbs.Post)]
      public ActionResult Import(FormCollection collection)
      {
         if (this.ImportData(collection) == true)
         {
            ContentResult oResult = new ContentResult();
            oResult.Content = string.Empty;
            oResult.Content += "<script type='text/javascript'>";
            //oResult.Content += "parent.import_success();";
            oResult.Content += "parent.window.location = '" + Url.Action("Index", "User") + "'";
            oResult.Content += "</script>";
            return oResult;
          }
         else
          { return View("Error"); }
       }

      #endregion

      #endregion

   }
}
