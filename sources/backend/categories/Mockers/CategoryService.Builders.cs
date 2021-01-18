namespace Elesse.Categories
{
   partial class CategoryService
   {

      internal static CategoryService Create() =>
         new CategoryService(
            Tests.CategoryRepositoryMocker.Create().Build(),
            Shared.Tests.InsightsServiceMocker.Create().Build()
            );

      internal static CategoryService Create(ICategoryRepository categoryRepository) =>
         new CategoryService(
            categoryRepository,
            Shared.Tests.InsightsServiceMocker.Create().Build()
         );

   }
}
