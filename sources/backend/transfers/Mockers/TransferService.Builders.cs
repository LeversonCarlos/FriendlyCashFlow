namespace Elesse.Transfers
{
   partial class TransferService
   {
      public static Tests.TransferServiceBuilder Builder() => new Tests.TransferServiceBuilder();
   }
}
namespace Elesse.Transfers.Tests
{
   internal class TransferServiceBuilder
   {

      ITransferRepository _TransferRepository = Tests.TransferRepositoryMocker.Create().Build();
      public TransferServiceBuilder With(ITransferRepository transferRepository)
      {
         _TransferRepository = transferRepository;
         return this;
      }

      Balances.IBalanceService _BalanceService = Balances.BalanceService.Mocker().Build();
      public TransferServiceBuilder With(Balances.IBalanceService balanceService)
      {
         _BalanceService = balanceService;
         return this;
      }

      Shared.IInsightsService _InsightsService = Shared.Tests.InsightsServiceMocker.Create().Build();
      public TransferServiceBuilder With(Shared.IInsightsService insightsService)
      {
         _InsightsService = insightsService;
         return this;
      }

      public TransferService Build() =>
         new TransferService(_TransferRepository, _BalanceService, _InsightsService);

   }
}
