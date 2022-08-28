namespace Lewio.Shared;

public abstract partial class Command<TRequest, TResponse> : IDisposable
   where TRequest : RequestModel
   where TResponse : ResponseModel
{

   public Command(IServiceProvider serviceProvider)
   {
      _ServiceProvider = serviceProvider;
      // _Request = default!;
      _Response = Activator.CreateInstance<TResponse>();
   }
   protected readonly IServiceProvider _ServiceProvider;
   protected TRequest _Request { get; private set; }
   protected TResponse _Response { get; private set; }

   public virtual void Dispose()
   {
   }

}
