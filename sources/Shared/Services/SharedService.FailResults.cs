namespace Lewio.CashFlow.Services;

partial class SharedService<TRequest, TResponse>
{

   protected bool SetMessageAndReturn(params string[] messages)
   {
      _Response.Messages = messages
         .Select(x => SharedResponseModel.Message.CreateMessage(x))
         .ToArray();
      return false;
   }

   protected bool SetWarningAndReturn(params string[] warnings)
   {
      _Response.Messages = warnings
         .Select(x => SharedResponseModel.Message.CreateWarning(x))
         .ToArray();
      return false;
   }

   protected bool SetErrorAndReturn(Exception value)
   {
      _Response.Messages = SetErrorAndReturn_GetParsedExceptions(value).ToArray();
      return false;
   }

   private SharedResponseModel.Message[] SetErrorAndReturn_GetParsedExceptions(Exception value)
   {
      var result = new List<SharedResponseModel.Message>();
      result.Add(SharedResponseModel.Message.CreateError(value.Message));
      if (value.InnerException != null)
         result.AddRange(SetErrorAndReturn_GetParsedExceptions(value.InnerException));
      return result.ToArray();
   }

}
