using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FriendlyCashFlow.API.Users
{

   [Table("v6_identityUsers")]
   internal class UserData : Base.BaseData
   {

      [Key, DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
      public long UserID { get; set; }

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
   }
}
