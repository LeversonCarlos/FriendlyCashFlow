using System;

namespace migrate
{

   partial class Program
   {

      static Arguments GetArguments(string[] args)
      {

         if (args == null || args.Length != 2)
         {
            WriteLine(" forma de usar:", ConsoleColor.Red);
            WriteLine("  migrate.exe ./assembly.dll \"Data Source=hostName;Initial Catalog=baseName;Persist Security Info=True;User ID=userName;Password=password\"", ConsoleColor.Red);
            return null;
         }

         var assemblyPath = args[0];
         assemblyPath = System.IO.Path.GetFullPath(assemblyPath);
         WriteLine(" assemblyPath:", ConsoleColor.Yellow);
         WriteLine($"  {assemblyPath}", ConsoleColor.Yellow);

         if (!System.IO.File.Exists(assemblyPath))
         {
            WriteLine(" erro:", ConsoleColor.Red);
            WriteLine("  O assembly não foi encontrado no caminho", ConsoleColor.Red);
            return null;
         }

         var connStr = args[1];
         WriteLine(" connStr:", ConsoleColor.Yellow);
         WriteLine($"  {connStr}", ConsoleColor.Yellow);

         var arguments = new Arguments(assemblyPath, connStr);
         return arguments;
      }

   }

   internal class Arguments
   {
      public Arguments(string assemblyPath, string connStr)
      {
         AssemblyPath = assemblyPath;
         ConnStr = connStr;
      }
      public string AssemblyPath { get; }
      public string ConnStr { get; }
   }

}
