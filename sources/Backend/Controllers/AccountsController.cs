using Lewio.CashFlow.Accounts;
using Lewio.CashFlow.Services;
using Microsoft.AspNetCore.Mvc;

namespace Lewio.CashFlow.Controllers;

[Route("api/accounts")]
public class AccountsController : Controller
{

   public AccountsController(IServiceProvider serviceProvider) =>
      _ServiceProvider = serviceProvider;
   IServiceProvider _ServiceProvider;

   [HttpPost("load")]
   public Task<ActionResult<LoadResponseModel>> LoadAsync([FromServices] LoadCommand service, [FromBody] LoadRequestModel request) =>
      service.ExecuteAsync(request, ModelState);

   [HttpPost("save")]
   public Task<ActionResult<SaveResponseModel>> SaveAsync([FromServices] SaveCommand service, [FromBody] SaveRequestModel request) =>
      service.ExecuteAsync(request, ModelState);

}
