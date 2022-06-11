namespace Lewio.CashFlow.Services;

partial class SharedService<TRequest, TResponse>
{

   protected virtual Task OnExecuting() => Task.CompletedTask;

   public async Task<TResponse> ExecuteAsync(TRequest request)
   {
      try
      {
         await OnExecuting();
      }
      catch (Exception ex) { SetErrorAndReturn(ex); }
      return _Response;
   }

}
