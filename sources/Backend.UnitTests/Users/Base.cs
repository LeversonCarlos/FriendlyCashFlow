using Microsoft.Extensions.DependencyInjection;

namespace Lewio.CashFlow.UnitTests;

public partial class UsersTests : BaseTests
{

   [Fact]
   public void InjectedLoggedInUser_AreScoped_OnTheRequest()
   {

      var userID = _Faker.Database.Random.Guid().ToString();
      var user = Users.LoggedInUser.Create(userID);

      var serviceProvider = Mocks.Builder
         .ServiceProvider()
         .With(services => services.AddScoped<Users.LoggedInUser>(sp => user))
         .Build();

      var loggedInUser = serviceProvider
         .GetService<Users.LoggedInUser>();

      Assert.NotNull(loggedInUser);
      Assert.Equal(userID, loggedInUser);
   }

}
