using FriendCash.Model.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FriendCash.Web.Code
{

   #region Converter
   public static class Converter
   {

      #region GetPackageJson

      public class PackageJson
      {
         public bool OK { get; set; }
         public string[] Exceptions { get; set; }
         public string[] Warnings { get; set; }
         public string[] Informations { get; set; }
         public string Redirect { get; set; }
         public SortedList<string, object> DATA { get; set; }
      }

      public static PackageJson GetPackageJson(Package oReturn, string sRedirect, bool bIncludeData)
      {
         var oJson = new PackageJson();
         oJson.OK = oReturn.OK;
         oJson.Exceptions = (from MSGs in oReturn.MSG where MSGs.Type == Message.enType.Exception select MSGs.Text).ToArray();
         oJson.Warnings = (from MSGs in oReturn.MSG where MSGs.Type == Message.enType.Warning select MSGs.Text).ToArray();
         oJson.Informations = (from MSGs in oReturn.MSG where MSGs.Type == Message.enType.Information select MSGs.Text).ToArray();
         if (bIncludeData) { oJson.DATA = oReturn.DATA; }
         oJson.Redirect = sRedirect;
         return oJson;
      }

      #endregion

   }
   #endregion

}