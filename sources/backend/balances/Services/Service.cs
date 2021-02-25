namespace Elesse.Balances
{

   internal partial class BalanceService : Shared.BaseService, IBalanceService
   {

      public BalanceService(IBalanceRepository balanceRepository, Shared.IInsightsService insightsService)
         : base("balance", insightsService)
      {
         _BalanceRepository = balanceRepository;
      }

      readonly IBalanceRepository _BalanceRepository;

   }

   public partial interface IBalanceService { }

   internal partial struct WARNINGS { }

}