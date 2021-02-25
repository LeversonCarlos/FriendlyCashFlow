namespace Elesse.Balances
{
   partial class BalanceService
   {
      public static Tests.BalanceServiceBuilder Builder() => new Tests.BalanceServiceBuilder();
   }
}
namespace Elesse.Balances.Tests
{
   internal class BalanceServiceBuilder
   {

      IBalanceRepository _BalanceRepository = BalanceRepository.Mocker().Build();
      public BalanceServiceBuilder With(IBalanceRepository balanceRepository)
      {
         _BalanceRepository = balanceRepository;
         return this;
      }

      Shared.IInsightsService _InsightsService = Shared.Tests.InsightsServiceMocker.Create().Build();
      public BalanceServiceBuilder With(Shared.IInsightsService insightsService)
      {
         _InsightsService = insightsService;
         return this;
      }

      public BalanceService Build() =>
         new BalanceService(_BalanceRepository, _InsightsService);

   }
}
