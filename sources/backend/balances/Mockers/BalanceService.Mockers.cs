using System;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Elesse.Balances
{
   partial class BalanceService
   {
      public static Tests.BalanceServiceMocker Mocker() => new Tests.BalanceServiceMocker();
   }
}
namespace Elesse.Balances.Tests
{
   internal class BalanceServiceMocker
   {

      readonly Mock<IBalanceService> _Mock;
      public BalanceServiceMocker() => _Mock = new Mock<IBalanceService>();

      public BalanceServiceMocker WithList(ActionResult<IBalanceEntity[]> result)
      {
         _Mock.Setup(m => m.ListAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(result);
         return this;
      }

      /*
      public BalanceServiceMocker WithIncrease(IBalanceEntity result)
      {
         _Mock.Setup(m => m.IncreaseAsync(It.IsAny<IBalanceEntity>())).ReturnsAsync(result);
         return this;
      }
      public BalanceServiceMocker WithIncrease(Exception ex)
      {
         _Mock.Setup(m => m.IncreaseAsync(It.IsAny<IBalanceEntity>())).ThrowsAsync(ex);
         return this;
      }
      */

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

      public IBalanceService Build() => _Mock.Object;
   }
}
