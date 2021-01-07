using Moq;

namespace Elesse.Accounts.Tests
{
   internal class AccountRepositoryMocker
   {

      readonly Mock<IAccountRepository> _Mock;
      public AccountRepositoryMocker() => _Mock = new Mock<IAccountRepository>();
      public static AccountRepositoryMocker Create() => new AccountRepositoryMocker();

      public AccountRepositoryMocker WithSearchAccounts() =>
         WithSearchAccounts(new IAccountEntity[][] { });
      public AccountRepositoryMocker WithSearchAccounts(params IAccountEntity[][] results)
      {
         var seq = new MockSequence();
         foreach (var result in results)
            _Mock.InSequence(seq)
               .Setup(m => m.SearchAccountsAsync(It.IsAny<string>()))
               .ReturnsAsync(result);
         return this;
      }

      public AccountRepositoryMocker WithGetAccountByID() =>
         WithGetAccountByID(new IAccountEntity[] { });
      public AccountRepositoryMocker WithGetAccountByID(params IAccountEntity[] results)
      {
         var seq = new MockSequence();
         foreach (var result in results)
            _Mock.InSequence(seq)
               .Setup(m => m.GetAccountByIDAsync(It.IsAny<Shared.EntityID>()))
               .ReturnsAsync(result);
         return this;
      }

      public IAccountRepository Build() => _Mock.Object;
   }
}
