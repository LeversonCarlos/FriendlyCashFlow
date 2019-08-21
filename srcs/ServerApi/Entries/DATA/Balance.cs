using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FriendlyCashFlow.API.Entries
{

   [Table("v6_dataBalance")]
   internal class Balance : Base.BaseData
   {

      public Balance() : base() { }

      [Column(TypeName = "varchar(128)")]
      [StringLength(128), Required]
      public string ResourceID { get; set; }

      [Required]
      public long AccountID { get; set; }

      [DataType(DataType.DateTime)]
      [Required]
      public DateTime SearchDate { get; set; }

      [Column(TypeName = "decimal(15,2)")]
      public decimal TotalValue { get; set; }

      [Column(TypeName = "decimal(15,2)")]
      public decimal PaidValue { get; set; }

   }

}

namespace FriendlyCashFlow.API.Base
{
   partial class dbContext
   {
      internal DbSet<Entries.Balance> Balances { get; set; }

      private void OnModelCreating_Balances(ModelBuilder modelBuilder)
      {
         modelBuilder.Entity<Entries.Balance>()
            .HasKey(c => new { c.ResourceID, c.AccountID, c.SearchDate });
      }

   }
}
