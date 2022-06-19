namespace Lewio.CashFlow.Shared;

partial class SharedCommand<TRequest, TResponse>
{

   protected virtual Task OnExecuting() => Task.CompletedTask;

   public async Task<TResponse> ExecuteAsync(TRequest request)
   {
      try
      {
         _Request = request;
         await OnExecuting();
      }
      catch (Exception ex) { SetErrorAndReturn(ex); }
      return _Response;
   }

}
