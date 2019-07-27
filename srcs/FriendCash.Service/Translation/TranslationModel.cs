#region Using
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
#endregion

namespace FriendCash.Service.Translation
{

   [Table("v5_sysTranslation")]
   public class TranslationModel
   {

      #region idLanguage
      [Column("idLanguage", TypeName = "varchar", Order = 0), Key]
      [Display(Description = "LABEL_SYSTRANSLATION_IDLANGUAGE")]
      public string idLanguage { get; set; }
      #endregion

      #region idTranslation
      [Column("idTranslation", TypeName = "varchar", Order = 1), Key]
      [StringLength(255)]
      [Display(Description = "LABEL_SYSTRANSLATION_IDTRANSLATION")]
      public string idTranslation { get; set; }
      #endregion

      #region Text
      [Column("Text", TypeName = "varchar")]
      [StringLength(4000), Required]
      [Display(Description = "LABEL_SYSTRANSLATION_TEXT")]
      public string Text { get; set; }
      #endregion

   }
}