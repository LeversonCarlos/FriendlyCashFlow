using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace FriendlyCashFlow
{

   public class ContextMigrator
   {
      public async Task ApplyAsync(string connStr)
      {
         try
         {
            using (var context = API.Base.dbContext.CreateNewContext(connStr))
            {
               await context.Database.MigrateAsync();
            }
         }
         catch (Exception) { throw; }
      }
   }

}
