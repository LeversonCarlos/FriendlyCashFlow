namespace Elesse.Patterns
{

   public enum enPatternType : short { Expense = 1, Income = 2 }

   [Shared.JsonInterfaceConverter(typeof(PatternEntityConverter))]
   public interface IPatternEntity
   {
      Shared.EntityID PatternID { get; }
      enPatternType Type { get; }
      Shared.EntityID CategoryID { get; }
      string Text { get; }
   }

}
