using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.IO;
using System.Globalization;
/// Globalize: https://github.com/jquery/globalize

namespace FriendCash.Web.Code
{
   public static class URLs
   {

      //public static string Validate { get { return "~/Scripts/jquery.validate.js"; } }

      public static string Globalize { get { return "~/Scripts/globalize/globalize.js"; } }
      public static string GlobalizeCulture
      {
         get
         {
            //Determine culture - GUI culture for preference, user selected culture as fallback
            var currentCulture = CultureInfo.CurrentCulture;
            var filePattern = "~/scripts/globalize/cultures/globalize.culture.{0}.js";
            var regionalisedFileToUse = string.Format(filePattern, "en-US"); //Default localisation to use

            //Try to pick a more appropriate regionalisation
            if (File.Exists(HostingEnvironment.MapPath(string.Format(filePattern, currentCulture.Name)))) //First try for a globalize.culture.en-GB.js style file
               regionalisedFileToUse = string.Format(filePattern, currentCulture.Name);
            else if (File.Exists(HostingEnvironment.MapPath(string.Format(filePattern, currentCulture.TwoLetterISOLanguageName)))) //That failed; now try for a globalize.culture.en.js style file
               regionalisedFileToUse = string.Format(filePattern, currentCulture.TwoLetterISOLanguageName);

            return regionalisedFileToUse;
         }
      }
   }
}