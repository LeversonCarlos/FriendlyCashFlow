using Microsoft.Extensions.DependencyInjection;

namespace Lewio.CashFlow.Repository;

public abstract class BaseRepository : IRepository
{

   public BaseRepository(IServiceProvider serviceProvider) =>
      _ServiceProvider = serviceProvider;
   protected readonly IServiceProvider _ServiceProvider;

   public IMainRepository Main { get => _ServiceProvider.GetService<IMainRepository>()!; }

   public virtual void Dispose()
   {
   }

}
