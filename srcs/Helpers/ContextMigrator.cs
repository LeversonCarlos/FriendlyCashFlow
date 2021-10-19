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
            Console.WriteLine("  Criando Contexto");
            using (var ctx = API.Base.dbContext.CreateNewContext(connStr))
            {
               Console.WriteLine("  Contexto Criado");
               Console.WriteLine("  Aplicando Migration");
               await ctx.Database.MigrateAsync();
               Console.WriteLine("  Migration Aplicado");
            }
         }
         catch (Exception) { throw; }
      }
   }

}
