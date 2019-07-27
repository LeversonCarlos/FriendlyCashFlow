using System;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace FriendCash.Model
{

   [DataContract()]
   [Table("v4_Accounts")]
   public class Account : Base
   {

      #region New
      public Account()
      {
         this.Status = enStatus.Enabled;
         this.Type = enType.None;
       }
      #endregion

      #region idAccount
      [DataMember]
      [Column("idAccount"), Index("IX_AccountKey")]
      [Display(Name = "COLUMN_ID", ResourceType = typeof(FriendCash.Resources.Accounts))]
      public long idAccount { get; set; }
      #endregion

      #region Code
      [DataMember]
      [Column("Code", TypeName = "varchar"), StringLength(50)]
      [Required(ErrorMessageResourceName = "MSG_REQUIRED_CODE", ErrorMessageResourceType = typeof(FriendCash.Resources.Accounts))]
      [Display(Name = "COLUMN_CODE", ResourceType = typeof(FriendCash.Resources.Accounts))]
      public string Code { get; set; }
      #endregion

      #region Description
      [DataMember]
      [Column("Description", TypeName = "varchar"), StringLength(500)]
      [Required(ErrorMessageResourceName = "MSG_REQUIRED_DESCRIPTION", ErrorMessageResourceType = typeof(FriendCash.Resources.Accounts))]
      [Display(Name = "COLUMN_DESCRIPTION", ResourceType = typeof(FriendCash.Resources.Accounts))]
      public string Description { get; set; }
      #endregion

      #region Type

      [DataMember]
      [Column("Type"), Required]
      public short TypeValue { get; set; }

      [NotMapped]
      [Display(Name = "COLUMN_TYPE", ResourceType = typeof(FriendCash.Resources.Accounts))]
      public enType Type
      {
         get { return ((enType)this.TypeValue); }
         set { this.TypeValue = ((short)value); }
      }
      public enum enType : short { None = 0, Bank = 1, CreditCard = 2 }

      #endregion

      #region DueDay
      [DataMember]
      [Column("DueDay")]
      [Display(Name = "COLUMN_DUE_DAY", ResourceType = typeof(FriendCash.Resources.Accounts))]
      public short? DueDay { get; set; }
      #endregion

      #region DueDate
      public DateTime DueDate
      {
         get
         {
            DateTime oDueDate = DateTime.Now;
            if (this.Type == enType.CreditCard && this.DueDay.HasValue && this.DueDay.Value != 0)
            {
               if (DateTime.Now.Day > this.DueDay.Value) { oDueDate = oDueDate.AddMonths(1); }
               oDueDate = new DateTime(oDueDate.Year, oDueDate.Month, this.DueDay.Value);
            }
            return oDueDate;
          }
       }
      #endregion

      #region Status

      [DataMember]
      [Column("Status")]
      public short StatusValue { get; set; }

      [NotMapped]
      [Display(Name = "COLUMN_STATUS", ResourceType = typeof(FriendCash.Resources.Accounts))]
      public enStatus Status
      {
         get { return ((enStatus)this.StatusValue); }
         set { this.StatusValue = ((short)value); }
       }
      public enum enStatus : short { Disabled = 0, Enabled = 1 }

      #endregion

      #region ToString
      public override string ToString()
      {
         var sReturn = string.Empty;

         sReturn += Resources.Accounts.COLUMN_CODE +":";
         sReturn += (string.IsNullOrEmpty(this.Code) ? Resources.Base.EMPTY : "<b>" + this.Code + "</b>");
         sReturn += "</br>";

         sReturn += Resources.Accounts.COLUMN_DESCRIPTION + ":";
         sReturn += (string.IsNullOrEmpty(this.Description) ? Resources.Base.EMPTY : "<b>" + this.Description + "</b>");
         sReturn += "</br>";

         return sReturn;
       }
      #endregion

   }

 }
