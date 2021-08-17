using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FriendlyCashFlow.API.Budget
{

   [Table("v6_dataBudget")]
   internal class BudgetData : Base.BaseData
   {

      public BudgetData() : base() { }

      [Key]
      [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
      public long BudgetID { get; set; }

      [Column(TypeName = "varchar(128)")]
      [Required, StringLength(128)]
      public string ResourceID { get; set; }

      public long PatternID { get; set; }

      [ForeignKey("PatternID")]
      public virtual Patterns.PatternData PatternDetails { get; set; }

      [Column(TypeName = "decimal(15,2)")]
      [Required]
      public decimal Value { get; set; }

   }

}

namespace FriendlyCashFlow.API.Base
{
   partial class dbContext
   {
      internal DbSet<Budget.BudgetData> Budget { get; set; }
      private void OnModelCreating_Budget(ModelBuilder modelBuilder)
      {
         modelBuilder.Entity<Budget.BudgetData>()
            .HasIndex(x => new { x.RowStatus, x.ResourceID, x.PatternID })
            .IncludeProperties(x => new { x.BudgetID, x.Value })
            .HasName("v6_dataBudget_index_Search");
      }
   }
}
