using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FriendCash.Web.Controllers
{
   [Authorize]
   public class FlowController : MasterController
   {

      #region New
      public FlowController()
      { this.PageTitle = Resources.Flow.PAGE_TITLE; }
      #endregion

      #region Index
      public ActionResult Index()
      {
         return View();
      }
      #endregion

      #region ResumeAccount

      public PartialViewResult ResumeAccount()
      {
         PartialViewResult oResult = null;

         try
         {

            // INITIALIZE
            ViewData[Service.Flow.TAG_ENTITY_LIST] = new List<Model.Flow>();

            // PARAMETERS
            var oParameters = this.GetParameters();
            oParameters.DATA.Add("JustFirst", true);
            oParameters.DATA.Add("idAccount", 0);

            // ACCOUNTS
            List<Model.Account> oAccounts = this.ResumeAccount_GetData_Accounts();
            if (oAccounts != null && oAccounts.Count > 0)
            {
               foreach (var oAccount in oAccounts)
               {

                  // PARAMETER
                  oParameters.DATA["idAccount"] = oAccount.idAccount;
                  if (oAccount.Type == Model.Account.enType.CreditCard) { oParameters.DATA["FinishDate"] = oAccount.DueDate; }

                  // SERVICE CALL
                  var oReturn = Service.Flow.GetDataBalance(oParameters);

                  // CHECK RESULT
                  if (this.CheckResult(oReturn) == true)
                  {
                     var oList = ((List<Model.Flow>)oReturn.DATA[Service.Flow.TAG_ENTITY_LIST]);
                     var oEnumerable = oList.AsEnumerable();
                     ((List<Model.Flow>)ViewData[Service.Flow.TAG_ENTITY_LIST]).AddRange(oEnumerable);
                  }

               }
            }
            oResult = PartialView("Index_ResumeAccount");

         }
         catch (Exception ex) { this.AddMessageException(ex.Message); }
         finally { if (oResult == null) { oResult = PartialView("Error"); } }

         return oResult;
      }

      private List<Model.Account> ResumeAccount_GetData_Accounts()
      {
         List<Model.Account> oList = null;

         // SERVICE CALL
         var oParameters = this.GetParameters();
         oParameters.DATA.Add("JustEnabled", true);
         var oReturn = Service.Account.Index(oParameters);

         // CHECK RESULT
         if (this.CheckResult(oReturn) == true)
         {
            oList = ((List<Model.Account>)oReturn.DATA[Service.Account.TAG_ENTITY_LIST]);
            ViewData[Service.Account.TAG_ENTITY_LIST] = oList;
         }

         return oList;
      }

      #endregion

      #region ResumeParcels
      public PartialViewResult ResumeParcels()
      {
         PartialViewResult oResult = null;

         try
         {

            // INITIALIZE
            var oParameters = this.GetParameters();

            // SERVICE CALL
            var oReturn = Service.Flow.GetDataParcels(oParameters);

            // CHECK RESULT
            if (this.CheckResult(oReturn) == true)
            {
               var oList = ((List<Model.Flow>)oReturn.DATA[Service.Flow.TAG_ENTITY_LIST]);
               ViewData[Service.Flow.TAG_ENTITY_LIST] = oList;
               oResult = PartialView("Index_ResumeParcels");
            }

         }
         catch (Exception ex) { this.AddMessageException(ex.Message); }
         finally { if (oResult == null) { oResult = PartialView("Error"); } }

         return oResult;
      }
      #endregion

      #region ResumePlanning

      #region ResumePlanningModel
      public class ResumePlanningModel
      {
         public ResumePlanningModel() { }
         public ResumePlanningModel(DateTime dDate) { this.DateSet(dDate); }
         #region Date
         public DateTime DateGet()  { return new DateTime(this.DateY, this.DateM, this.DateD); }
         public void DateSet(DateTime value) {this.DateY = value.Year; this.DateM = value.Month; this.DateD = value.Day; }
         public int DateY { get; set; }
         public int DateM { get; set; }
         public int DateD { get; set; }
         #endregion
         public Model.Document.enType Type { get; set; }
      }
      #endregion

      public PartialViewResult ResumePlanning(short id)
      {
         return this.ResumePlanning(new ResumePlanningModel(DateTime.Now) { Type = (Model.Document.enType)id });
       }

      [HttpPost]
      public PartialViewResult ResumePlanning(ResumePlanningModel oModel)
      {
         PartialViewResult oResult = null;

         try
         {

            // INITIALIZE
            var oParameters = this.GetParameters();
            oParameters.DATA[Service.Document.TAG_ENTITY_TYPE] = (long)oModel.Type;
            oParameters.DATA[Service.Planning.TAG_DATE] = oModel.DateGet();

            // SERVICE CALL
            var oReturn = Service.Planning.GetIndicators(oParameters);

            // CHECK RESULT
            if (this.CheckResult(oReturn) == true)
            {
               ViewData[FriendCash.Service.Planning.TAG_DATE] = oModel.DateGet();
               ViewData[FriendCash.Service.Document.TAG_ENTITY_TYPE] = oModel.Type;
               var oList = ((List<Model.ViewPlanningFlow>)oReturn.DATA[Service.Planning.TAG_ENTITY_INDICATORS]);
               oResult = PartialView("Index_ResumePlanning", oList);
            }

         }
         catch (Exception ex) { this.AddMessageException(ex.Message); }
         finally { if (oResult == null) { oResult = PartialView("Error"); } }

         return oResult;
      }

      #endregion

      #region ResumeMonthlyFlow
      public PartialViewResult ResumeMonthlyFlow()
      {
         PartialViewResult oResult = null;

         try
         {

            // INITIALIZE
            var oParameters = this.GetParameters();

            // RANGE
            var oToday = DateTime.Now;
            oParameters.DATA["StartDate"] = new DateTime(oToday.Year, oToday.Month, 1).AddMonths(-4);
            oParameters.DATA["FinishDate"] = new DateTime(oToday.Year, oToday.Month, 1).AddMonths(3).AddDays(-1);

            // SERVICE CALL
            var oReturn = Service.Flow.GetMonthlyFlow(oParameters);

            // CHECK RESULT
            if (this.CheckResult(oReturn) == true)
            {
               var oList = (List<Model.ViewMonthlyFlow>)oReturn.DATA[Service.Flow.TAG_ENTITY_LIST];
               ViewData[Service.Flow.TAG_ENTITY_LIST] = oList;
               oResult = PartialView("Index_ResumeMonthlyFlow");
            }

         }
         catch (Exception ex) { this.AddMessageException(ex.Message); }
         finally { if (oResult == null) { oResult = PartialView("Error"); } }

         return oResult;
      }
      #endregion

      #region Account

      public ActionResult Account(long id, Int16? page)
      {
         ActionResult oResult = null;

         try
         {

            // TITLE
            var oAccount = this.Account_Details(id);
            this.PageTitle = oAccount.Description;
            this.PageSubTitle = Resources.Flow.PAGE_TITLE;

            // INITIALIZE
            var oParameters = this.GetParameters(page);
            oParameters.DATA["idAccount"] = id;

            // SERVICE CALL
            var oReturn = Service.Flow.GetDataBalance(oParameters);

            // CHECK RESULT
            if (this.CheckResult(oReturn) == true)
            {
               var oList = ((List<Model.Flow>)oReturn.DATA[Service.Flow.TAG_ENTITY_LIST]);
               ViewData[Service.Flow.TAG_ENTITY_LIST] = oList;
               oResult = View(oList);
            }

         }
         catch (Exception ex) { this.AddMessageException(ex.Message); }
         finally { if (oResult == null) { oResult = View(); } }

         return oResult;
      }

      private Model.Account Account_Details(long idAccount)
      {
         Model.Account oAccount = null;

         // SERVICE CALL
         var oParameters = this.GetParameters();
         oParameters.DATA[Service.Account.TAG_ENTITY_KEY] = idAccount;
         var oReturn = Service.Account.Index(oParameters);

         // CHECK RESULT
         if (this.CheckResult(oReturn) == true)
         {
            var oList = ((List<Model.Account>)oReturn.DATA[Service.Account.TAG_ENTITY_LIST]);
            oAccount = oList.SingleOrDefault();
         }

         return oAccount;
      }

      #endregion

   }
}
