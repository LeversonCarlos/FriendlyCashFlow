namespace Lewio.CashFlow;
using Settings;

public class Program
{
   public static void Main(string[] args)
   {
      var builder = WebApplication
         .CreateBuilder(args);

      builder.Services
         .AddSettingsServices(builder.Configuration)
         .AddDataServices(builder.Configuration);

      var app = builder
         .Build();
      app
         .MapGet("/", () => "Hello World!");
      app
         .Run();

   }
}
