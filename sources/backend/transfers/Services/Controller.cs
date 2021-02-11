using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Elesse.Transfers
{

   [Route("api/transfers")]
   [Authorize]
   public partial class TransferController : Shared.BaseController
   {

      internal readonly ITransferService _TransferService;

      public TransferController(ITransferService transferService)
      {
         _TransferService = transferService;
      }

   }

}
