using System;
using Moq;

namespace Elesse.Recurrences
{
   partial class RecurrenceService
   {
      public static Tests.RecurrenceServiceMocker Mocker() => new Tests.RecurrenceServiceMocker();
   }
}
namespace Elesse.Recurrences.Tests
{
   internal class RecurrenceServiceMocker
   {

      readonly Mock<IRecurrenceService> _Mock;
      public RecurrenceServiceMocker() => _Mock = new Mock<IRecurrenceService>();

      public RecurrenceServiceMocker WithGetNewRecurrence(Shared.EntityID result)
      {
         _Mock.Setup(m => m.GetNewRecurrenceAsync(It.IsAny<IRecurrenceEntityProperties>())).ReturnsAsync(result);
         return this;
      }
      public RecurrenceServiceMocker WithGetNewRecurrence(Exception ex)
      {
         _Mock.Setup(m => m.GetNewRecurrenceAsync(It.IsAny<IRecurrenceEntityProperties>())).ThrowsAsync(ex);
         return this;
      }

      /*
      public BalanceServiceMocker WithDecrease(IBalanceEntity result)
      {
         _Mock.Setup(m => m.DecreaseAsync(It.IsAny<IBalanceEntity>())).ReturnsAsync(result);
         return this;
      }
      public BalanceServiceMocker WithDecrease(Exception ex)
      {
         _Mock.Setup(m => m.DecreaseAsync(It.IsAny<IBalanceEntity>())).ThrowsAsync(ex);
         return this;
      }
      */

      public IRecurrenceService Build() => _Mock.Object;
   }
}
