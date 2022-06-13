namespace Lewio.CashFlow;

public class Program
{
   public static void Main(string[] args)
   {
      var builder = WebApplication.CreateBuilder(args);

      builder.Services
         .ConfigureSettings(builder.Configuration)
         .ConfigureControllers()
         .ConfigureFrontend()
         .ConfigureServices();

      var app = builder.Build();

      app
         .ConfigureEndpoints(app.Environment)
         .ConfigureFrontend(app.Environment);

      app.Run();
   }
}
