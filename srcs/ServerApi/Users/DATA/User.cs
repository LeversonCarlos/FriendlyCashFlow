using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FriendlyCashFlow.API.Users
{

   [Table("v6_identityUsers")]
   internal class UserData : Base.BaseData
   {

      [Key, Column(TypeName = "varchar(128)")]
      [StringLength(128), Required]
      public string UserID { get; set; }

      [Column(TypeName = "varchar(128)")]
      [StringLength(128), Required]
      public string UserName { get; set; }

      [Column(TypeName = "varchar(max)")]
      [Required]
      public string PasswordHash { get; set; }

      [Column(TypeName = "varchar(500)")]
      [StringLength(500), Required]
      public string Text { get; set; }

      [DataType(DataType.DateTime)]
      public DateTime JoinDate { get; set; }

   }

}

namespace FriendlyCashFlow.API.Base
{
   partial class dbContext
   {
      internal DbSet<Users.UserData> Users { get; set; }
      private void OnModelCreating_Users(ModelBuilder modelBuilder)
      {
         modelBuilder.Entity<Users.UserData>()
            .HasIndex(x => new { x.RowStatus, x.UserID })
            .HasName("v6_identityUsers_index_UserID");
         modelBuilder.Entity<Users.UserData>()
            .HasIndex(x => new { x.RowStatus, x.UserName, x.PasswordHash })
            .HasName("v6_identityUsers_index_Login");
      }
   }
}
