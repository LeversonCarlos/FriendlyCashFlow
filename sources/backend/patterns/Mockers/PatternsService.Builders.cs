namespace Elesse.Patterns
{
   partial class PatternService
   {

      internal static PatternService Create() =>
         new PatternService(
            Tests.PatternRepositoryMocker.Create().Build(),
            Shared.Tests.InsightsServiceMocker.Create().Build()
            );

      internal static PatternService Create(IPatternRepository patternRepository) =>
         new PatternService(
            patternRepository,
            Shared.Tests.InsightsServiceMocker.Create().Build()
         );

   }
}
