using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using FriendCash.Model.Tools;

namespace FriendCash.Model
{

   #region Context
   public class Context : DbContext
   {

      #region New

      public Context() : base("name=ConnStr")
      { Database.SetInitializer(new MigrateDatabaseToLatestVersion<Context, Migration>()); }

      public Context(Model.Login oLogin) : base(GetConnStr(oLogin))
      { 
         Database.SetInitializer(new MigrateDatabaseToLatestVersion<Context, Migration>());
         Database.CreateIfNotExists();
       }

      public static string GetConnStr(Model.Login oLogin)
      {
         var sReturn = string.Empty;
         sReturn = oLogin.ConnStr;
         return sReturn;
       }

      #endregion

      #region ObjectContext
      public System.Data.Entity.Core.Objects.ObjectContext ObjectContext
      {
         get { return (this as System.Data.Entity.Infrastructure.IObjectContextAdapter).ObjectContext; }
      }
      #endregion

      #region SaveChanges
      public bool SaveChanges(List<Message> oMSG)
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
                        if (oMSG != null) { oMSG.Add(new Message() { Warning = oError.ErrorMessage }); }
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
         catch (Exception ex) { oMSG.Add(new Message() { Exception = ex.Message }); }

         return bReturn;
      }
      #endregion

      #region OnModelCreating
      protected override void OnModelCreating(DbModelBuilder modelBuilder)
      {
         //modelBuilder.Entity<Document>().HasRequired<Supplier>(DATA => DATA.SupplierDetails).WithMany();
       }
      /*
      private System.Collections.Generic.IEnumerable<TMODEL> SupplierExpression<TMODEL>(System.Linq.Expressions.Expression<Func<TMODEL>> expression)
      {
         return this.Set<TMODEL>().Where(expression).AsEnumerable();
       }
      */ 
      #endregion

      public DbSet<Login> Logins { get; set; }
      public DbSet<Account> Accounts { get; set; }
      public DbSet<Supplier> Suppliers { get; set; }
      public DbSet<Planning> Plannings { get; set; }
      public DbSet<Document> Documents { get; set; }
      public DbSet<History> Historys { get; set; }
      public DbSet<Import> Imports { get; set; }
      public DbSet<ImportStatus> ImportsStatus { get; set; }
      public DbSet<ImportData> ImportDatas { get; set; }
      public DbSet<Recurrent> Recurrents { get; set; }

   }
   #endregion

   #region Migration
   internal sealed class Migration : DbMigrationsConfiguration<Context>
   {

      #region New
      public Migration()
      {
         AutomaticMigrationsEnabled = true;
         AutomaticMigrationDataLossAllowed = false;
      }
      #endregion

      #region Seed

      protected override void Seed(Context oContext)
      {
         this.Seed_User(oContext);
      }

      private void Seed_User(Context oContext)
      {
         /*
         oContext.Users.AddOrUpdate(
           p => p.Code,
           new User { idRow=1, idUser=1, Code = "lcjohnny", Password = "*******", Description = "Master User", RowStatus= Base.enRowStatus.Active }
         );
         oContext.SaveChanges();
          */
      }

      #endregion

   }
   #endregion

}
