namespace Elesse.Categories
{

   internal partial class CategoryService : ICategoryService
   {

      public CategoryService(ICategoryRepository categoryRepository, Shared.IInsightsService insightsService)
      {
         _CategoryRepository = categoryRepository;
         _InsightsService = insightsService;
      }

      readonly ICategoryRepository _CategoryRepository;
      readonly Shared.IInsightsService _InsightsService;

      Microsoft.AspNetCore.Mvc.BadRequestObjectResult Warning(params string[] messageList) =>
         Shared.Results.Warning("categories", messageList);

   }

   public partial interface ICategoryService { }

   internal partial struct WARNINGS { }

}
