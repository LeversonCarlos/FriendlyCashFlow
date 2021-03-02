namespace Elesse.Transfers
{

   internal partial class TransferService : Shared.BaseService, ITransferService
   {

      public TransferService(
         ITransferRepository transferRepository,
         Balances.IBalanceService balanceService,
         Shared.IInsightsService insightsService)
         : base("transfer", insightsService)
      {
         _TransferRepository = transferRepository;
         _BalanceService = balanceService;
      }

      readonly ITransferRepository _TransferRepository;
      readonly Balances.IBalanceService _BalanceService;

   }

   public partial interface ITransferService { }

   internal partial struct WARNINGS { }

}
