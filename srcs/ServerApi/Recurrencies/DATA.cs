using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FriendlyCashFlow.API.Recurrencies
{

   [Table("v6_dataRecurrencies")]
   internal class RecurrencyData : Base.BaseData
   {

      public RecurrencyData() : base() { }

      [Key]
      [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
      public long RecurrencyID { get; set; }

      [Column(TypeName = "varchar(128)")]
      [Required, StringLength(128)]
      public string ResourceID { get; set; }

      [Required]
      public long PatternID { get; set; }

      [Required]
      public long AccountID { get; set; }

      [Column(TypeName = "decimal(15,2)")]
      [Required]
      public decimal EntryValue { get; set; }

      [DataType(DataType.DateTime)]
      [Required]
      public DateTime EntryDate { get; set; }

      [Required]
      public short Type { get; set; }

      public int Count { get; set; }

      [DataType(DataType.DateTime)]
      [Required]
      public DateTime InitialDate { get; set; }

      [DataType(DataType.DateTime)]
      public DateTime? LastDate { get; set; }

   }

}

namespace FriendlyCashFlow.API.Base
{
   partial class dbContext
   {
      internal DbSet<Recurrencies.RecurrencyData> Recurrencies { get; set; }
      private void OnModelCreating_Recurrencies(ModelBuilder modelBuilder)
      {
         modelBuilder.Entity<Recurrencies.RecurrencyData>()
            .HasIndex(x => new { x.RowStatus, x.ResourceID, x.RecurrencyID })
            .HasDatabaseName("v6_dataRecurrencies_index_Search");
      }
   }
}
