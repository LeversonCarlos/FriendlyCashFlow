using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Elesse.Accounts.Tests
{
   internal class AccountServiceMocker
   {

      readonly Mock<IAccountService> _Mock;
      public AccountServiceMocker() => _Mock = new Mock<IAccountService>();
      public static AccountServiceMocker Create() => new AccountServiceMocker();

      public AccountServiceMocker WithLoad(Shared.EntityID param, ActionResult<IAccountEntity> result)
      {
         _Mock.Setup(m => m.LoadAsync(param)).ReturnsAsync(result);
         return this;
      }

      public AccountServiceMocker WithSearch(string param, ActionResult<IAccountEntity[]> result)
      {
         _Mock.Setup(m => m.SearchAsync(param)).ReturnsAsync(result);
         return this;
      }

      public AccountServiceMocker WithList(ActionResult<IAccountEntity[]> result)
      {
         _Mock.Setup(m => m.ListAsync()).ReturnsAsync(result);
         return this;
      }

      public AccountServiceMocker WithInsert(InsertVM param, IActionResult result)
      {
         _Mock.Setup(m => m.InsertAsync(param)).ReturnsAsync(result);
         return this;
      }

      public AccountServiceMocker WithUpdate(UpdateVM param, IActionResult result)
      {
         _Mock.Setup(m => m.UpdateAsync(param)).ReturnsAsync(result);
         return this;
      }

      public AccountServiceMocker WithChangeState(ChangeStateVM param, IActionResult result)
      {
         _Mock.Setup(m => m.ChangeStateAsync(param)).ReturnsAsync(result);
         return this;
      }

      public AccountServiceMocker WithDelete(string param, IActionResult result)
      {
         _Mock.Setup(m => m.DeleteAsync(param)).ReturnsAsync(result);
         return this;
      }

      public IAccountService Build() => _Mock.Object;
   }
}
