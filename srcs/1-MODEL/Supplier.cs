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
      [Column("idSupplier"), ConcurrencyCheck, Display(Description = "ID")]
      public long idSupplier { get; set; }
      #endregion

      #region Code
      [DataMember]
      [Column("Code"), StringLength(50), Display(Description="Code"), Required(ErrorMessage="The 'Code' is Required")]
      public string Code { get; set; }
      #endregion

      #region Description
      [DataMember]
      [Column("Description"), StringLength(500), Required, Display(Description = "Description")]
      public string Description { get; set; }
      #endregion

    }

 }
