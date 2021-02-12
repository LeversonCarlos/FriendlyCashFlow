using Moq;

namespace Elesse.Accounts.Tests
{
   internal class AccountRepositoryMocker
   {

      readonly Mock<IAccountRepository> _Mock;
      public AccountRepositoryMocker() => _Mock = new Mock<IAccountRepository>();
      public static AccountRepositoryMocker Create() => new AccountRepositoryMocker();

      public AccountRepositoryMocker WithList() =>
         WithList(new IAccountEntity[] { });
      public AccountRepositoryMocker WithList(params IAccountEntity[][] results)
      {
         var seq = new MockSequence();
         foreach (var result in results)
            _Mock.InSequence(seq)
               .Setup(m => m.ListAccountsAsync())
               .ReturnsAsync(result);
         return this;
      }

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

      public AccountRepositoryMocker WithLoadAccount() =>
         WithLoadAccount(new IAccountEntity[] { });
      public AccountRepositoryMocker WithLoadAccount(params IAccountEntity[] results)
      {
         var seq = new MockSequence();
         foreach (var result in results)
            _Mock.InSequence(seq)
               .Setup(m => m.LoadAccountAsync(It.IsAny<Shared.EntityID>()))
               .ReturnsAsync(result);
         return this;
      }

      public IAccountRepository Build() => _Mock.Object;
   }
}
