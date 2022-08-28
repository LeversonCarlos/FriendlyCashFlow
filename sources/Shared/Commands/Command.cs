namespace Lewio.Shared;

public abstract partial class CommandLite<TRequest, TResponse> : IDisposable
   where TRequest : RequestModel
   where TResponse : ResponseModel
{

   public CommandLite(IServiceProvider serviceProvider)
   {
      _ServiceProvider = serviceProvider;
      _Response = Activator.CreateInstance<TResponse>();
   }
   protected readonly IServiceProvider _ServiceProvider;
   protected TRequest _Request { get; private set; }
   protected TResponse _Response { get; private set; }

   public virtual void Dispose()
   {
      _Request = null;
      _Response = null;
   }

}
