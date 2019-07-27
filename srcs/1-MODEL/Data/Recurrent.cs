using System;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace FriendCash.Model
{

   [Table("v4_Recurrent")]
   public class Recurrent : Base
   {

      #region New
      public Recurrent()
      {
         this.Status = enStatus.Enabled;
         this.Frequency = enFrequency.Monthly;
         this.Parcels = 0;
         this.StartDate = DateTime.Now;
       }
      #endregion

      #region idRecurrent
      [Column("idRecurrent")]
      [Display(Name = "COLUMN_ID", ResourceType = typeof(FriendCash.Resources.Recurrent))]
      public long idRecurrent { get; set; }
      #endregion

      #region Type

      [Column("Type"), Required]
      public short TypeValue { get; set; }

      [NotMapped]
      [Required(ErrorMessageResourceName = "MSG_REQUIRED_TYPE", ErrorMessageResourceType = typeof(FriendCash.Resources.Document))]
      [Display(Name = "COLUMN_TYPE", ResourceType = typeof(FriendCash.Resources.Document))]
      public Document.enType Type
      {
         get { return ((Document.enType)this.TypeValue); }
         set { this.TypeValue = ((short)value); }
      }

      #endregion      
      
      #region idDocument
      [Column("idDocument")]
      [Required(ErrorMessageResourceName = "MSG_REQUIRED_DOCUMENT", ErrorMessageResourceType = typeof(FriendCash.Resources.Recurrent))]
      [Display(Name = "COLUMN_DOCUMENT", ResourceType = typeof(FriendCash.Resources.Recurrent))]
      public long idDocument { get; set; }
      #endregion

      #region Value
      [Column("Value")]
      [Required(ErrorMessageResourceName = "MSG_REQUIRED_VALUE", ErrorMessageResourceType = typeof(FriendCash.Resources.Recurrent))]
      [Display(Name = "COLUMN_VALUE", ResourceType = typeof(FriendCash.Resources.Recurrent))]
      public double Value { get; set; }
      #endregion

      #region Frequency

      [Column("Frequency"), Required]
      public short FrequencyValue { get; set; }

      public enum enFrequency : short { Daily = 0, Weekly = 1, Monthly = 2, Bimonthly = 3, Quarterly = 4, Semiannual = 5, Annual = 6 }

      [NotMapped]
      [Required(ErrorMessageResourceName = "MSG_REQUIRED_FREQUENCY", ErrorMessageResourceType = typeof(FriendCash.Resources.Recurrent))]
      [Display(Name = "COLUMN_FREQUENCY", ResourceType = typeof(FriendCash.Resources.Recurrent))]
      public enFrequency Frequency
      {
         get { return ((enFrequency)this.FrequencyValue); }
         set { this.FrequencyValue = ((short)value); }
      }

      #endregion

      #region Parcels

      [Column("Parcels")]
      [Required(ErrorMessageResourceName = "MSG_REQUIRED_PARCELS", ErrorMessageResourceType = typeof(FriendCash.Resources.Recurrent))]
      [Display(Name = "COLUMN_PARCELS", ResourceType = typeof(FriendCash.Resources.Recurrent))]
      public int Parcels { get; set; }

      public bool ParcelsRecurrent 
      {
         get { return (this.Parcels == -1 ? true : false); }
      }

      public string ParcelsDescription { get { return (this.Parcels == -1 ? "Recurrent" : this.Parcels.ToString()); } }

      #endregion

      #region StartDate
      [Column("StartDate"), DataType(DataType.Date)]
      [Required(ErrorMessageResourceName = "MSG_REQUIRED_START_DATE", ErrorMessageResourceType = typeof(FriendCash.Resources.Recurrent))]
      [Display(Name = "COLUMN_START_DATE", ResourceType = typeof(FriendCash.Resources.Recurrent))]
      public DateTime StartDate { get; set; }
      #endregion

      #region Status

      [Column("Status"), Required]
      public short StatusValue { get; set; }

      [NotMapped]
      [Display(Name = "COLUMN_STATUS", ResourceType = typeof(FriendCash.Resources.Recurrent))]
      public enStatus Status
      {
         get { return ((enStatus)this.StatusValue); }
         set { this.StatusValue = ((short)value); }
      }
      public enum enStatus : short { Disabled = 0, Enabled = 1 }

      #endregion

      #region Details

      #region DocumentDetails
      [ForeignKey("idDocument")]
      public virtual Document DocumentDetails { get; set; }
      #endregion

      #endregion

   }

}
