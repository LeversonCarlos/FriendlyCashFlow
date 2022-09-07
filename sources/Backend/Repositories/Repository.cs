namespace Lewio.CashFlow.Common;

internal partial interface IMainRepository { }

internal partial class MainRepository : BaseRepository, IMainRepository
{
   public MainRepository(IServiceProvider serviceProvider) : base(serviceProvider) { }
}
