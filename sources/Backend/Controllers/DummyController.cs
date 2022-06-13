using Lewio.CashFlow.Services;
using Microsoft.AspNetCore.Mvc;

namespace Lewio.CashFlow.Controllers;

[Route("api/dummy")]
public class DummyController : Controller
{

   public DummyController(IServiceProvider serviceProvider) =>
      _ServiceProvider = serviceProvider;
   IServiceProvider _ServiceProvider;

   [HttpPost("")]
   public Task<ActionResult<DummyResponseModel>> DummyAsync([FromServices] DummyService service, [FromBody] DummyRequestModel request) =>
      service.ExecuteAsync(request, ModelState);

}
