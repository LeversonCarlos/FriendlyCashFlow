using System;
using System.Threading.Tasks;

namespace migrate
{
   public partial class Program
   {
      static async Task Main(string[] args)
      {
         try
         {

            WriteLine();
            WriteLine("Migration Tool", ConsoleColor.Blue);

            var arguments = GetArguments(args);
            if (arguments == null)
               return;

            var assembly = GetAssembly(arguments);
            if (assembly == null)
               return;

            var migratorType = GetMigratorType(assembly);
            if (migratorType == null)
               return;

            var migratorMethod = GetMigratorMethod(migratorType);
            if (migratorMethod == null)
               return;

            var instance = GetInstance(migratorType);
            if (instance == null)
               return;

            await Apply(instance, migratorType, migratorMethod, arguments.ConnStr);

         }
         catch (Exception ex)
         {
            WriteLine($" {ex.ToString()}", ConsoleColor.Red);
         }
         finally { WriteLine(); }
      }
   }
}
