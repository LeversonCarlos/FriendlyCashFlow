using Microsoft.Extensions.DependencyInjection;
namespace Lewio.CashFlow.Shared;

public abstract partial class SharedCommand<TRequest, TResponse> : IDisposable
   where TRequest : SharedRequestModel
   where TResponse : SharedResponseModel
{

   public SharedCommand(IServiceProvider serviceProvider)
   {
      _ServiceProvider = serviceProvider;
      _Localization = serviceProvider.GetService<ILocalization>()!;
      _Response = Activator.CreateInstance<TResponse>();
   }
   protected readonly IServiceProvider _ServiceProvider;
   protected readonly ILocalization _Localization;

   protected TResponse _Response { get; private set; }

   TRequest? __Request;
   protected TRequest _Request
   {
      get => __Request!;
      private set => __Request = value;
   }

   public virtual void Dispose()
   {
   }

}
