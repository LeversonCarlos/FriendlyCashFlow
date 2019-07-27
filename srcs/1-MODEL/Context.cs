using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;

namespace FriendCash.Model
{

   public class Context : DbContext
   {

      #region New
      public Context() : base("name=ConnStr") 
      {
         Database.SetInitializer(new MigrateDatabaseToLatestVersion<Context, Migration>());
       }
      #endregion

      #region ObjectContext
      public System.Data.Objects.ObjectContext ObjectContext
      {
         get { return (this as System.Data.Entity.Infrastructure.IObjectContextAdapter).ObjectContext; }
      }
      #endregion

      #region SaveChanges
      public bool SaveChanges(List<string> oMSG)
      {
         bool bReturn = false;

         try
         {

            // VALIDATE
            bool bHasErrors = false;
            IEnumerable<System.Data.Entity.Validation.DbEntityValidationResult> oResults = this.GetValidationErrors();
            if (oResults != null && oResults.Count() != 0)
            {
               foreach (System.Data.Entity.Validation.DbEntityValidationResult oResult in oResults)
               {
                  if (oResult.ValidationErrors != null && oResult.ValidationErrors.Count != 0)
                  {
                     foreach (System.Data.Entity.Validation.DbValidationError oError in oResult.ValidationErrors)
                     {
                        oMSG.Add(oError.ErrorMessage);
                        bHasErrors = true;
                     }
                  }
               }
            }
            if (bHasErrors == true)
             { return bReturn; }

            // SAVE CHANGES
            int iResult = base.SaveChanges();

            // COMMIT
            //this.Database.Connection.Commit

            // OK
            bReturn = true;

          }
         catch (Exception ex) { oMSG.Add(ex.Message); }

         return bReturn;
       }
      #endregion

      public DbSet<User> Users { get; set; }
      public DbSet<Login> Logins { get; set; }
      public DbSet<Account> Accounts { get; set; }
      public DbSet<Supplier> Suppliers { get; set; }
      public DbSet<Planning> Plannings { get; set; }
      public DbSet<Document> Documents { get; set; }
      public DbSet<History> Historys { get; set; }

    }

}
