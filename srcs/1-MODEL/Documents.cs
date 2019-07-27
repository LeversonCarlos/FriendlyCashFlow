using System;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace FriendCash.Model
{

   [DataContract()]
   [Table("v4_Documents")]
   public class Document : Base
   {

      #region New
      public Document()
      {
         // this.Status = enStatus.Enabled;
       }
      #endregion

      #region idDocument
      [DataMember]
      [Column("idDocument"), ConcurrencyCheck, Display(Description="ID")]
      public long idDocument { get; set; }
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

      public enum enType : short { None = 0, Expense = 1, Income = 2, Transfer = 3 }

      [NotMapped]
      public enType Type
      {
         get { return ((enType)this.TypeValue); }
         set { this.TypeValue = ((short)value); }
      }

      #endregion

      #region idSupplier
      [DataMember]
      [Column("idSupplier"), Display(Description = "Supplier")]
      public long? idSupplier { get; set; }
      #endregion

      #region idPlanning
      [DataMember]
      [Column("idPlanning"), Display(Description = "Planning")]
      public long? idPlanning { get; set; }
      #endregion

      #region Details

      #region SupplierDetails
      public virtual Supplier SupplierDetails
      {
         get
         {
            Supplier oReturn = null;
            Model.Context oContext = null;
            try
            {
               oContext = new Context();
               var oQuery = from DATA in oContext.Suppliers
                            where DATA.idSupplier == this.idSupplier &&
                                  DATA.RowStatusValue == ((short)Model.Base.enRowStatus.Active)
                            select DATA;
               oReturn = oQuery.SingleOrDefault<Supplier>();
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

      #region PlanningDetails
      public virtual Planning PlanningDetails
      {
         get
         {
            Planning oReturn = null;
            Model.Context oContext = null;
            try
            {
               oContext = new Context();
               var oQuery = from DATA in oContext.Plannings
                            where DATA.idPlanning == this.idPlanning &&
                                  DATA.RowStatusValue == ((short)Model.Base.enRowStatus.Active)
                            select DATA;
               oReturn = oQuery.SingleOrDefault<Planning>();
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

      #endregion

   }

 }
