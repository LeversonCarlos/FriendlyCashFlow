using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FriendlyCashFlow.API.Shared
{
   partial class dbContext
   {
      internal DbSet<Accounts.AccountData> Accounts { get; set; }
   }
}

namespace FriendlyCashFlow.API.Accounts
{

   [Table("v5_dataAccounts")]
   internal class AccountData : Shared.BaseData
   {

      [Column("AccountID"), Key]
      [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
      public long AccountID { get; set; }

      [Column("ResourceID", TypeName = "varchar(128)")]
      [StringLength(128), Required]
      public string ResourceID { get; set; }

      [Column("Text", TypeName = "varchar(500)")]
      [StringLength(500), Required]
      public string Text { get; set; }

      [Column("Type")]
      public short Type { get; set; }

      [Column("DueDay")]
      public short? DueDay { get; set; }

      [Column("Active")]
      public bool Active { get; set; }

   }

}
