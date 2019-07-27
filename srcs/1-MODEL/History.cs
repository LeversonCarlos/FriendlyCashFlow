using System;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace FriendCash.Model
{

   [DataContract()]
   [Table("v4_History")]
   public class History : Base
   {

      #region New
      public History()
      {
         this.Settled = false;
       }
      #endregion

      #region idHistory
      [DataMember]
      [Column("idHistory"), ConcurrencyCheck, Display(Description = "ID")]
      public long idHistory { get; set; }
      #endregion

      #region idDocument
      [DataMember]
      [Column("idDocument"), Required, Display(Description = "Document")]
      public long idDocument { get; set; }
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

      #region DueDate
      [DataMember]
      [Column("DueDate"), Required, Display(Description = "Due Date")]
      public DateTime DueDate { get; set; }
      #endregion

      #region Value
      [DataMember]
      [Column("Value"), Required, Display(Description = "Value")]
      public double Value { get; set; }
      #endregion

      #region Settled
      [DataMember]
      [Column("Settled"), Required, Display(Description = "Settled")]
      public bool Settled { get; set; }
      #endregion

      #region idAccount
      [DataMember]
      [Column("idAccount"), Display(Description = "Account")]
      public long? idAccount { get; set; }
      #endregion

      #region PayDate
      [DataMember]
      [Column("PayDate"), Display(Description = "Pay Date")]
      public DateTime? PayDate { get; set; }
      #endregion

      #region idTransfer
      [DataMember]
      [Column("idTransfer"), Display(Description = "Transfer")]
      public long? idTransfer { get; set; }
      #endregion

      #region Sorting

      [DataMember]
      [Column("Sorting"), Display(Description = "Sorting")]
      public double Sorting { get; set; }

      public void SortingRefresh()
      {
         DateTime oInitial = new DateTime(1901, 1, 1);
         DateTime oFinal = (this.Settled == true && this.PayDate.HasValue == true ? this.PayDate.Value : this.DueDate);
         double iInterval = Convert.ToInt64(oFinal.Subtract(oInitial).TotalDays);
         //iInterval = iInterval * 100000;
         iInterval += Convert.ToDouble(this.idHistory) / Math.Pow(10, this.idHistory.ToString().Length);
         this.Sorting = iInterval;
       }

      #endregion

      #region Details

      #region DocumentDetails
      public virtual Document DocumentDetails
      {
         get
         {
            Document oReturn = null;
            Model.Context oContext = null;
            try
            {
               oContext = new Context();
               var oQuery = from DATA in oContext.Documents
                            where DATA.idDocument == this.idDocument &&
                                  DATA.RowStatusValue == ((short)Model.Base.enRowStatus.Active)
                            select DATA;
               oReturn = oQuery.SingleOrDefault<Document>();
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

      #region AccountDetails
      public virtual Account AccountDetails
      {
         get
         {
            Account oReturn = null;
            Model.Context oContext = null;
            try
            {
               oContext = new Context();
               var oQuery = from DATA in oContext.Accounts
                            where DATA.idAccount == this.idAccount &&
                                  DATA.RowStatusValue == ((short)Model.Base.enRowStatus.Active)
                            select DATA;
               oReturn = oQuery.SingleOrDefault<Account>();
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
