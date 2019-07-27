using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FriendCash.Web.Code
{

   public abstract class AutoCompleteView
   {
      public AutoCompleteView(object oContentDescription)
      {
         this.ContentDescription = (oContentDescription == null ? string.Empty : oContentDescription.ToString());
       }
      public string NameForValue { get; set; }
      public string NameForDescription { get { return this.NameForValue + "Description"; } }
      public string RelatedController { get; set; }
      public object ContentDescription { get; set; }
   }

   public class AutoCompleteSupplier : AutoCompleteView
   {
      public AutoCompleteSupplier(Model.Supplier oSupplier)
         : base((oSupplier == null ? string.Empty : oSupplier.Description))
      {
         this.RelatedController = "Suppliers"; 
      }
    }

   public class AutoCompleteAccount : AutoCompleteView
   {
      public AutoCompleteAccount(Model.Account oAccount)
         : base((oAccount==null ? string.Empty : oAccount.Description))
      {
         this.RelatedController = "Accounts";
      }
   }

   public class AutoCompleteDocument : AutoCompleteView
   {
      public AutoCompleteDocument(string sRelatedController, Model.Document oDocument)
         : base((oDocument == null ? string.Empty : oDocument.Description))
      {
         this.RelatedController = sRelatedController;
      }
   }

}