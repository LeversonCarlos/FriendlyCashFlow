namespace Elesse.Transfers
{

   internal partial class TransferService : Shared.BaseService, ITransferService
   {

      public TransferService(ITransferRepository transferRepository, Shared.IInsightsService insightsService)
         : base("transfer", insightsService)
      {
         _TransferRepository = transferRepository;
      }

      readonly ITransferRepository _TransferRepository;

   }

   public partial interface ITransferService { }

   internal partial struct WARNINGS { }

}
