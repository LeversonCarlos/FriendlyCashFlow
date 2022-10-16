namespace Lewio.CashFlow;

public class Program
{
   public static void Main(string[] args)
   {
      var builder = WebApplication
         .CreateBuilder(args);

      builder.Services
         .AddSettingsServices(builder.Configuration)
         .AddDataServices(builder.Configuration)
         .AddControllersServices(builder.Configuration);

      builder.Services
         .AddAccountsRepository()
         .AddAccountsCommands();

      builder.Services
         .AddScoped<Users.LoggedInUser>(sp => Users.LoggedInUser.Create("DummyUser"));

      var app = builder
         .Build()
         .AddControllersServices();
      app.Run();

   }
}
