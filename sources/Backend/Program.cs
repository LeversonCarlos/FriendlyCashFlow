namespace Lewio.CashFlow;

public class Program
{
   public static void Main(string[] args)
   {
      var builder = WebApplication
         .CreateBuilder(args);

      builder.Services
         .AddSettingsServices(builder.Configuration)
         .AddDataServices(builder.Configuration);

      builder.Services
         .AddAccountsServices(builder.Configuration);

      var app = builder
         .Build();
      app
         .MapGet("/", () => "Hello World!");
      app
         .Run();

   }
}
