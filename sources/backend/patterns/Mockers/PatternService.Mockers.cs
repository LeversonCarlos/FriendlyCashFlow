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

      public PatternServiceMocker WithRetrieve(IPatternEntity result)
      {
         _Mock.Setup(m => m.RetrieveAsync(It.IsAny<IPatternEntity>())).ReturnsAsync(result);
         return this;
      }
      public PatternServiceMocker WithRetrieve(Exception ex)
      {
         _Mock.Setup(m => m.RetrieveAsync(It.IsAny<IPatternEntity>())).ThrowsAsync(ex);
         return this;
      }

      public PatternServiceMocker WithIncrease(IPatternEntity result)
      {
         _Mock.Setup(m => m.IncreaseAsync(It.IsAny<IPatternEntity>())).ReturnsAsync(result);
         return this;
      }
      public PatternServiceMocker WithIncrease(Exception ex)
      {
         _Mock.Setup(m => m.IncreaseAsync(It.IsAny<IPatternEntity>())).ThrowsAsync(ex);
         return this;
      }

      /*
      public PatternServiceMocker WithUpdate(UpdateVM param, IActionResult result)
      {
         _Mock.Setup(m => m.UpdateAsync(param)).ReturnsAsync(result);
         return this;
      }
      */

      public PatternServiceMocker WithDecrease(IPatternEntity result)
      {
         _Mock.Setup(m => m.DecreaseAsync(It.IsAny<IPatternEntity>())).ReturnsAsync(result);
         return this;
      }
      public PatternServiceMocker WithDecrease(Exception ex)
      {
         _Mock.Setup(m => m.DecreaseAsync(It.IsAny<IPatternEntity>())).ThrowsAsync(ex);
         return this;
      }

      public IPatternService Build() => _Mock.Object;
   }
}
