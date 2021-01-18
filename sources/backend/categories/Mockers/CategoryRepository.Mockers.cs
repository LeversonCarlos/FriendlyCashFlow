using Moq;

namespace Elesse.Categories.Tests
{
   internal class CategoryRepositoryMocker
   {

      readonly Mock<ICategoryRepository> _Mock;
      public CategoryRepositoryMocker() => _Mock = new Mock<ICategoryRepository>();
      public static CategoryRepositoryMocker Create() => new CategoryRepositoryMocker();

      public CategoryRepositoryMocker WithList() =>
         WithList(new ICategoryEntity[] { });
      public CategoryRepositoryMocker WithList(params ICategoryEntity[][] results)
      {
         var seq = new MockSequence();
         foreach (var result in results)
            _Mock.InSequence(seq)
               .Setup(m => m.ListCategoriesAsync())
               .ReturnsAsync(result);
         return this;
      }

      public CategoryRepositoryMocker WithLoadCategory() =>
         WithLoadCategory(new ICategoryEntity[] { });
      public CategoryRepositoryMocker WithLoadCategory(params ICategoryEntity[] results)
      {
         var seq = new MockSequence();
         foreach (var result in results)
            _Mock.InSequence(seq)
               .Setup(m => m.LoadCategoryAsync(It.IsAny<Shared.EntityID>()))
               .ReturnsAsync(result);
         return this;
      }

      public ICategoryRepository Build() => _Mock.Object;
   }
}
