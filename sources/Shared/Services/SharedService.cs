using Lewio.CashFlow.Repository;
using Lewio.CashFlow.Shared;
using Microsoft.Extensions.DependencyInjection;

namespace Lewio.CashFlow.Services;

public abstract partial class SharedService<TRequest, TResponse> : IDisposable
   where TRequest : SharedRequestModel
   where TResponse : SharedResponseModel
{

   public SharedService(IServiceProvider serviceProvider)
   {
      _ServiceProvider = serviceProvider;
      _MainRepository = serviceProvider.GetService<IMainRepository>()!;
      _Localization = serviceProvider.GetService<ILocalization>()!;
      _Response = Activator.CreateInstance<TResponse>();
      _Request = Activator.CreateInstance<TRequest>(); // the proper value will be defined on the hanldle function
   }
   protected readonly IServiceProvider _ServiceProvider;
   protected readonly IMainRepository _MainRepository;
   protected readonly ILocalization _Localization;

   protected TRequest _Request { get; private set; }
   protected TResponse _Response { get; private set; }

   public virtual void Dispose()
   {
   }

}
