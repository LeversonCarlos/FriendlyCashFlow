using Moq;

namespace Elesse.Entries.Tests
{
   internal class EntryRepositoryMocker
   {

      readonly Mock<IEntryRepository> _Mock;
      public EntryRepositoryMocker() => _Mock = new Mock<IEntryRepository>();
      public static EntryRepositoryMocker Create() => new EntryRepositoryMocker();

      /*
      public EntryRepositoryMocker WithList() =>
         WithList(new IEntryEntity[] { });
      public EntryRepositoryMocker WithList(params IEntryEntity[][] results)
      {
         var seq = new MockSequence();
         foreach (var result in results)
            _Mock.InSequence(seq)
               .Setup(m => m.ListPatternsAsync())
               .ReturnsAsync(result);
         return this;
      }
      */

      /*
      public EntryRepositoryMocker WithLoadPattern() =>
         WithLoadPattern(new IEntryEntity[] { });
      public EntryRepositoryMocker WithLoadPattern(params IEntryEntity[] results)
      {
         var seq = new MockSequence();
         foreach (var result in results)
            _Mock.InSequence(seq)
               .Setup(m => m.LoadPatternAsync(It.IsAny<Shared.EntityID>()))
               .ReturnsAsync(result);
         var seq2 = new MockSequence();
         foreach (var result in results)
            _Mock.InSequence(seq2)
               .Setup(m => m.LoadPatternAsync(It.IsAny<enPatternType>(), It.IsAny<Shared.EntityID>(), It.IsAny<string>()))
               .ReturnsAsync(result);
         return this;
      }
      */

      public IEntryRepository Build() => _Mock.Object;
   }
}
