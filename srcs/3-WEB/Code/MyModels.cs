using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//using System.Runtime.Serialization;

namespace FriendCash.Web.Code.MyModels
{

   #region Search
   public class Search
   {

      #region Value
      public string Value 
      {
         get
         {
            return "" +
               (this.UseSettled == true ? (this.ValueSettled == true ? "S1!" : "S0!") : "") +
               this.ValueSearch + 
               "";
         }
         set { string r = value; }
       }
      #endregion

      #region ValueSearch
      public string ValueSearch { get; set; }
      #endregion

      #region ValueSettled
      public bool ValueSettled { get; set; }
      #endregion

      #region UseSettled
      public bool UseSettled { get; set; }
      #endregion

    }
   #endregion

   #region AutoComplete
   public class AutoComplete
   {

      public string Controller { get; set; }
      public string ContentValue { get; set; }
      public string ContentDescr { get; set; }
      public string ControlValue { get; set; }
      public string ControlDescr { get { return "Descr" + this.ControlValue; } }
      public string RelatedValue { get; set; }
      public string RelatedDescr { get; set; }

      public string TagSuccess
      {
         get
         {
            return "" +
               "function (data) " + 
               "{" +
                  "response($.map(data, function (item) " + 
                  "{" +
                     "return { label: item." + this.RelatedDescr + ", id: item." + this.RelatedValue + " }; " + 
                   "})) " + 
                "} " + 
               "";
          }
       }

    }
   #endregion

}