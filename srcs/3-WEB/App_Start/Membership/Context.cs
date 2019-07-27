using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity.Migrations;

namespace FriendCash.Model.Membership
{

   #region Context
   public class Context : DbContext
   {

      #region New
      public Context() : base("MembershipConnStr")
      {
         Database.SetInitializer(new MigrateDatabaseToLatestVersion<Context, Migration>());
      }
      #endregion

      public DbSet<UserProfile> UserProfiles { get; set; }
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

   }
   #endregion

   #region ApplicationUser
   // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
   public class ApplicationUser : IdentityUser
   {
   }
   #endregion

   #region ApplicationDbContext
   public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
   {
      public ApplicationDbContext() : base("MembershipConnStr")
      {
      }
   }
   #endregion


}
