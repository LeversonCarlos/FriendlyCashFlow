namespace Lewio.CashFlow;

public class Program
{
   public static void Main(string[] args)
   {
      var builder = WebApplication.CreateBuilder(args);

      builder.Services
         .ConfigureSettings(builder.Configuration)
         .ConfigureLocalization()
         .ConfigureControllers()
         .ConfigureFrontend()
         .ConfigureServices();

      var app = builder.Build();

      app
         .ConfigureLocalization(app.Environment)
         .ConfigureEndpoints(app.Environment)
         .ConfigureFrontend(app.Environment);

      app.Run();
   }
}
