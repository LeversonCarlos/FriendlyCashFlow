using Moq;

namespace Elesse.Entries.Tests
{
   internal class EntryRepositoryMocker
   {

      readonly Mock<IEntryRepository> _Mock;
      public EntryRepositoryMocker() => _Mock = new Mock<IEntryRepository>();
      public static EntryRepositoryMocker Create() => new EntryRepositoryMocker();

      public EntryRepositoryMocker WithList() =>
         WithList(new IEntryEntity[] { });
      public EntryRepositoryMocker WithList(params IEntryEntity[][] results)
      {
         var seq = new MockSequence();
         foreach (var result in results)
            _Mock.InSequence(seq)
               .Setup(m => m.ListAsync(It.IsAny<int>(), It.IsAny<int>()))
               .ReturnsAsync(result);
         return this;
      }

      public EntryRepositoryMocker WithLoad() =>
         WithLoad(new IEntryEntity[] { });
      public EntryRepositoryMocker WithLoad(params IEntryEntity[] results)
      {
         var seq = new MockSequence();
         foreach (var result in results)
            _Mock.InSequence(seq)
               .Setup(m => m.LoadAsync(It.IsAny<Shared.EntityID>()))
               .ReturnsAsync(result);
         return this;
      }

      public IEntryRepository Build() => _Mock.Object;
   }
}
