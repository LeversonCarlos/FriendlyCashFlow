using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FriendlyCashFlow.API.Accounts
{

   [Table("v6_dataAccounts")]
   internal class AccountData : Base.BaseData
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

      [Column("ClosingDay")]
      public short? ClosingDay { get; set; }

      [Column("DueDay")]
      public short? DueDay { get; set; }

      [Column("Active")]
      public bool Active { get; set; }

   }

}

namespace FriendlyCashFlow.API.Base
{
   partial class dbContext
   {
      internal DbSet<Accounts.AccountData> Accounts { get; set; }
      private void OnModelCreating_Accounts(ModelBuilder modelBuilder)
      {
         modelBuilder.Entity<Accounts.AccountData>()
            .HasIndex(x => new { x.RowStatus, x.ResourceID, x.AccountID, x.Text })
            .HasDatabaseName("v6_dataAccounts_index_Search");
      }
   }
}
