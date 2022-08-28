using Microsoft.AspNetCore.Mvc;
namespace Lewio.CashFlow.Accounts;

// [Authorize]
[Route("api/accounts")]
public partial class AccountsController : Controller
{

   public AccountsController(IServiceProvider serviceProvider) =>
      _ServiceProvider = serviceProvider;
   readonly IServiceProvider _ServiceProvider;


   [HttpPost("search")]
   public Task<ActionResult<SearchResponseModel>> SearchAsync([FromBody] SearchRequestModel request) =>
      _ServiceProvider
         .GetService<SearchCommand>()
         .HandleAsync(request, ModelState);

}
