using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Elesse.Transfers.Tests
{
   internal class TransferServiceMocker
   {

      readonly Mock<ITransferService> _Mock;
      public TransferServiceMocker() => _Mock = new Mock<ITransferService>();
      public static TransferServiceMocker Create() => new TransferServiceMocker();

      public TransferServiceMocker WithList(ActionResult<ITransferEntity[]> result)
      {
         _Mock.Setup(m => m.ListAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(result);
         return this;
      }

      public TransferServiceMocker WithLoad(string id, ActionResult<ITransferEntity> result)
      {
         _Mock.Setup(m => m.LoadAsync(id)).ReturnsAsync(result);
         return this;
      }

      /*
      public TransferServiceMocker WithInsert(InsertVM param, IActionResult result)
      {
         _Mock.Setup(m => m.InsertAsync(param)).ReturnsAsync(result);
         return this;
      }
      */

      /*
      public TransferServiceMocker WithUpdate(UpdateVM param, IActionResult result)
      {
         _Mock.Setup(m => m.UpdateAsync(param)).ReturnsAsync(result);
         return this;
      }
      */

      /*
      public TransferServiceMocker WithDelete(string id, IActionResult result)
      {
         _Mock.Setup(m => m.DeleteAsync(id)).ReturnsAsync(result);
         return this;
      }
      */

      public ITransferService Build() => _Mock.Object;
   }
}
