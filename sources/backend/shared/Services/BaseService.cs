using Microsoft.AspNetCore.Mvc;

namespace Elesse.Shared
{
   internal abstract partial class BaseService
   {

      protected BaseService(string translationsResource, Shared.IInsightsService insightsService)
      {
         _TranslationsResource = translationsResource;
         _InsightsService = insightsService;
      }

      readonly string _TranslationsResource;
      protected readonly Shared.IInsightsService _InsightsService;

      protected OkObjectResult Ok<T>(T value) =>
         new OkObjectResult(value);

      protected OkResult Ok() =>
         new OkResult();

      protected BadRequestObjectResult Warning(params string[] messageList) =>
         Shared.Results.Warning(_TranslationsResource, messageList);

   }
}
