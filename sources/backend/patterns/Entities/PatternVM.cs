using System.Collections.Generic;
using Elesse.Shared;

namespace Elesse.Patterns
{

   public interface IPatternVM
   {
      enPatternType Type { get; }
      EntityID CategoryID { get; }
      string Text { get; }
   }

   public class PatternVM : ValueObject, IPatternVM
   {

      internal PatternVM(enPatternType type, EntityID categoryID, string text)
      {
         Type = type;
         CategoryID = categoryID;
         Text = text;
      }

      public enPatternType Type { get; }
      public EntityID CategoryID { get; }
      public string Text { get; }

      protected override IEnumerable<object> GetAtomicValues()
      {
         yield return Type;
         yield return CategoryID;
         yield return Text;
      }

   }
}
