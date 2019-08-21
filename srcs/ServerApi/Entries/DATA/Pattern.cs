using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FriendlyCashFlow.API.Entries
{

   [Table("v6_dataPattern")]
   internal class Pattern : Base.BaseData
   {

      public Pattern() : base() { }

      [Key]
      [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
      public long PatternID { get; set; }

      [Column(TypeName = "varchar(128)")]
      [Required, StringLength(128)]
      public string ResourceID { get; set; }

      [Required]
      public short Type { get; set; }

      [Required]
      public long CategoryID { get; set; }

      [Column(TypeName = "varchar(500)")]
      [Required, StringLength(500)]
      public string Text { get; set; }

      public short Quantity { get; set; }

   }

}

namespace FriendlyCashFlow.API.Base
{
   partial class dbContext
   {
      internal DbSet<Entries.Pattern> Patterns { get; set; }

      private void OnModelCreating_Patterns(ModelBuilder modelBuilder)
      {
         modelBuilder.Entity<Entries.Pattern>()
            .HasIndex(x => new { x.RowStatus, x.ResourceID, x.Type, x.CategoryID, x.Text })
            .HasName("v6_dataPatterns_index_Search");
      }

   }
}
