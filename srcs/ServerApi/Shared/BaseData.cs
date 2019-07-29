using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FriendlyCashFlow.API.Shared
{
   public enum enRowStatus : short { Removed = -1, Temporary = 0, Active = 1 }

   public class BaseData : IDisposable
   {

      internal BaseData()
      {
         this.RowDate = DateTime.Now;
         this.RowStatus = (short)enRowStatus.Temporary;
      }

      [Column("RowStatus")]
      public short RowStatus { get; set; }

      [Column("RowDate"), DataType(DataType.DateTime)]
      public DateTime RowDate { get; set; }

      public void Dispose()
      { /* throw new NotImplementedException(); */ }

   }

}
