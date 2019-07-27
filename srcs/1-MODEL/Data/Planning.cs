using System;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Collections.Generic;

namespace FriendCash.Model
{

   [DataContract()]
   [Table("v4_Planning")]
   public class Planning : Base
   {

      #region New
      public Planning()
      {
         this.Type = Document.enType.None;
       }
      #endregion

      #region idPlanning
      [DataMember]
      [Column("idPlanning"), Index("IX_PlanningKey")]
      [Display(Name = "COLUMN_ID", ResourceType = typeof(FriendCash.Resources.Planning))]
      public long idPlanning { get; set; }
      #endregion

      #region Description
      [DataMember]
      [Column("Description", TypeName = "varchar"), StringLength(500)]
      [Required(ErrorMessageResourceName = "MSG_REQUIRED_DESCRIPTION", ErrorMessageResourceType = typeof(FriendCash.Resources.Planning))]
      [Display(Name = "COLUMN_DESCRIPTION", ResourceType = typeof(FriendCash.Resources.Planning))]
      public string Description { get; set; }
      #endregion

      #region Type

      [DataMember]
      [Column("Type"), Required]
      public short TypeValue { get; set; }

      [NotMapped]
      [Display(Name = "COLUMN_TYPE", ResourceType = typeof(FriendCash.Resources.Planning))]
      public Document.enType Type
      {
         get { return ((Document.enType)this.TypeValue); }
         set { this.TypeValue = ((short)value); }
      }

      #endregion

      #region idParentRow
      [DataMember]
      [Column("idParentRow"), Index("IX_Planning")]
      [Display(Name = "COLUMN_PARENT", ResourceType = typeof(FriendCash.Resources.Planning))]
      public long? idParentRow { get; set; }
      #endregion

      #region Childs
      [ForeignKey("idParentRow")]
      public virtual ICollection<Planning> Childs { get; set; }
      #endregion

   }

   #region ViewPlanningFlow
   public class ViewPlanningFlow
   {

      #region idPlanning
      public long idPlanning { get; set; }
      #endregion

      #region Description
      [Display(Name = "COLUMN_DESCRIPTION", ResourceType = typeof(FriendCash.Resources.Planning))]
      public string Description { get; set; }
      #endregion

      #region Type
      public Document.enType Type { get; set; }
      #endregion

      #region Date
      [DataType(System.ComponentModel.DataAnnotations.DataType.Date)]
      public DateTime Date { get; set; }
      public int DateYear { get; set; }
      public int DateMonth { get; set; }
      #endregion

      #region Value
      public double Value { get; set; }
      #endregion

      #region Childrens
      public List<ViewPlanningFlow> Childrens { get; set; }
      #endregion

   }
   #endregion

}
