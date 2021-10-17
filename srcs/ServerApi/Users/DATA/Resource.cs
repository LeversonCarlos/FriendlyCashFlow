using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FriendlyCashFlow.API.Users
{

   [Table("v6_identityUserResources")]
   internal class UserResourceData : Base.BaseData
   {

      public UserResourceData() : base() { }

      [Column(Order = 0, TypeName = "varchar(128)")]
      [StringLength(128), Required]
      public string UserID { get; set; }

      [Column(Order = 1, TypeName = "varchar(128)")]
      [StringLength(128), Required]
      public string ResourceID { get; set; }

   }

}

namespace FriendlyCashFlow.API.Base
{
   partial class dbContext
   {
      internal DbSet<Users.UserResourceData> UserResources { get; set; }

      private void OnModelCreating_UserResources(ModelBuilder modelBuilder)
      {
         modelBuilder.Entity<Users.UserResourceData>()
            .HasKey(c => new { c.UserID, c.ResourceID });
         modelBuilder.Entity<Users.UserResourceData>()
            .HasIndex(x => new { x.RowStatus, x.UserID, x.ResourceID })
            .HasDatabaseName("v6_identityUserResources_index_Search");
      }

   }
}
