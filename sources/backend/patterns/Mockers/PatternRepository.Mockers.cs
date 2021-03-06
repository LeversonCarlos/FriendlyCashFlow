using Moq;

namespace Elesse.Patterns.Tests
{
   internal class PatternRepositoryMocker
   {

      readonly Mock<IPatternRepository> _Mock;
      public PatternRepositoryMocker() => _Mock = new Mock<IPatternRepository>();
      public static PatternRepositoryMocker Create() => new PatternRepositoryMocker();

      public PatternRepositoryMocker WithList() =>
         WithList(new IPatternEntity[] { });
      public PatternRepositoryMocker WithList(params IPatternEntity[][] results)
      {
         var seq = new MockSequence();
         foreach (var result in results)
            _Mock.InSequence(seq)
               .Setup(m => m.ListAsync())
               .ReturnsAsync(result);
         return this;
      }

      /*
      public PatternRepositoryMocker WithSearchPatterns() =>
         WithSearchPatterns(new IPatternEntity[][] { });
      public PatternRepositoryMocker WithSearchPatterns(params IPatternEntity[][] results)
      {
         var seq = new MockSequence();
         foreach (var result in results)
            _Mock.InSequence(seq)
               .Setup(m => m.SearchPatternsAsync(It.IsAny<enPatternType>(), It.IsAny<string>()))
               .ReturnsAsync(result);
         var seq2 = new MockSequence();
         foreach (var result in results)
            _Mock.InSequence(seq2)
               .Setup(m => m.SearchPatternsAsync(It.IsAny<enPatternType>(), It.IsAny<Shared.EntityID>(), It.IsAny<string>()))
               .ReturnsAsync(result);
         return this;
      }
      */

      public PatternRepositoryMocker WithLoadPattern() =>
         WithLoadPattern(new IPatternEntity[] { });
      public PatternRepositoryMocker WithLoadPattern(params IPatternEntity[] results)
      {
         var seq = new MockSequence();
         foreach (var result in results)
            _Mock.InSequence(seq)
               .Setup(m => m.LoadAsync(It.IsAny<Shared.EntityID>()))
               .ReturnsAsync(result);
         var seq2 = new MockSequence();
         foreach (var result in results)
            _Mock.InSequence(seq2)
               .Setup(m => m.LoadAsync(It.IsAny<enPatternType>(), It.IsAny<Shared.EntityID>(), It.IsAny<string>()))
               .ReturnsAsync(result);
         return this;
      }

      public IPatternRepository Build() => _Mock.Object;
   }
}
