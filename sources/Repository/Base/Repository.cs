using Lewio.CashFlow.Shared;
namespace Lewio.CashFlow.Repository;

public abstract class BaseRepository : IRepository
{

   public BaseRepository(IServiceProvider serviceProvider) =>
      _ServiceProvider = serviceProvider;
   protected readonly IServiceProvider _ServiceProvider;

   public virtual void Dispose()
   {
   }

}
