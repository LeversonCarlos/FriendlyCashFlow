using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics;

namespace FriendlyCashFlow.API.Base
{
   partial class BaseService
   {

      /* https://developer.mozilla.org/pt-BR/docs/Web/HTTP/Status */
      public enum enStatusCode : int { Info = 100, Success = 200, Warning = 400, Unauthorized = 401, Forbidden = 403, NotFound = 404, ServerError = 500 }


      [DebuggerStepThrough]
      protected ActionResult<T> OkResponse<T>(T value)
      { return new OkObjectResult(value); }

      [DebuggerStepThrough]
      protected ActionResult<T> CreatedResponse<T>(string controller, long id, T value)
      { return new CreatedResult(new Uri($"/api/{controller}/{id}", UriKind.Relative), value);}

      [DebuggerStepThrough]
      protected ActionResult<T> CreatedResponse<T>(string controller, string id, T value)
      { return new CreatedResult(new Uri($"/api/{controller}/{id}", UriKind.Relative), value);}

      [DebuggerStepThrough]
      protected ActionResult InformationResponse(params string[] messages)
      { return this.MessagesResponse(messages, enStatusCode.Info); }

      [DebuggerStepThrough]
      protected ActionResult WarningResponse(params string[] messages)
      { return this.MessagesResponse(messages, enStatusCode.Warning); }

      [DebuggerStepThrough]
      protected ActionResult NotFoundResponse()
      { return new NotFoundResult(); }

      [DebuggerStepThrough]
      protected ActionResult ForbiddenResponse()
      { return new ForbidResult(); }

      [DebuggerStepThrough]
      private ActionResult MessagesResponse(string[] messages, enStatusCode statusCode)
      {
         var modelState = new Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary();
         var prefix = (statusCode == enStatusCode.Info ? "INFO" : "WARNING");
         foreach (var messageKey in messages)
         {
            var messageValue = this.GetTranslation(messageKey);
            modelState.AddModelError($"{prefix}_{messageKey}", messageValue);
         }
         return new BadRequestObjectResult(modelState);
      }

      [DebuggerStepThrough]
      protected ActionResult ExceptionResponse(Exception ex)
      {
         var modelState = new Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary();
         modelState.AddModelError("EXCEPTION_", ex.Message);
         if (ex.InnerException != null)
         { modelState.AddModelError("INNER_EXCEPTION_", ex.InnerException.Message); }
         return new BadRequestObjectResult(modelState);
      }

      [DebuggerStepThrough]
      public T GetValue<T>(ActionResult<T> actionResult)
      {
         try
         {
            if (actionResult.Result is OkObjectResult)
            {
               var objectResult = (actionResult.Result as OkObjectResult);
               if (objectResult.Value != null) { return (T)objectResult.Value; }
            }
            if (actionResult.Value != null) { return actionResult.Value; }
            return default(T);
         }
         catch (Exception) { return default(T); }

      }

   }

}
