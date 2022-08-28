namespace Lewio.Shared;

partial class Command<TRequest, TResponse>
{

   protected virtual void SetSuccessResult()
   {
      _Response.OK = true;
      _Response.Execution.Finish = DateTimeOffset.UtcNow;
   }

}
