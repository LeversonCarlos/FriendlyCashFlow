using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FriendlyCashFlow.API.Entries
{

   [Table("v6_dataEntries")]
   internal class EntryData : Base.BaseData
   {

      public EntryData() : base() { }

      [Key]
      [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
      public long EntryID { get; set; }

      [Column(TypeName = "varchar(128)")]
      [Required, StringLength(128)]
      public string ResourceID { get; set; }

      [Required]
      public short Type { get; set; }

      public long? AccountID { get; set; }

      [DataType(DataType.DateTime)]
      [Required]
      public DateTime SearchDate { get; set; }

      [Column(TypeName = "varchar(500)")]
      [Required, StringLength(500)]
      public string Text { get; set; }

      public long? CategoryID { get; set; }

      public long? PatternID { get; set; }

      public long? RecurrencyID { get; set; }
      public short? RecurrencyItem { get; set; }
      public short? RecurrencyTotal { get; set; }

      [Column(TypeName = "varchar(128)")]
      [StringLength(128)]
      public string TransferID { get; set; }

      [DataType(DataType.Date)]
      [Required]
      public DateTime DueDate { get; set; }

      [Column(TypeName = "decimal(15,2)")]
      [Required]
      public decimal EntryValue { get; set; }

      [Required]
      public bool Paid { get; set; }

      [DataType(DataType.DateTime)]
      public DateTime? PayDate { get; set; }

      [Column(TypeName = "decimal(20,10)")]
      [Required]
      public decimal Sorting { get; set; }

   }

}

namespace FriendlyCashFlow.API.Base
{
   partial class dbContext
   {
      internal DbSet<Entries.EntryData> Entries { get; set; }
      private void OnModelCreating_Entries(ModelBuilder modelBuilder)
      {
         modelBuilder.Entity<Entries.EntryData>()
            .HasIndex(x => new { x.RowStatus, x.ResourceID, x.AccountID, x.SearchDate })
            .IncludeProperties(x => new { x.EntryID, x.Type, x.TransferID, x.EntryValue, x.Paid })
            .HasDatabaseName("v6_dataEntries_index_SearchDate");
         modelBuilder.Entity<Entries.EntryData>()
            .HasIndex(x => new { x.RowStatus, x.ResourceID, x.AccountID, x.Text })
            .HasDatabaseName("v6_dataEntries_index_SearchText");
         modelBuilder.Entity<Entries.EntryData>()
            .HasIndex(x => new { x.RowStatus, x.ResourceID, x.TransferID })
            .HasDatabaseName("v6_dataEntries_index_SearchTransfer");
         modelBuilder.Entity<Entries.EntryData>()
            .HasIndex(x => new { x.RecurrencyID })
            .HasDatabaseName("v6_dataEntries_index_SearchRecurrency");
      }
   }
}
