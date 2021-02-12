using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Elesse.Categories
{

   public static class StartupExtentions
   {

      public static IMvcBuilder AddCategoryService(this IMvcBuilder mvcBuilder, IConfiguration configs)
      {
         var services =
            mvcBuilder.Services;
         services
            .AddScoped<ICategoryRepository, CategoryRepository>()
            .AddScoped<ICategoryService, CategoryService>();
         mvcBuilder
            .AddApplicationPart(Assembly.GetAssembly(typeof(CategoryController)));
         return mvcBuilder;
      }

   }

}
