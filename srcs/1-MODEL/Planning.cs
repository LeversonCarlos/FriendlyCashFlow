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
      [Column("idPlanning"), ConcurrencyCheck, Display(Description = "ID")]
      public long idPlanning { get; set; }
      #endregion

      #region Description
      [DataMember]
      [Column("Description"), StringLength(500), Required, Display(Description = "Description")]
      public string Description { get; set; }
      #endregion

      #region Type

      [DataMember]
      [Column("Type"), Required]
      public short TypeValue { get; set; }

      [NotMapped]
      public Document.enType Type
      {
         get { return ((Document.enType)this.TypeValue); }
         set { this.TypeValue = ((short)value); }
      }

      #endregion

      #region idParentRow
      [DataMember]
      [Column("idParentRow"), Display(Description="Parent")]
      public long? idParentRow { get; set; }
      #endregion

      #region Childs
      public virtual IEnumerable<Planning> Childs
      {
         get
         {
            IEnumerable<Planning> oReturn = null;
            Model.Context oContext = null;
            try
            {
               oContext = new Context();
               var oQuery = from DATA in oContext.Plannings
                            where DATA.idParentRow == this.idPlanning &&
                                  DATA.RowStatusValue == ((short)Model.Base.enRowStatus.Active)
                            orderby DATA.Description
                            select DATA;
               oReturn = oQuery.ToList<Planning>();
            }
            catch (Exception) { }
            finally
            {
               if (oContext != null)
               {
                  oContext.Dispose();
                  oContext = null;
               }
            }
            return oReturn;
         }
      }
      #endregion

   }

}
