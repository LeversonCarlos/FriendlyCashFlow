using System;

namespace migrate
{
   partial class Program
   {

      static void WriteLine(string text, ConsoleColor color)
      {
         Console.ForegroundColor = color;
         Console.WriteLine(text);
         Console.ResetColor();
      }

      static void WriteLine(string text = "")
      {
         Console.WriteLine(text);
      }

   }
}
