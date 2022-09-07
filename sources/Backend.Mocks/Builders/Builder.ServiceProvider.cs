using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
namespace Lewio.CashFlow.Mocks;

partial class Builder
{
   public static ServiceProviderBuilder ServiceProvider() => ServiceProviderBuilder.Create();
}

public class ServiceProviderBuilder
{

   internal static ServiceProviderBuilder Create() =>
      new ServiceProviderBuilder();

   ServiceCollection _ServiceCollection = new ServiceCollection();

   public ServiceProviderBuilder With(Action<ServiceCollection> services)
   {
      services(_ServiceCollection);
      return this;
   }

   public IServiceProvider Build() =>
      _ServiceCollection
         .AddDbContext<Common.DataContext>(options => options.UseInMemoryDatabase(Guid.NewGuid().ToString()))
         .AddScoped<Users.LoggedInUser>(sp => Users.LoggedInUser.Create(Guid.NewGuid().ToString()))
         .AddAccountsCommands()
         .BuildServiceProvider();

}
