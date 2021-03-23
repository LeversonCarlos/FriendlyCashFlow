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

      public RecurrenceServiceMocker WithInsert(Shared.EntityID result)
      {
         _Mock.Setup(m => m.InsertAsync(It.IsAny<IRecurrenceProperties>())).ReturnsAsync(result);
         return this;
      }
      public RecurrenceServiceMocker WithInsert(Exception ex)
      {
         _Mock.Setup(m => m.InsertAsync(It.IsAny<IRecurrenceProperties>())).ThrowsAsync(ex);
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
