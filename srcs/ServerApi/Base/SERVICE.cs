
using System;
using System.Diagnostics;

namespace FriendlyCashFlow.API.Base
{
   internal partial class BaseService : IDisposable
   {

      protected readonly Base.dbContext dbContext;
      public IServiceProvider serviceProvider { get; set; }
      protected BaseService(IServiceProvider _serviceProvider)
      {
         this.serviceProvider = _serviceProvider;
         this.dbContext = this.GetService<Base.dbContext>();
      }

      [DebuggerStepThrough]
      protected T GetService<T>() where T : class
      { return (T)this.serviceProvider.GetService(typeof(T)); }

      public void Dispose()
      { /* throw new NotImplementedException(); */ }

   }
}
