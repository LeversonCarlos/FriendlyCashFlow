using System;

namespace migrate
{

   partial class Program
   {

      static Type GetMigratorType(System.Reflection.Assembly assembly)
      {

         var migratorTypeName = $"{assembly.ManifestModule.Name.Replace(".dll", "")}.ContextMigrator";
         WriteLine(" migratorType:", ConsoleColor.Yellow);
         WriteLine($"  {migratorTypeName}", ConsoleColor.Yellow);

         var migratorType = assembly.GetType(migratorTypeName);
         if (migratorType == null)
         {
            WriteLine(" erro:", ConsoleColor.Red);
            WriteLine("  Não foi localizado o migratorType no assembly", ConsoleColor.Red);
            return null;
         }

         return migratorType;
      }

   }

}
