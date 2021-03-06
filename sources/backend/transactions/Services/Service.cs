namespace Elesse.Transactions
{
   internal partial class TransactionService : Shared.BaseService, ITransactionService
   {

      public TransactionService(Shared.IInsightsService insightsService)
         : base("transaction", insightsService)
      { }

   }
}
