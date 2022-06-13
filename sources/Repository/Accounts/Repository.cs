using Microsoft.Extensions.DependencyInjection;

namespace Lewio.CashFlow.Repository;

public partial class AccountRepository : BaseRepository, IAccountRepository
{
   public AccountRepository(IServiceProvider serviceProvider) : base(serviceProvider) { }
}

partial class MainRepository
{

   public IAccountRepository Accounts { get => _ServiceProvider.GetService<IAccountRepository>()!; }

}

