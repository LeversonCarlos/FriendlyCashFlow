#region Using
using System;
using System.ComponentModel.DataAnnotations;
#endregion

namespace FriendCash.Auth.Model
{

   public class viewSignature
   {
      public long idSignature { get; set; }

      [Display(Description = "LABEL_AUTH_SIGNATURE_DUEDATE")]
      [DataType(DataType.DateTime)]
      public DateTime DueDate { get; set; }

      [Display(Description = "LABEL_AUTH_SIGNATURE_ROWDATE")]
      [DataType(DataType.DateTime)]
      public DateTime RowDate { get; set; }

      [Display(Description = "LABEL_AUTH_SIGNATURE_STATUS")]
      public enumSignatureStatus Status { get; set; }
      public enum enumSignatureStatus : short { Expired = -1, Temporary = 0, Active = 1 }

   }

   public class editSignature : viewSignature
   {

      [Display(Description = "LABEL_USERS_ID")]
      [Required(ErrorMessage = "MSG_AUTH_USERS_ID_INVALID")]
      public string idUser { get; set; }

      [Display(Description = "LABEL_USERSSIGNATURE_DAYS")]
      [Required(ErrorMessage = "MSG_AUTH_USERSSIGNATURE_DAYS_INVALID")]
      public short days { get; set; }

   }

}