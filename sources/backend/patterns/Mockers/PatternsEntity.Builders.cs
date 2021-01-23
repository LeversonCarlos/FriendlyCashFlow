namespace Elesse.Patterns
{
   partial class PatternEntity
   {

      internal static PatternEntity Mock() =>
         new PatternEntity(Shared.EntityID.NewID(), enPatternType.Income, Shared.EntityID.NewID(), "My Dummy Pattern");

   }
}
