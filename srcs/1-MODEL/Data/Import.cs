using System;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace FriendCash.Model
{

   #region Import
   [Table("v4_Import")]
   public class Import : Base
   {

      #region New
      public Import()
      {
         this.Status = enStatus.Waiting;
         this.StartDate = DateTime.Now;
       }
      #endregion

      #region idImport
      [Column("idImport"), ConcurrencyCheck, Required]
      public long idImport { get; set; }
      #endregion

      #region StartDate
      [Column("StartDate"), Required]
      public DateTime StartDate { get; set; }
      #endregion

      #region FinishDate
      [Column("FinishDate")]
      public DateTime? FinishDate { get; set; }
      #endregion

      #region Message
      [Column("Message", TypeName = "varchar"), StringLength(4000)]
      public string Message { get; set; }
      #endregion

      #region Status

      [Column("Status")]
      public short StatusValue { get; set; }

      public enum enStatus : short { Waiting = 0, Starting = 1, Clearing = 2, Streaming = 3, Importing = 4, Finished = 5, Canceled = 9 }

      [NotMapped]
      public enStatus Status
      {
         get { return ((enStatus)this.StatusValue); }
         set { this.StatusValue = ((short)value); }
      }

      #endregion

   }
   #endregion

   #region ImportData
   [Table("v4_ImportData")]
   public class ImportData : Base
   {

      #region New
      public ImportData()
      {
         this.Status = enStatus.Waiting;
      }
      #endregion

      #region idImport
      [Column("idImport"), Required]
      public long idImport { get; set; }
      #endregion

      #region Data
      [Column("Data", TypeName = "varchar"), StringLength(4000), Required]
      public string Data { get; set; }
      #endregion

      #region Message
      [Column("Message", TypeName = "varchar"), StringLength(4000)]
      public string Message { get; set; }
      #endregion

      #region Status

      [Column("Status")]
      public short StatusValue { get; set; }

      public enum enStatus : short { Waiting = 0, Processing = -1, Success = 1, Error = 9 }

      [NotMapped]
      public enStatus Status
      {
         get { return ((enStatus)this.StatusValue); }
         set { this.StatusValue = ((short)value); }
      }

      #endregion

   }
   #endregion

   #region ImportStatus
   [Table("v4_ImportStatus")]
   public class ImportStatus : Base
   {

      #region New
      public ImportStatus()
      {
         this.Status = Import.enStatus.Waiting;
         this.ProgressTotal = 100;
         this.ProgressCompleted = 0;
      }
      #endregion

      #region idImport
      [Column("idImport"), Required]
      public long idImport { get; set; }
      #endregion

      #region Status

      [Column("Status")]
      public short StatusValue { get; set; }

      public Import.enStatus Status
      {
         get { return ((Import.enStatus)this.StatusValue); }
         set { this.StatusValue = ((short)value); }
      }

      #endregion

      #region StepDate
      [Column("StepDate"), Required]
      public DateTime StepDate { get; set; }
      #endregion

      #region ProgressTotal
      [Column("ProgressTotal"), Required]
      public long ProgressTotal { get; set; }
      #endregion

      #region ProgressCompleted
      [Column("ProgressCompleted")]
      public long ProgressCompleted { get; set; }
      #endregion

      #region ProgressPercent
      public long ProgressPercent { get { return Convert.ToInt64(Convert.ToDouble(this.ProgressCompleted) / Convert.ToDouble(this.ProgressTotal) * Convert.ToDouble(100)); } }
      #endregion

   }
   #endregion

   #region ViewImport
   public class ViewImport 
   {

      #region idImport
      public long idImport { get; set; }
      #endregion

      #region StartDate
      public DateTime StartDate { get; set; }
      #endregion

      #region FinishDate
      public DateTime? FinishDate { get; set; }
      #endregion

      #region EstimatedMinutes
      public double EstimatedMinutes
      {
         get
         {
            double iReturn = 0;
            try
            {
               double iMilliseconds = (this.ProgressTotal - this.ProgressCompleted) * this.ProgressSpeed;
               iReturn = Math.Round(TimeSpan.FromMilliseconds(iMilliseconds).TotalMinutes, 1);
            }
            catch { }
            return iReturn;
          }
       }
      #endregion

      #region Message
      public string Message { get; set; }
      #endregion

      #region Status

      public short StatusValue { get; set; }

      public Import.enStatus Status
      {
         get { return ((Import.enStatus)this.StatusValue); }
         set { this.StatusValue = ((short)value); }
      }

      public string StatusDescr { get { return this.Status.ToString(); } }

      #endregion

      #region StepDate
      public DateTime StepDate { get; set; }
      #endregion

      #region ProgressTotal
      public long ProgressTotal { get; set; }
      #endregion

      #region ProgressCompleted
      public long ProgressCompleted { get; set; }
      #endregion

      #region ProgressPercent
      public long ProgressPercent { get { return Convert.ToInt64(Convert.ToDouble(this.ProgressCompleted) / Convert.ToDouble(this.ProgressTotal) * Convert.ToDouble(100)); } }
      #endregion

      #region ProgressSpeed
      public double ProgressSpeed
      {
         get
         {
            TimeSpan oDiff = DateTime.Now.Subtract(this.StepDate);
            return Math.Round(oDiff.TotalMilliseconds / Convert.ToDouble(this.ProgressCompleted), 1);
         }
      }
      #endregion

      #region ImportedSucess
      public long ImportedSucess { get; set; }
      #endregion

      #region ImportedError
      public long ImportedError { get; set; }
      #endregion

   }
   #endregion

}
