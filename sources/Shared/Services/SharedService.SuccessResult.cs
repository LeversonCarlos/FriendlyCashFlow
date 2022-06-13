namespace Lewio.CashFlow.Services;

partial class SharedService<TRequest, TResponse>
{

   protected bool SetSuccessAndReturn()
   {
      _Response.OK = true;
      return true;
   }

}
