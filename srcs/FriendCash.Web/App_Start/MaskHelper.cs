using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriendCash.Web
{
   public static class Masks
   {

      #region GetDecimalSeparator
      public static string GetDecimalSeparator()
      {
         var oFormat = System.Threading.Thread.CurrentThread.CurrentUICulture.NumberFormat;
         return oFormat.CurrencyDecimalSeparator;
      }
      #endregion 

      #region GetDecinalDigits
      public static string GetDecinalDigits()
      {
         var oFormat = System.Threading.Thread.CurrentThread.CurrentUICulture.NumberFormat;
         return oFormat.CurrencyDecimalDigits.ToString();
      }
      #endregion 

      #region GetValueMask
      public static string GetValueMask()
      {

         var sReturn = "";

         try
         {
            var oFormat = System.Threading.Thread.CurrentThread.CurrentUICulture.NumberFormat;
            char sGroupChar = "#".ToCharArray()[0]; int iGroupsSize = (oFormat.CurrencyGroupSizes[0] - 1);
            char sDecimalChar = "0".ToCharArray()[0]; int iDecimalsSize = 2;

            sReturn += "#";
            //sReturn += oValueFormat.CurrencyGroupSeparator;
            //sReturn += new String(sGroupChar, iGroupsSize);
            sReturn += "0";
            sReturn += oFormat.CurrencyDecimalSeparator;
            sReturn += new String(sDecimalChar, iDecimalsSize);
         }
         catch { }

         return sReturn;
      }
      #endregion

      #region GetDateMask

      public static string GetDateMask()
      {

         var sReturn = "";

         try
         {
            var oFormat = System.Threading.Thread.CurrentThread.CurrentUICulture.DateTimeFormat;
            var aSplited = oFormat.ShortDatePattern.Split(new string[] { oFormat.DateSeparator }, StringSplitOptions.RemoveEmptyEntries);

            aSplited[0] = GetDateMask(aSplited[0]);
            aSplited[1] = GetDateMask(aSplited[1]); ;
            aSplited[2] = GetDateMask(aSplited[2]); ;

            //sReturn += "00/00/0000";
            //sReturn += oFormat.ShortDatePattern;
            sReturn += aSplited[0] + oFormat.DateSeparator;
            sReturn += aSplited[1] + oFormat.DateSeparator;
            sReturn += aSplited[2];
         }
         catch { }

         return sReturn;
      }

      private static string GetDateMask(string sTag)
      {
         var sLetter = sTag.Substring(0, 1);
         int iSize = (sLetter.ToLower() == "y" ? 4 : 2);
         char sChar = sLetter.ToCharArray()[0];
         return new string(sChar, iSize);
      }

      #endregion

   }
}
