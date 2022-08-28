namespace Lewio.Shared;

partial class Command<TRequest, TResponse>
{

   protected virtual Task OnHandling() => Task.CompletedTask;

   public virtual async Task<TResponse> HandleAsync(TRequest request)
   {
      try
      {
         _Request = request;
         await OnHandling();
      }
      catch (Exception ex) { SetErrorAndReturn(ex); }
      return _Response;
   }

}
