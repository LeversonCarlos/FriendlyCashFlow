namespace Lewio.CashFlow.Common;

public partial interface IMainRepository { }

public partial class MainRepository : BaseRepository, IMainRepository
{
   public MainRepository(IServiceProvider serviceProvider) : base(serviceProvider) { }
}
