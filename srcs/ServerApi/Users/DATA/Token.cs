using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FriendlyCashFlow.API.Users
{

   [Table("v6_identityUserTokens")]
   internal class UserTokenData
   {

      [Column(Order = 0, TypeName = "varchar(128)")]
      [StringLength(128), Required]
      public string UserID { get; set; }

      [Column(Order = 1, TypeName = "varchar(128)")]
      [StringLength(128), Required]
      public string RefreshToken { get; set; }

      [DataType(DataType.DateTime)]
      public DateTime ExpirationDate { get; set; }

   }

}

namespace FriendlyCashFlow.API.Base
{
   partial class dbContext
   {
      internal DbSet<Users.UserTokenData> UserTokens { get; set; }
      private void OnModelCreating_UserToken(ModelBuilder modelBuilder)
      {
         modelBuilder.Entity<Users.UserTokenData>()
            .HasKey(c => new { c.UserID, c.RefreshToken });
         modelBuilder.Entity<Users.UserTokenData>()
            .HasIndex(x => new { x.RefreshToken })
            .HasName("v6_identityUserTokens_index_Search");
      }
   }
}
