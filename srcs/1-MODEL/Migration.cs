using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;

namespace FriendCash.Model
{
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
         oContext.Users.AddOrUpdate(
           p => p.Code,
           new User { idRow=1, idUser=1, Code = "lcjohnny", Password = "******", Description = "Master User", RowStatus= Base.enRowStatus.Active }
         );
         oContext.SaveChanges();
       }

      #endregion

   }
}
