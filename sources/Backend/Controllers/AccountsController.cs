using Lewio.CashFlow.Domain.Accounts.Services;
using Lewio.CashFlow.Services;
using Microsoft.AspNetCore.Mvc;

namespace Lewio.CashFlow.Controllers;

[Route("api/accounts")]
public class AccountsController : Controller
{

   public AccountsController(IServiceProvider serviceProvider) =>
      _ServiceProvider = serviceProvider;
   IServiceProvider _ServiceProvider;

   [HttpPost("create")]
   public Task<ActionResult<CreateResponseModel>> CreateAsync([FromServices] CreateService service, [FromBody] CreateRequestModel request) =>
      service.ExecuteAsync(request, ModelState);

}
