using System;
using Moq;

namespace Elesse.Balances
{
   partial class BalanceRepository
   {
      internal static Tests.BalanceRepositoryMocker Mocker() => new Tests.BalanceRepositoryMocker();
   }
}
namespace Elesse.Balances.Tests
{
   internal class BalanceRepositoryMocker
   {

      readonly Mock<IBalanceRepository> _Mock;
      public BalanceRepositoryMocker() => _Mock = new Mock<IBalanceRepository>();

      public BalanceRepositoryMocker WithList() =>
         WithList(new IBalanceEntity[] { });
      public BalanceRepositoryMocker WithList(params IBalanceEntity[][] results)
      {
         var seq = new MockSequence();
         foreach (var result in results)
            _Mock.InSequence(seq)
               .Setup(m => m.ListAsync(It.IsAny<int>(), It.IsAny<int>()))
               .ReturnsAsync(result);
         return this;
      }

      public BalanceRepositoryMocker WithLoad() =>
         WithLoad(new IBalanceEntity[] { });
      public BalanceRepositoryMocker WithLoad(params IBalanceEntity[] results)
      {
         var seq = new MockSequence();
         foreach (var result in results)
            _Mock.InSequence(seq)
               .Setup(m => m.LoadAsync(It.IsAny<Shared.EntityID>()))
               .ReturnsAsync(result);
         return this;
      }

      public BalanceRepositoryMocker WithInsert(Exception ex)
      {
         _Mock
            .Setup(m => m.InsertAsync(It.IsAny<IBalanceEntity>()))
            .ThrowsAsync(ex);
         return this;
      }

      public BalanceRepositoryMocker WithUpdate(Exception ex)
      {
         _Mock
            .Setup(m => m.UpdateAsync(It.IsAny<IBalanceEntity>()))
            .ThrowsAsync(ex);
         return this;
      }

      public BalanceRepositoryMocker WithDelete(Exception ex)
      {
         _Mock
            .Setup(m => m.DeleteAsync(It.IsAny<Shared.EntityID>()))
            .ThrowsAsync(ex);
         return this;
      }

      public IBalanceRepository Build() => _Mock.Object;
   }
}
