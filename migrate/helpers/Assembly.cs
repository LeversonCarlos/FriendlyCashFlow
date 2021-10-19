using System;

namespace migrate
{

   partial class Program
   {

      static System.Reflection.Assembly GetAssembly(Arguments arguments)
      {

         var assembly = System.Reflection.Assembly.LoadFrom(arguments.AssemblyPath);
         WriteLine(" assemblyName:", ConsoleColor.Yellow);
         WriteLine($"  {assembly.FullName}", ConsoleColor.Yellow);

         return assembly;
      }

   }

}
