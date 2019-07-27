#region Using
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
#endregion

namespace FriendCash.Service.Categories.Model
{

   [Table("v5_dataCategories")]
   public class bindCategory : Base.BaseModel
   {

      #region New
      public bindCategory()
      {
         this.idCategory = -1;
         this.Type = enCategoryType.None;
       }
      #endregion

      #region idCategory
      [Column("idCategory"), Key]
      [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
      public long idCategory { get; set; }
      #endregion

      #region idUser
      [Column("idUser", TypeName = "varchar")]
      [StringLength(128), Required]
      public string idUser { get; set; }
      #endregion

      #region Text
      [Column("Text", TypeName = "varchar")]
      [StringLength(500), Required]
      public string Text { get; set; }
      #endregion

      #region Type

      [Column("Type")]
      public short TypeValue { get; set; }

      [NotMapped]
      public enCategoryType Type
      {
         get { return ((enCategoryType)this.TypeValue); }
         set { this.TypeValue = ((short)value); }
      }

      #endregion

      #region idParentRow
      [Column("idParentRow")]
      public long? idParentRow { get; set; }
      #endregion

      #region HierarchyText
      [Column("HierarchyText", TypeName = "varchar")]
      [StringLength(4000)]
      public string HierarchyText { get; set; }
      #endregion

   }

}
