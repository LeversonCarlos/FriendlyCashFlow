using System;

namespace migrate
{

   partial class Program
   {

      static object GetInstance(Type migratorType)
      {

         var instance = System.Activator.CreateInstance(migratorType);
         WriteLine(" instance:", ConsoleColor.Yellow);
         WriteLine($"  {instance.ToString()}", ConsoleColor.Yellow);

         if (instance == null)
         {
            WriteLine(" erro:", ConsoleColor.Red);
            WriteLine("  Não foi possivel instanciar o migratorType do assembly", ConsoleColor.Red);
            return null;
         }

         return instance;
      }

   }

}
