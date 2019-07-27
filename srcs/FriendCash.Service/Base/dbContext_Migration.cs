using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;

namespace FriendCash.Service.Base
{
   internal sealed class dbContext_Migration : DbMigrationsConfiguration<dbContext>
   {

      #region New
      public dbContext_Migration()
      {
         this.AutomaticMigrationsEnabled = true;
         this.AutomaticMigrationDataLossAllowed = true;
      }
      #endregion

      #region Seed

      protected override void Seed(dbContext oContext)
      {
         this.Seed_User(oContext);
      }

      private void Seed_User(dbContext oContext)
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
}