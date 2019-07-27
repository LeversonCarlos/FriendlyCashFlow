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
   public enum enCategoryType : short { None = 0, Expense = 1, Income = 2 }

   public class viewCategory
   {

      #region New
      public viewCategory() { }
      public viewCategory(bindCategory value) : this()
      {
         this.idCategory = value.idCategory;
         this.Text = value.Text;
         this.HierarchyText = value.HierarchyText;
         this.Type = value.Type;
         this.idParentRow = value.idParentRow;
      }
      #endregion

      #region idCategory
      [Display(Description = "LABEL_CATEGORIES_IDCATEGORY")]
      public long idCategory { get; set; }
      #endregion

      #region Text
      [StringLength(500, ErrorMessage = "MSG_CATEGORIES_TEXT_MAXLENGTH")]
      [Required(ErrorMessage = "MSG_CATEGORIES_TEXT_REQUIRED")]
      [Display(Description = "LABEL_CATEGORIES_TEXT")]
      public string Text { get; set; }
      #endregion

      #region Type
      public short TypeValue { get; set; }
      [Display(Description = "LABEL_CATEGORIES_TYPE")]
      public enCategoryType Type
      {
         get { return ((enCategoryType)this.TypeValue); }
         set { this.TypeValue = ((short)value); }
      }
      #endregion

      #region idParentRow
      [Display(Description = "LABEL_CATEGORIES_PARENT")]
      public long? idParentRow { get; set; }
      #endregion

      #region HierarchyText
      [StringLength(4000, ErrorMessage = "MSG_CATEGORIES_HIERARCHYTEXT_MAXLENGTH")]
      [Display(Description = "LABEL_CATEGORIES_HIERARCHYTEXT")]
      public string HierarchyText { get; set; }
      #endregion


      #region Childs
      public virtual List<viewCategory> Childs { get; set; }
      #endregion

   }
}