using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Elesse.Entries.Tests
{
   internal class EntryServiceMocker
   {

      readonly Mock<IEntryService> _Mock;
      public EntryServiceMocker() => _Mock = new Mock<IEntryService>();
      public static EntryServiceMocker Create() => new EntryServiceMocker();

      public EntryServiceMocker WithList(ActionResult<IEntryEntity[]> result)
      {
         _Mock.Setup(m => m.ListAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(result);
         return this;
      }

      public EntryServiceMocker WithLoad(string entryID, ActionResult<IEntryEntity> result)
      {
         _Mock.Setup(m => m.LoadAsync(entryID)).ReturnsAsync(result);
         return this;
      }

      public EntryServiceMocker WithInsert(InsertVM param, IActionResult result)
      {
         _Mock.Setup(m => m.InsertAsync(param)).ReturnsAsync(result);
         return this;
      }

      /*
      public EntryServiceMocker WithUpdate(UpdateVM param, IActionResult result)
      {
         _Mock.Setup(m => m.UpdateAsync(param)).ReturnsAsync(result);
         return this;
      }
      */

      /*
      public EntryServiceMocker WithDelete(string categoryID, IActionResult result)
      {
         _Mock.Setup(m => m.DeleteAsync(categoryID)).ReturnsAsync(result);
         return this;
      }
      */

      public IEntryService Build() => _Mock.Object;
   }
}
