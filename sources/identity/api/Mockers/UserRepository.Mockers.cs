using Moq;

namespace Elesse.Identity.Tests
{
   internal class UserRepositoryMocker
   {

      readonly Mock<IUserRepository> _Mock;
      public UserRepositoryMocker() => _Mock = new Mock<IUserRepository>();
      public static UserRepositoryMocker Create() => new UserRepositoryMocker();

      public UserRepositoryMocker WithGetUserByUserName(string userName, IUserEntity result)
      {
         _Mock.Setup(m => m.GetUserByUserNameAsync(userName)).ReturnsAsync(result);
         return this;
      }

      public IUserRepository Build() => _Mock.Object;
   }
}