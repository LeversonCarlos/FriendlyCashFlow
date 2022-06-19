namespace Lewio.CashFlow.Shared;

partial class SharedCommand<TRequest, TResponse>
{

   protected bool SetMessageAndReturn(params string[] messages)
   {
      _Response.OK = false;
      _Response.Messages = messages
         .Select(x => MessageModel.CreateMessage(x))
         .ToArray();
      return false;
   }

   protected bool SetWarningAndReturn(params string[] warnings)
   {
      _Response.OK = false;
      _Response.Messages = warnings
         .Select(x => MessageModel.CreateWarning(x))
         .ToArray();
      return false;
   }

   protected bool SetErrorAndReturn(Exception value)
   {
      _Response.OK = false;
      _Response.Messages = value.ToMessageModel();
      return false;
   }

}
