#region Using
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
#endregion

namespace FriendCash.Service.Accounts.Model
{
   public enum enAccountType : short { General = 0, Bank = 1, CreditCard = 2, Investment = 3, Service = 4 }

   public class viewAccount
   {

      #region New
      public viewAccount() { }
      public viewAccount(bindAccount Value) : this()
      {
         this.idAccount = Value.idAccount;
         this.Text = Value.Text;
         this.Type = Value.Type;
         this.Active = Value.Active;
         this.ClosingDay = Value.ClosingDay;
         this.DueDay = Value.DueDay;
      }
      #endregion

      #region idAccount
      [Display(Description = "LABEL_ACCOUNTS_IDACCOUNT")]
      public long idAccount { get; set; }
      #endregion

      #region Text
      [StringLength(500, ErrorMessage = "MSG_ACCOUNTS_TEXT_MAXLENGTH")]
      [Required(ErrorMessage = "MSG_ACCOUNTS_TEXT_REQUIRED")]
      [Display(Description = "LABEL_ACCOUNTS_TEXT")]
      public string Text { get; set; }
      #endregion

      #region Type
      public short TypeValue { get; set; }
      [Display(Description = "LABEL_ACCOUNTS_TYPE")]
      public enAccountType Type
      {
         get { return ((enAccountType)this.TypeValue); }
         set { this.TypeValue = ((short)value); }
      }
      #endregion

      #region ClosingDay
      [Display(Description = "LABEL_ACCOUNTS_CLOSINGDAY")]
      public short? ClosingDay { get; set; }
      #endregion

      #region DueDay
      [Display(Description = "LABEL_ACCOUNTS_DUEDAY")]
      public short? DueDay { get; set; }
      #endregion

      #region Active
      [Display(Description = "LABEL_ACCOUNTS_ACTIVE")]
      public bool Active { get; set; }
      #endregion


      #region Icon

      public string Icon
      {
         get { return GetIcon(this.Type); }
      }

      public static string GetIcon(Model.enAccountType accountType)
      {
         switch (accountType)
         {
            case Model.enAccountType.Bank: return "account_balance";
            case Model.enAccountType.CreditCard: return "credit_card";
            case Model.enAccountType.Investment: return "local_atm";
            case Model.enAccountType.Service: return "card_giftcard";
            default: return "account_balance_wallet";
         }
      }

      #endregion

   }
}