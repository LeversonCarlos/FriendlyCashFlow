using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace Elesse.Identity.Tests
{
   internal class ProviderMocker
   {

      readonly IServiceCollection _ServiceCollection;
      public ProviderMocker() => _ServiceCollection = new ServiceCollection();
      public static ProviderMocker Create() => new ProviderMocker();

      public ProviderMocker WithIdentityService()
      {
         var identityService = new Mock<IIdentityService>();
         _ServiceCollection.AddSingleton<IIdentityService>(identityService.Object);
         return this;
      }
      public ProviderMocker WithIdentityService(IIdentityService identityService)
      {
         _ServiceCollection.AddSingleton<IIdentityService>(identityService);
         return this;
      }

      public IServiceCollection Build() => _ServiceCollection;
   }
}