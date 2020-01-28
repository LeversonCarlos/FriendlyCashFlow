using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FriendlyCashFlow.API.Categories
{

   [Table("v6_dataCategories")]
   internal class CategoryData : Base.BaseData
   {

      [Column("CategoryID"), Key]
      [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
      public long CategoryID { get; set; }

      [Column("ResourceID", TypeName = "varchar(128)")]
      [StringLength(128), Required]
      public string ResourceID { get; set; }

      [Column("Text", TypeName = "varchar(500)")]
      [StringLength(500), Required]
      public string Text { get; set; }

      [Column("Type"), Required]
      public short Type { get; set; }

      [Column("ParentID")]
      public long? ParentID { get; set; }

      [Column("HierarchyText", TypeName = "varchar(4000)")]
      [StringLength(4000), Required]
      public string HierarchyText { get; set; }

   }

}

namespace FriendlyCashFlow.API.Base
{
   partial class dbContext
   {
      internal DbSet<Categories.CategoryData> Categories { get; set; }
      private void OnModelCreating_Categories(ModelBuilder modelBuilder)
      {
         modelBuilder.Entity<Categories.CategoryData>()
            .HasIndex(x => new { x.RowStatus, x.ResourceID, x.Type, x.CategoryID, x.HierarchyText })
            .HasName("v6_dataCategories_index_Search");
         modelBuilder.Entity<Categories.CategoryData>()
            .HasIndex(x => new { x.RowStatus, x.ResourceID, x.Type, x.ParentID, x.CategoryID, x.Text })
            .HasName("v6_dataCategories_index_Parent");
      }
   }
}
