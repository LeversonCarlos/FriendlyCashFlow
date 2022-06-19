namespace Lewio.CashFlow.Shared;

partial class SharedCommand<TRequest, TResponse>
{

   protected bool SetSuccessAndReturn()
   {
      _Response.OK = true;
      return true;
   }

}
