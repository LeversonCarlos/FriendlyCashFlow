using Moq;

namespace Elesse.Identity.Tests
{
   internal class UserRepositoryMocker
   {

      readonly Mock<IUserRepository> _Mock;
      public UserRepositoryMocker() => _Mock = new Mock<IUserRepository>();
      public static UserRepositoryMocker Create() => new UserRepositoryMocker();

      public UserRepositoryMocker WithGetUserByUserName(params IUserEntity[] results)
      {
         var seq = new MockSequence();
         foreach (var result in results)
            _Mock.InSequence(seq)
               .Setup(m => m.GetUserByUserNameAsync(It.IsAny<string>()))
               .ReturnsAsync(result);
         return this;
      }

      public IUserRepository Build() => _Mock.Object;
   }
}