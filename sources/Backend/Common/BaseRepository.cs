namespace Lewio.CashFlow.Common;

public abstract class BaseRepository
{

   public BaseRepository(IServiceProvider serviceProvider)
   {
      _ServiceProvider = serviceProvider;
      _DataContext = serviceProvider.GetRequiredService<DataContext>();
   }
   protected readonly IServiceProvider _ServiceProvider;
   protected readonly DataContext _DataContext;

}
