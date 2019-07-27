#region Using
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
#endregion 

namespace FriendCash.Service.Base
{
   internal partial class dbContext : DbContext, IDisposable
   {

      #region New
      public dbContext() : base("name=DefaultConnection")
      {        
         Configuration.ProxyCreationEnabled = false;
         Configuration.LazyLoadingEnabled = false;
         // Database.SetInitializer(new MigrateDatabaseToLatestVersion<dbContext, dbContext_Migration>());
         Database.SetInitializer<dbContext>(null);
      }
      #endregion

      #region ObjectContext
      public System.Data.Entity.Core.Objects.ObjectContext ObjectContext
      {
         get { return (this as System.Data.Entity.Infrastructure.IObjectContextAdapter).ObjectContext; }
      }
      #endregion

      #region SaveChanges
      public new Bundle<bool> SaveChanges()
      {
         var oReturn = new Bundle<bool>();

         try
         {

            // VALIDATE
            IEnumerable<System.Data.Entity.Validation.DbEntityValidationResult> oResults = this.GetValidationErrors();
            if (oResults != null && oResults.Count() != 0)
            {
               foreach (System.Data.Entity.Validation.DbEntityValidationResult oResult in oResults)
               {
                  if (oResult.ValidationErrors != null && oResult.ValidationErrors.Count != 0)
                  {
                     foreach (System.Data.Entity.Validation.DbValidationError oError in oResult.ValidationErrors)
                     {
                        oReturn.Messages.Add(new BundleMessage(oError.ErrorMessage, BundleMessage.enumType.Warning)); 
                     }
                  }
               }
            }

            // APPLY
            if (oReturn.Messages.Count() != 0) 
            {
               int iResult = base.SaveChanges();
               oReturn.Result = true;
               //this.Database.Connection.Commit
            }

         }
         catch (Exception ex) { oReturn.Messages.Add(new BundleMessage(ex.Message, BundleMessage.enumType.Alert)); }

         return oReturn;
      }
      #endregion      

   }
}