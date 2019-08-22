using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FriendlyCashFlow.API.Entries
{

   [Table("v6_dataEntry")]
   internal class Entry : Base.BaseData
   {

      public Entry() : base() { }

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

      [Required]
      public long PatternID { get; set; }

      public long? RecurrencyID { get; set; }

      [Column(TypeName = "varchar(128)")]
      [Required, StringLength(128)]
      public string TransferID { get; set; }

      [DataType(DataType.Date)]
      [Required]
      public DateTime DueDate { get; set; }

      [Column(TypeName = "decimal(15,2)")]
      [Required]
      public decimal Value { get; set; }

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
      internal DbSet<Entries.Entry> Entries { get; set; }
      private void OnModelCreating_Entries(ModelBuilder modelBuilder)
      {
         modelBuilder.Entity<Entries.Entry>()
            .HasIndex(x => new { x.RowStatus, x.ResourceID, x.Type, x.AccountID, x.SearchDate })
            .HasName("v6_dataPattern_index_SearchDate");
         modelBuilder.Entity<Entries.Entry>()
            .HasIndex(x => new { x.RowStatus, x.ResourceID, x.Type, x.AccountID, x.Text })
            .HasName("v6_dataPattern_index_SearchText");
      }
   }
}
