using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FriendlyCashFlow.API.Users
{
   public enum UserRoleEnum : short { None = 0, Viewer = 1, Editor = 2, Owner = 3 }

   [Table("v6_identityUserRoles")]
   internal class UserRoleData : Base.BaseData
   {

      public UserRoleData() : base()
      {
         this.RowStatus = (short)Base.enRowStatus.Active;
      }

      [Column(Order = 0, TypeName = "varchar(128)")]
      [StringLength(128), Required]
      public string UserID { get; set; }

      [Column(Order = 1, TypeName = "varchar(128)")]
      [StringLength(128), Required]
      public string ResourceID { get; set; }

      [Column(Order = 2, TypeName = "varchar(15)")]
      [StringLength(15), Required]
      public string RoleID { get; set; }

   }

}

namespace FriendlyCashFlow.API.Base
{
   partial class dbContext
   {
      internal DbSet<Users.UserRoleData> UserRoles { get; set; }

      private void OnModelCreating_UserRoles(ModelBuilder modelBuilder)
      {
         modelBuilder.Entity<Users.UserRoleData>()
            .HasKey(c => new { c.UserID, c.RoleID });
         modelBuilder.Entity<Users.UserRoleData>()
            .HasIndex(x => new { x.RowStatus, x.UserID, x.ResourceID, x.RoleID })
            .HasDatabaseName("v6_identityUserRoles_index_Search");
      }

   }
}
