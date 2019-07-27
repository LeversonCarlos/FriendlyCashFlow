#region Using
using System.ComponentModel.DataAnnotations;
#endregion

namespace FriendCash.Auth.Model
{
   public class bindClient
   {

      [Key]
      public string Id { get; set; }

      [Required, MaxLength(256)]
      public string Name { get; set; }

      public viewClient.enumType Type { get; set; }

      public int RefreshTokenLifetime { get; set; }

      [MaxLength(128)]
      public string AllowedOrigin { get; set; }

      [Required]
      public string Secret { get; set; }

      public bool Active { get; set; }

   }
}