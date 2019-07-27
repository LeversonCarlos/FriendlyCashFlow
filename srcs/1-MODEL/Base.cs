using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace FriendCash.Model
{

   [DataContract]
   public class Base
   {

      #region New
      public Base()
      {
         this.CreatedIn = DateTime.Now;
         this.RowStatus = enRowStatus.Temporary;
       }
      #endregion

      #region idRow
      [DataMember]
      [Column("idRow"), KeyAttribute]
      //[ReadOnly(true)]
      public Int64 idRow { get; set; }
      #endregion

      #region RowStatus

      [DataMember]
      [Column("RowStatus")]
      public short RowStatusValue { get; set; }

      //[EnumMember]
      public enum enRowStatus : short { Removed=-1, Temporary=0, Active=1 }

      [NotMapped]
      public enRowStatus RowStatus 
      {
         get { return ((enRowStatus)this.RowStatusValue);  }
         set { this.RowStatusValue = ((short)value); }
      }

      #endregion

      #region CreatedBy

      [DataMember]
      [Column("CreatedBy")]
      public Int64? CreatedBy { get; set; }

      // [ForeignKey("CreatedBy")]
      // public virtual Login CreatedByDetails { get; set; }

      #endregion

      #region CreatedIn
      [DataMember]
      [Column("CreatedIn"), DataType(DataType.DateTime)]
      public DateTime CreatedIn { get; set; }
      #endregion

      #region RemovedBy

      [DataMember]
      [Column("RemovedBy")]
      public Int64? RemovedBy { get; set; }

      //[ForeignKey("RemovedBy")]
      //public virtual Login RemovedByDetails { get; set; }

      #endregion

      #region RemovedIn
      [DataMember]
      [Column("RemovedIn"), DataType(DataType.DateTime)]
      public DateTime? RemovedIn { get; set; }
      #endregion

      #region idOriginal
      [DataMember]
      [Column("idOriginal")]
      public Int64? idOriginal { get; set; }
      #endregion

    }

 }
