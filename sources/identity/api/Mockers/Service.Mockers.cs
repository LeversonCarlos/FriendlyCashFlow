using Microsoft.AspNetCore.Mvc;
using Moq;

namespace FriendlyCashFlow.Identity.Tests
{
   internal class ServiceMocker
   {

      readonly Mock<IIdentityService> _Mock;
      public ServiceMocker() => _Mock = new Mock<IIdentityService>();
      public static ServiceMocker Create() => new ServiceMocker();

      public ServiceMocker WithRegister(RegisterVM registerVM, IActionResult actionResult)
      {
         _Mock.Setup(m => m.RegisterAsync(registerVM)).ReturnsAsync(actionResult);
         return this;
      }

      public IIdentityService Build() => _Mock.Object;
   }
}