namespace Lewio.CashFlow;

partial class StartupExtensions
{

   public static IServiceCollection ConfigureFrontend(this IServiceCollection serviceCollection)
   {

      /*
      serviceCollection
         .AddSpaStaticFiles(config =>
         {
            config.RootPath = "ClientApp/dist";
         });
      */

      return serviceCollection;
   }

   public static IApplicationBuilder ConfigureFrontend(this IApplicationBuilder applicationBuilder, IWebHostEnvironment env)
   {

      /*
      if (!env.IsDevelopment())
         applicationBuilder.UseSpaStaticFiles();
      applicationBuilder.UseSpa(spa =>
      {
         spa.Options.SourcePath = "../FlowFrontend";
         if (env.IsDevelopment())
         {
            // spa.Options.StartupTimeout = System.TimeSpan.FromMinutes(3);
            spa.UseAngularCliServer(npmScript: "start");
         }
      });
      */

      return applicationBuilder;
   }

}
