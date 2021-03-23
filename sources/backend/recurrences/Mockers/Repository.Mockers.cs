using System;
using System.Threading.Tasks;
using Moq;

namespace Elesse.Recurrences
{
   partial class RecurrenceRepository
   {
      internal static Tests.RecurrenceRepositoryMocker Mocker() => new Tests.RecurrenceRepositoryMocker();
   }
}
namespace Elesse.Recurrences.Tests
{
   internal class RecurrenceRepositoryMocker
   {

      readonly Mock<IRecurrenceRepository> _Mock;
      public RecurrenceRepositoryMocker() => _Mock = new Mock<IRecurrenceRepository>();

      public RecurrenceRepositoryMocker With(Action<Mock<IRecurrenceRepository>> callback)
      {
         callback?.Invoke(_Mock);
         return this;
      }

      public RecurrenceRepositoryMocker WithLoad() =>
         WithLoad(new IRecurrenceEntity[] { });
      public RecurrenceRepositoryMocker WithLoad(params IRecurrenceEntity[] results)
      {
         var seq = new MockSequence();
         foreach (var result in results)
            _Mock.InSequence(seq)
               .Setup(m => m.LoadAsync(It.IsAny<Shared.EntityID>()))
               .ReturnsAsync(result);
         return this;
      }
      public RecurrenceRepositoryMocker WithLoad(Exception ex)
      {
         _Mock
            .Setup(m => m.LoadAsync(It.IsAny<Shared.EntityID>()))
            .ThrowsAsync(ex);
         return this;
      }

      public RecurrenceRepositoryMocker WithInsert(Exception ex)
      {
         _Mock
            .Setup(m => m.InsertAsync(It.IsAny<IRecurrenceEntity>()))
            .ThrowsAsync(ex);
         return this;
      }
      public RecurrenceRepositoryMocker WithInsert(params IRecurrenceEntity[] results)
      {
         var seq = new MockSequence();
         foreach (var result in results)
            _Mock.InSequence(seq)
               .Setup(m => m.InsertAsync(result))
               .Returns(Task.CompletedTask);
         return this;
      }

      public RecurrenceRepositoryMocker WithUpdate(Exception ex)
      {
         _Mock
            .Setup(m => m.UpdateAsync(It.IsAny<IRecurrenceEntity>()))
            .ThrowsAsync(ex);
         return this;
      }

      public IRecurrenceRepository Build() => _Mock.Object;
   }
}
