namespace Lewio.Shared;

partial class Command<TRequest, TResponse>
{

   protected bool SetWarningAndReturn(params string[] warnings)
   {
      var messages = warnings
         .Select(x => Message.CreateWarning(x))
         .ToArray();
      return SetFailResultAndReturn(messages);
   }

   protected bool SetErrorAndReturn(Exception value) =>
      SetFailResultAndReturn(value.ToMessages());


   private bool SetFailResultAndReturn(params Message[] typedMessages)
   {
      _Response.OK = false;
      _Response.Messages = typedMessages;
      _Response.Execution.Finish = DateTimeOffset.UtcNow;
      return false;
   }

}
