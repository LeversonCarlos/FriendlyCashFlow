using System;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace FriendCash.Model
{

   [DataContract()]
   [Table("v4_Suppliers")]
   public class Supplier : Base
   {

      #region New
      public Supplier()
      {
         // this.Status = enStatus.Enabled;
       }
      #endregion

      #region idSupplier
      [DataMember]
      [Column("idSupplier"), Index("IX_SupplierKey")]
      [Display(Name = "COLUMN_ID", ResourceType = typeof(FriendCash.Resources.Suppliers))]
      public long idSupplier { get; set; }
      #endregion

      #region Code
      [DataMember]
      [Column("Code", TypeName = "varchar"), StringLength(50)]
      [Required(ErrorMessageResourceName = "MSG_REQUIRED_CODE", ErrorMessageResourceType = typeof(FriendCash.Resources.Suppliers))]
      [Display(Name = "COLUMN_CODE", ResourceType = typeof(FriendCash.Resources.Suppliers))]
      public string Code { get; set; }
      #endregion

      #region Description
      [DataMember]
      [Column("Description", TypeName = "varchar"), StringLength(500)]
      [Required(ErrorMessageResourceName = "MSG_REQUIRED_DESCRIPTION", ErrorMessageResourceType = typeof(FriendCash.Resources.Suppliers))]
      [Display(Name = "COLUMN_DESCRIPTION", ResourceType = typeof(FriendCash.Resources.Suppliers))]
      public string Description { get; set; }
      #endregion

    }

 }
