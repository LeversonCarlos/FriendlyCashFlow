using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FriendCash.Web.Code
{

   public class MyHelper
   {

      #region User
      public class User
      {

         #region MyLogin
         public static Model.Login MyLogin
         {
            get
            {
               Model.Login oReturn = null;
               if (System.Web.HttpContext.Current.Session["MyLogin"] != null)
               {
                  oReturn = ((Model.Login)System.Web.HttpContext.Current.Session["MyLogin"]);

               }
               return oReturn;
            }
         }
         #endregion

         #region IsAuthenticated
         public static bool IsAuthenticated()
         {
            bool bReturn = false;
            if (MyLogin != null)
            {
               bReturn = true;
            }
            return bReturn;
         }
         #endregion

       }
      #endregion

      #region Common
      public class Common
      {

         #region GetFooterClass
         public static string GetFooterClass(object oMSG)
         {
            string sReturn = "main-footer-default";

            //main-footer-warning
            if (oMSG != null && ((List<string>)oMSG).Count != 0)
            {
               sReturn = "main-footer-error";
             }

            return sReturn;
          }
         #endregion

       }
      #endregion

      #region Planning
      public class Planning
      {

         #region PlanningDropDown

         public static string PlanningDropDown(string sName, long? iSelected, object oPlannings)
         {
            string sReturn = string.Empty;

            sReturn += "<select class='treeview' id='" + sName + "' name='" + sName + "'>";
            sReturn += PlanningDropDown(iSelected, 0, string.Empty, 0);
            if (oPlannings != null) { sReturn += PlanningDropDown(iSelected, ((IEnumerable<Model.Planning>)oPlannings), 0); }
            sReturn += "</select>";

            return sReturn;
         }

         private static string PlanningDropDown(long? iSelected, IEnumerable<Model.Planning> oPlannings, int iLevel)
         {
            string sReturn = string.Empty;

            foreach (Model.Planning oPlanning in oPlannings)
            {
               sReturn += PlanningDropDown(iSelected, oPlanning.idPlanning, oPlanning.Description, iLevel);
               IEnumerable<Model.Planning> oChilds = oPlanning.Childs;
               if (oChilds != null && oChilds.Count() != 0)
               { sReturn += PlanningDropDown(iSelected, oChilds, (iLevel + 1)); }
            }
            return sReturn;
         }

         private static string PlanningDropDown(long? iSelected, long iValue, string sDescription, int iLevel)
         {
            string sReturn = string.Empty;

            char sSpace = Convert.ToChar(".");
            string sTab = new string(sSpace, iLevel);
            string sValue = "value='" + iValue.ToString() + "'";
            string sLabel = "label='" + sTab + sDescription + "'";
            string sSelected = ""; if (iValue == iSelected) { sSelected = "selected='selected'"; }

            sReturn += "<option " + sValue + " " + sLabel + " " + sSelected + " />";

            return sReturn;
         }

         #endregion

         #region GetTypeClass
         public static string GetTypeClass(FriendCash.Model.Document.enType iType)
         {
            string sReturn = string.Empty;

            if (iType == FriendCash.Model.Document.enType.Expense) { sReturn = "Expense"; }
            else if (iType == FriendCash.Model.Document.enType.Income) { sReturn = "Income"; }

            return sReturn;
         }
         #endregion

       }
      #endregion

      #region IndexForm
      public class IndexForm
      {

         #region GetCrossHatch
         public static string GetCrossHatch(System.Web.Mvc.TempDataDictionary oTempData)
         {

            if (!oTempData.ContainsKey("CrossHatch") || oTempData["CrossHatch"] == "TableRow1")
            { oTempData["CrossHatch"] = "TableRow0"; }
            else { oTempData["CrossHatch"] = "TableRow1"; }

            return oTempData["CrossHatch"].ToString();
         }
         #endregion

         #region HasMorePages
         public static bool HasMorePages(System.Web.Mvc.ViewDataDictionary oViewData)
         {
            return ((bool)oViewData["HasMorePages"]);
         }
         #endregion

         #region GetPaginationTarget
         public static string GetPaginationTarget(System.Web.Mvc.ViewDataDictionary oViewData)
         {
            Int16? iNextPage = ((Int16?)oViewData["NextPage"]);
            return "oPage" + iNextPage.Value.ToString();
         }
         #endregion

         #region GetPaginationParams
         public static object GetPaginationParams(System.Web.Mvc.ViewDataDictionary oViewData, long iID)
         {
            Int16? iNextPage = ((Int16?)oViewData["NextPage"]);
            return new { id= iID, page = iNextPage.Value };
          }
         public static object GetPaginationParams(System.Web.Mvc.ViewDataDictionary oViewData)
         {
            Int16? iNextPage = ((Int16?)oViewData["NextPage"]);
            string sSearch = oViewData["Search"].ToString();
            return new { page = iNextPage.Value, search = sSearch };
         }
         #endregion

         #region BoxedCheckBoxFor
         public static string BoxedCheckBoxFor(string sName, bool bValue, string sDescription)
         {
            string sReturn = string.Empty;

            sReturn += "<div class='boxed-checkbox'>";
            sReturn +=  "<label>";
            sReturn +=   "<input id='" + sName + "' name='" + sName + "' type='checkbox' value='true' >";
            /* value='" + bValue.ToString() + "' */
            sReturn +=   "<span>" + sDescription + "</span>";
            sReturn +=  "</label>";
            sReturn += "</div>";

            return sReturn;
         }
         #endregion

      }
      #endregion

   }

}