using Moq;

namespace Elesse.Identity.Tests
{
   internal class TokenRepositoryMocker
   {

      readonly Mock<ITokenRepository> _Mock;
      public TokenRepositoryMocker() => _Mock = new Mock<ITokenRepository>();
      public static TokenRepositoryMocker Create() => new TokenRepositoryMocker();

      public TokenRepositoryMocker WithRetrieveRefreshToken(params IRefreshToken[] results)
      {
         var seq = new MockSequence();
         foreach (var result in results)
            _Mock.InSequence(seq)
               .Setup(m => m.RetrieveRefreshTokenAsync(It.IsAny<string>()))
               .ReturnsAsync(result);
         return this;
      }

      public ITokenRepository Build() => _Mock.Object;
   }
}