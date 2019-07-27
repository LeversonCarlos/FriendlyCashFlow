#region Using
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
#endregion

namespace FriendCash.Service.Entries.Model
{

   [Table("v5_dataPatterns")]
   public class bindPattern : Base.BaseModel
   {

      #region New
      public bindPattern()
      {
         this.idPattern = -1;
      }
      #endregion

      #region idPattern
      [Column("idPattern"), Key]
      [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
      public long idPattern { get; set; }
      #endregion

      #region idUser
      [Column("idUser", TypeName = "varchar")]
      [StringLength(128), Required]
      [Index("IDX_PATTERN_MAIN", Order = 1)]
      public string idUser { get; set; }
      #endregion

      #region Type

      [Column("Type"), Required]
      public short TypeValue { get; set; }

      [NotMapped]
      public enEntryType Type
      {
         get { return ((enEntryType)this.TypeValue); }
         set { this.TypeValue = ((short)value); }
      }

      #endregion

      #region Text
      [Column("Text", TypeName = "varchar")]
      [StringLength(500), Required]
      public string Text { get; set; }
      #endregion

      #region idCategory

      [Column("idCategory")]
      public long idCategory { get; set; }

      [ForeignKey("idCategory")]
      public virtual Categories.Model.bindCategory idCategoryModel { get; set; }

      #endregion

      #region Hash
      [Column("Hash", TypeName = "varchar")]
      [StringLength(32), Required]
      [Index("IDX_PATTERN_MAIN", Order = 2)]
      public string Hash { get; set; }
      #endregion

      #region Quantity
      [Column("Quantity")]
      public long Quantity { get; set; }
      #endregion

      #region RowStatus
      [Index("IDX_PATTERN_MAIN", Order = 0)]
      [Column("RowStatus")]
      public override short RowStatusValue { get; set; }
      #endregion


      #region GetHash
      public string GetHash()
      {
         try
         {
            var tempModel = new { Type, Text, idCategory };
            var serializedModel = FriendCash.Model.Base.Json.Serialize(tempModel);
            var hashedModel = FriendCash.Model.Base.Hash.Execute(serializedModel);
            return hashedModel;
         }
         catch { return string.Empty; }
      }
      #endregion

      #region Create

      public static bindPattern Create(viewEntry value)
      {
         return new Model.bindPattern()
         {
            Type = value.Type,
            Text = value.Text,
            idCategory = value.idCategory.Value
         };
      }

      #endregion

   }

}
