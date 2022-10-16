using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
namespace Lewio.CashFlow.Mocks;

partial class Builder
{
   public static ServiceProviderBuilder ServiceProvider(bool withRepositories = true) => ServiceProviderBuilder.Create(withRepositories);
}

public class ServiceProviderBuilder
{
   ServiceCollection _ServiceCollection = new ServiceCollection();

   internal static ServiceProviderBuilder Create(bool withRepositories = true)
   {
      var builder = new ServiceProviderBuilder();
      builder
         .WithDefaults()
         .WithCommands();
      if (withRepositories)
         builder.WithRepositories();
      return builder;
   }

   private ServiceProviderBuilder WithDefaults()
   {
      _ServiceCollection
         .AddInMemoryContext<Common.DataContext>()
         .AddScoped<Users.LoggedInUser>(sp => Users.LoggedInUser.Create(Guid.NewGuid().ToString()));
      return this;
   }

   private ServiceProviderBuilder WithCommands()
   {
      _ServiceCollection
         .AddAccountsCommands();
      return this;
   }

   private ServiceProviderBuilder WithRepositories()
   {
      _ServiceCollection
         .AddAccountsRepository();
      return this;
   }

   public ServiceProviderBuilder With(Action<ServiceCollection> services)
   {
      services(_ServiceCollection);
      return this;
   }

   public IServiceProvider Build() => _ServiceCollection.BuildServiceProvider();
}
