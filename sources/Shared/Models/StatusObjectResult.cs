using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
namespace Lewio.Shared;

public class OkResult : StatusObjectResult
{
   public OkResult(object value) : base(value, StatusCodes.Status200OK) { }
}

public class BadRequestResult : StatusObjectResult
{
   public BadRequestResult(object value) : base(value, StatusCodes.Status400BadRequest) { }
}

public class ErrorObjectResult : StatusObjectResult
{
   public ErrorObjectResult(object value) : base(value, StatusCodes.Status500InternalServerError) { }
}

public abstract partial class StatusObjectResult : ObjectResult
{
   protected StatusObjectResult(object value, int? statusCode) : base(value)
   {
      StatusCode = statusCode;
   }
}
