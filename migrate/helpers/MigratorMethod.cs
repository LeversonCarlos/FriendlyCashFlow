using System;

namespace migrate
{

   partial class Program
   {

      static System.Reflection.MethodInfo GetMigratorMethod(Type migratorType)
      {

         var migratorMethodName = "ApplyAsync";
         WriteLine(" migratorMethod:", ConsoleColor.Yellow);
         WriteLine($"  {migratorMethodName}", ConsoleColor.Yellow);

         var migratorMethod = migratorType.GetMethod(migratorMethodName);
         if (migratorMethod == null)
         {
            WriteLine(" erro:", ConsoleColor.Red);
            WriteLine("  Não foi localizado o migratorMethod no migratorType do assembly", ConsoleColor.Red);
            return null;
         }

         return migratorMethod;
      }

   }

}
