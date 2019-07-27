#region Using
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
#endregion

namespace FriendCash.Service.Balances.Model
{

   [Table("v5_dataBalance")]
   internal class dataBalance : Base.BaseModel
   {

      #region New
      public dataBalance()
      {
         this.idBalance = -1;
      }
      #endregion

      #region idBalance
      [Column("idBalance"), Key]
      [DatabaseGeneratedAttribute(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
      public long idBalance { get; set; }
      #endregion

      #region idUser
      [Column("idUser", TypeName = "varchar")]
      [StringLength(128), Required]
      [Index("IDX_BALANCE_MAIN", Order = 1)]
      public string idUser { get; set; }
      #endregion

      #region idAccount

      [Column("idAccount"), Required]
      [Index("IDX_BALANCE_MAIN", Order = 2)]
      public long idAccount { get; set; }

      [ForeignKey("idAccount")]
      public virtual Accounts.Model.bindAccount idAccountModel { get; set; }

      #endregion

      #region SearchDate
      [Column("SearchDate"), Required, DataType(DataType.DateTime)]
      [Index("IDX_ENTRY_MAIN", Order = 3)]
      public DateTime SearchDate { get; set; }
      #endregion

      #region TotalValue
      [Column("TotalValue")]
      public double TotalValue { get; set; }
      #endregion

      #region PaidValue
      [Column("PaidValue")]
      public double PaidValue { get; set; }
      #endregion


      #region RowStatus
      [Index("IDX_BALANCE_MAIN", Order = 0)]
      [Column("RowStatus")]
      public override short RowStatusValue { get; set; }
      #endregion 

   }

}
