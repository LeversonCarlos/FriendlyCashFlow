namespace Elesse.Categories
{

   internal partial class CategoryService : Shared.BaseService, ICategoryService
   {

      public CategoryService(ICategoryRepository categoryRepository, Shared.IInsightsService insightsService)
         : base("categories", insightsService)
      {
         _CategoryRepository = categoryRepository;
      }

      readonly ICategoryRepository _CategoryRepository;

   }

   internal partial struct WARNINGS { }

}
