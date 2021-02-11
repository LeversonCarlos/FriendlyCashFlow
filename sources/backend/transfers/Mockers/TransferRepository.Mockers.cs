using System;
using Moq;

namespace Elesse.Transfers.Tests
{
   internal class TransferRepositoryMocker
   {

      readonly Mock<ITransferRepository> _Mock;
      public TransferRepositoryMocker() => _Mock = new Mock<ITransferRepository>();
      public static TransferRepositoryMocker Create() => new TransferRepositoryMocker();

      public TransferRepositoryMocker WithList() =>
         WithList(new ITransferEntity[] { });
      public TransferRepositoryMocker WithList(params ITransferEntity[][] results)
      {
         var seq = new MockSequence();
         foreach (var result in results)
            _Mock.InSequence(seq)
               .Setup(m => m.ListAsync(It.IsAny<int>(), It.IsAny<int>()))
               .ReturnsAsync(result);
         return this;
      }

      public TransferRepositoryMocker WithLoad() =>
         WithLoad(new ITransferEntity[] { });
      public TransferRepositoryMocker WithLoad(params ITransferEntity[] results)
      {
         var seq = new MockSequence();
         foreach (var result in results)
            _Mock.InSequence(seq)
               .Setup(m => m.LoadAsync(It.IsAny<Shared.EntityID>()))
               .ReturnsAsync(result);
         return this;
      }

      public TransferRepositoryMocker WithInsert(Exception ex)
      {
         _Mock
            .Setup(m => m.InsertAsync(It.IsAny<ITransferEntity>()))
            .ThrowsAsync(ex);
         return this;
      }

      public TransferRepositoryMocker WithUpdate(Exception ex)
      {
         _Mock
            .Setup(m => m.UpdateAsync(It.IsAny<ITransferEntity>()))
            .ThrowsAsync(ex);
         return this;
      }

      public TransferRepositoryMocker WithDelete(Exception ex)
      {
         _Mock
            .Setup(m => m.DeleteAsync(It.IsAny<Shared.EntityID>()))
            .ThrowsAsync(ex);
         return this;
      }

      public ITransferRepository Build() => _Mock.Object;
   }
}
