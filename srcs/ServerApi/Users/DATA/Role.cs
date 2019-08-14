using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FriendlyCashFlow.API.Users
{

   [Table("v6_identityUserRoles")]
   internal class UserRoleData : Base.BaseData
   {

      [Column(Order = 0)]
      public long UserID { get; set; }

      [Column(Order = 1, TypeName = "varchar(15)")]
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
      }

   }
}
