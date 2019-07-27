using System;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace FriendCash.Model
{

   public class Transfer : Model.Base
   {

      #region idTransfer
      public long idTransfer { get; set; }
      #endregion

      #region idDocument
      public long idDocument { get; set; }
      #endregion

      #region DueDate
      public DateTime DueDate { get; set; }
      #endregion

      #region Value
      public double Value { get; set; }
      #endregion

      #region Settled
      public bool Settled { get; set; }
      #endregion

      #region idAccountIncome
      public long? idAccountIncome { get; set; }
      #endregion

      #region idAccountExpense
      public long? idAccountExpense { get; set; }
      #endregion

      #region idRowIncome
      public Int64 idRowIncome { get; set; }
      #endregion

      #region idRowExpense
      public Int64 idRowExpense { get; set; }
      #endregion

      #region PayDate
      public DateTime? PayDate { get; set; }
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

      #region AccountIncomeDetails
      public virtual Account AccountIncomeDetails
      {
         get
         {
            Account oReturn = null;
            Model.Context oContext = null;
            try
            {
               oContext = new Context();
               var oQuery = from DATA in oContext.Accounts
                            where DATA.idAccount == this.idAccountIncome &&
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

      #region AccountExpenseDetails
      public virtual Account AccountExpenseDetails
      {
         get
         {
            Account oReturn = null;
            Model.Context oContext = null;
            try
            {
               oContext = new Context();
               var oQuery = from DATA in oContext.Accounts
                            where DATA.idAccount == this.idAccountExpense &&
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
