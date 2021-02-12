using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Elesse.Identity.Tests
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

      public ServiceMocker WithUserAuth(UserAuthVM param, ActionResult<TokenVM> result)
      {
         _Mock.Setup(m => m.UserAuthAsync(param)).ReturnsAsync(result);
         return this;
      }

      public ServiceMocker WithTokenAuth(TokenAuthVM param, ActionResult<TokenVM> result)
      {
         _Mock.Setup(m => m.TokenAuthAsync(param)).ReturnsAsync(result);
         return this;
      }

      public ServiceMocker WithChangePassword(ChangePasswordVM param, IActionResult result)
      {
         _Mock.Setup(m => m.ChangePasswordAsync(It.IsAny<System.Security.Principal.IIdentity>(), param)).ReturnsAsync(result);
         return this;
      }

      public IIdentityService Build() => _Mock.Object;
   }
}