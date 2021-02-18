using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Elesse.Transactions
{

   [Route("api/transactions")]
   [Authorize]
   public partial class TransactionController : Shared.BaseController
   {

      internal readonly ITransactionService _TransactionService;

      public TransactionController(ITransactionService transactionService)
      {
         _TransactionService = transactionService;
      }

   }

}
