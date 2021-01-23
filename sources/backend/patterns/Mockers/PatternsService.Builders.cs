namespace Elesse.Patterns
{
   partial class PatternService
   {

      internal static PatternService Mock() =>
         new PatternService(
            Tests.PatternRepositoryMocker.Create().Build(),
            Shared.Tests.InsightsServiceMocker.Create().Build()
            );

      internal static PatternService Mock(IPatternRepository patternRepository) =>
         new PatternService(
            patternRepository,
            Shared.Tests.InsightsServiceMocker.Create().Build()
         );

   }
}
