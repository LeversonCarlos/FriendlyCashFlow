namespace Lewio.CashFlow.Repository;

public partial class MainRepository : BaseRepository, IMainRepository
{
   public MainRepository(IServiceProvider serviceProvider) : base(serviceProvider) { }
}
