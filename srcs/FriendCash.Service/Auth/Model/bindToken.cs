#region Using
using System;
using System.ComponentModel.DataAnnotations;
#endregion

namespace FriendCash.Auth.Model
{
   public class bindToken
   {

      [Key]
      public string Id { get; set; }

      [Required]
      public string UserName { get; set; }

      [Required]
      public string ClientId { get; set; }

      public DateTime IssuedTime { get; set; }
      public DateTime ExpiryTime { get; set; }

      [Required]
      public string Ticket { get; set; }

   }
}