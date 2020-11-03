using Moq;

namespace Elesse.Identity.Tests
{
   internal class TokenRepositoryMocker
   {

      readonly Mock<ITokenRepository> _Mock;
      public TokenRepositoryMocker() => _Mock = new Mock<ITokenRepository>();
      public static TokenRepositoryMocker Create() => new TokenRepositoryMocker();

      public ITokenRepository Build() => _Mock.Object;
   }
}