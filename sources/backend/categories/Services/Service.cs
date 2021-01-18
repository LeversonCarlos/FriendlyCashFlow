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

   }

   public partial interface ICategoryService { }

   internal partial struct WARNINGS { }

}
