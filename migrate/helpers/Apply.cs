using System;
using System.Threading.Tasks;

namespace migrate
{

   partial class Program
   {

      static async Task Apply(object instance, Type migratorType, System.Reflection.MethodInfo migratorMethod, string connStr)
      {
         WriteLine(" Iniciando Migration", ConsoleColor.Green);
         var migratorParams = new object[] { connStr };
         var task = (Task)migratorMethod.Invoke(instance, migratorParams);
         await task;
         WriteLine(" Finalizando Migration", ConsoleColor.Green);
      }

   }

}
