using Microsoft.AspNetCore.Mvc;
using Moq;

namespace FriendlyCashFlow.Identity.Tests
{
   internal class ServiceMocker
   {

      readonly Mock<IIdentityService> _Mock;
      public ServiceMocker() => _Mock = new Mock<IIdentityService>();
      public static ServiceMocker Create() => new ServiceMocker();

      public ServiceMocker WithRegister(RegisterVM param, IActionResult result)
      {
         _Mock.Setup(m => m.RegisterAsync(param)).ReturnsAsync(result);
         return this;
      }

      public ServiceMocker WithAuthUser(AuthUserVM param, IActionResult result)
      {
         _Mock.Setup(m => m.AuthUserAsync(param)).ReturnsAsync(result);
         return this;
      }

      public IIdentityService Build() => _Mock.Object;
   }
}