#region Using
#endregion

using System.ComponentModel.DataAnnotations;

namespace FriendCash.Auth.Model
{

   public class viewClient
   {
      public const string RegexName = "(.{2,100})";

      public string Url { get; set; }
      public string ID { get; set; }

      [Required(ErrorMessage = "MSG_AUTH_CLIENTS_NAME_REQUIRED")]
      [RegularExpression(RegexName, ErrorMessage = "MSG_AUTH_CLIENTS_NAME_INVALID")]
      [Display(Description = "LABEL_AUTHS_CLIENTS_NAME")]
      public string Name { get; set; }

      [Display(Description = "LABEL_AUTH_CLIENTS_TYPE")]
      public enumType Type { get; set; }
      public enum enumType : short { JavaScript = 0, NativeConfidential = 1 };

      [Display(Description = "LABEL_AUTH_CLIENTS_LIFETIME")]
      [Range(60, 43200, ErrorMessage = "MSG_AUTH_CLIENTS_LIFETIME_INVALID")]
      public int RefreshTokenLifetime { get; set; }

      [Display(Description = "LABEL_AUTH_CLIENTS_ALLOWEDORIGIN")]
      [MaxLength(128, ErrorMessage = "MSG_AUTH_CLIENTS_ALLOWEDORIGIN_INVALID")]
      public string AllowedOrigin { get; set; }

      [Display(Description = "LABEL_AUTH_CLIENTS_Active")]
      public bool Active { get; set; }

   }

   public class editClient : viewClient
   {

      [Display(Description = "LABEL_AUTH_CLIENTS_ID")]
      [Required(ErrorMessage = "MSG_AUTH_CLIENTS_ID_INVALID")]
      public new string ID { get; set; }

      [Display(Description = "LABEL_AUTH_CLIENTS_SECRET")]
      [Required(ErrorMessage = "MSG_AUTH_CLIENTS_SECRET_INVALID")]
      public string Secret { get; set; }

   }

}