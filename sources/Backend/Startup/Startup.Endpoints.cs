namespace Friendly.CashFlow;

partial class StartupExtensions
{

   public static IApplicationBuilder ConfigureEndpoints(this IApplicationBuilder applicationBuilder, IWebHostEnvironment env)
   {

      if (env.IsDevelopment())
         applicationBuilder.UseDeveloperExceptionPage();

      applicationBuilder.UseStaticFiles();

      applicationBuilder.UseRouting();

      applicationBuilder
         .UseEndpoints(endpoints =>
         {
            endpoints.MapControllers();
         });

      return applicationBuilder;
   }

}
