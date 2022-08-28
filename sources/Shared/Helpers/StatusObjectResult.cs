using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
namespace Lewio.Shared;

partial class StatusObjectResult
{

   public static StatusObjectResult Ok<T>(T value) =>
      new OkResult(value);

   public static StatusObjectResult BadRequest<T>(T value) =>
      new BadRequestResult(value);

   public static StatusObjectResult InternalError<T>(T value) =>
      new ErrorObjectResult(value);

   public static StatusObjectResult InternalError(Exception value)
   {
      var response = new BaseResponseModel
      {
         OK = false,
         Messages = value.ToMessages()
      };
      if (response.Messages != null && response.Messages.Any(msg => msg.Type == Message.TypeEnum.Error))
         return StatusObjectResult.InternalError(response);
      else
         return StatusObjectResult.BadRequest(response);
   }

}
