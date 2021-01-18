using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Elesse.Categories.Tests
{
   internal class CategoryServiceMocker
   {

      readonly Mock<ICategoryService> _Mock;
      public CategoryServiceMocker() => _Mock = new Mock<ICategoryService>();
      public static CategoryServiceMocker Create() => new CategoryServiceMocker();

      public CategoryServiceMocker WithList(ActionResult<ICategoryEntity[]> result)
      {
         _Mock.Setup(m => m.ListAsync()).ReturnsAsync(result);
         return this;
      }

      public CategoryServiceMocker WithLoad(string categoryID, ActionResult<ICategoryEntity> result)
      {
         _Mock.Setup(m => m.LoadAsync(categoryID)).ReturnsAsync(result);
         return this;
      }

      public CategoryServiceMocker WithInsert(InsertVM param, IActionResult result)
      {
         _Mock.Setup(m => m.InsertAsync(param)).ReturnsAsync(result);
         return this;
      }

      /*
      public CategoryServiceMocker WithUpdate(UpdateVM param, IActionResult result)
      {
         _Mock.Setup(m => m.UpdateAsync(param)).ReturnsAsync(result);
         return this;
      }
      */

      /*
      public CategoryServiceMocker WithDelete(string categoryID, IActionResult result)
      {
         _Mock.Setup(m => m.DeleteAsync(categoryID)).ReturnsAsync(result);
         return this;
      }
      */

      public ICategoryService Build() => _Mock.Object;
   }
}
