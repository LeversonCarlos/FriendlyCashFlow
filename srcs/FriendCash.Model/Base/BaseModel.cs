#region Using
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
#endregion 

namespace FriendCash.Service.Base
{

   public class BaseModel : IDisposable
   {

      #region New
      public BaseModel()
      {
         this.RowDate = DateTime.Now;
         this.RowStatus = enRowStatus.Temporary;
      }
      #endregion

      #region RowStatus

      [Column("RowStatus")]
      public virtual short RowStatusValue { get; set; }

      [NotMapped]
      public enRowStatus RowStatus
      {
         get { return ((enRowStatus)this.RowStatusValue); }
         set { this.RowStatusValue = ((short)value); }
      }

      public enum enRowStatus : short { Removed = -1, Temporary = 0, Active = 1 }
      #endregion

      #region RowDate
      [Column("RowDate"), DataType(DataType.DateTime)]
      public DateTime RowDate { get; set; }
      #endregion

      #region Dispose
      public void Dispose()
      {
         //throw new NotImplementedException();
      }
      #endregion

   }

}
