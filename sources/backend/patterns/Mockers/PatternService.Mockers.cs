using System;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Elesse.Patterns.Tests
{
   internal class PatternServiceMocker
   {

      readonly Mock<IPatternService> _Mock;
      public PatternServiceMocker() => _Mock = new Mock<IPatternService>();
      public static PatternServiceMocker Create() => new PatternServiceMocker();

      public PatternServiceMocker WithList(ActionResult<IPatternEntity[]> result)
      {
         _Mock.Setup(m => m.ListAsync()).ReturnsAsync(result);
         return this;
      }

      /*
      public PatternServiceMocker WithLoad(string categoryID, ActionResult<IPatternEntity> result)
      {
         _Mock.Setup(m => m.LoadAsync(categoryID)).ReturnsAsync(result);
         return this;
      }
      */

      public PatternServiceMocker WithAdd(IPatternEntity result)
      {
         _Mock.Setup(m => m.AddAsync(It.IsAny<IPatternEntity>())).ReturnsAsync(result);
         return this;
      }
      public PatternServiceMocker WithAdd(Exception ex)
      {
         _Mock.Setup(m => m.AddAsync(It.IsAny<IPatternEntity>())).ThrowsAsync(ex);
         return this;
      }

      /*
      public PatternServiceMocker WithUpdate(UpdateVM param, IActionResult result)
      {
         _Mock.Setup(m => m.UpdateAsync(param)).ReturnsAsync(result);
         return this;
      }
      */

      public PatternServiceMocker WithRemove(IPatternEntity result)
      {
         _Mock.Setup(m => m.RemoveAsync(It.IsAny<IPatternEntity>())).ReturnsAsync(result);
         return this;
      }
      public PatternServiceMocker WithRemove(Exception ex)
      {
         _Mock.Setup(m => m.RemoveAsync(It.IsAny<IPatternEntity>())).ThrowsAsync(ex);
         return this;
      }

      public IPatternService Build() => _Mock.Object;
   }
}
