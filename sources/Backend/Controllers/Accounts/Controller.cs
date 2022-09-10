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

   [HttpGet("{id}")]
   public Task<ActionResult<LoadResponseModel>> LoadAsync([FromQuery] string id) =>
      _ServiceProvider
         .GetService<LoadCommand>()
         .HandleAsync(LoadRequestModel.Create(id), ModelState);

   [HttpPatch("")]
   public Task<ActionResult<UpdateResponseModel>> UpdateAsync([FromBody] UpdateRequestModel request) =>
      _ServiceProvider
         .GetService<UpdateCommand>()
         .HandleAsync(request, ModelState);

}
