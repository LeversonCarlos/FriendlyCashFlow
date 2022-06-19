using Lewio.CashFlow.Shared;
using Microsoft.AspNetCore.Mvc;
namespace Lewio.CashFlow;

internal partial class ActionResults
{

   public static ActionResult<T> Ok<T>(T value) =>
      new OkObjectResult(value);

   public static BadRequestObjectResult Warning<T>(T value) =>
      new BadRequestObjectResult(value);

   public static ErrorObjectResult Error<T>(T value) =>
      new ErrorObjectResult(value!);

}

partial class ActionResults
{

   public static BadRequestObjectResult Warning(Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary modelState)
   {
      var response = new SharedResponseModel
      {
         OK = false,
         Messages = modelState.Values
            .SelectMany(x => x.Errors)
            .Select(x => MessageModel.CreateWarning(x.ErrorMessage))
            .ToArray()
      };
      return Warning(response);
   }

   public static ErrorObjectResult Error(Exception ex)
   {
      var response = new SharedResponseModel
      {
         OK = false,
         Messages = ex.ToMessageModel()
      };
      return Error(response);
   }

}

public class ErrorObjectResult : StatusCodeObjectResult
{
   public ErrorObjectResult(object value)
   : base(StatusCodes.Status500InternalServerError, value) { }
}

public abstract class StatusCodeObjectResult : ObjectResult
{
   public StatusCodeObjectResult(int statusCode, object value) : base(value)
   {
      StatusCode = statusCode;
   }
}
