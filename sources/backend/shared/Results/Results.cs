using System;
using System.Collections.Generic;

namespace Elesse.Shared
{

   public enum enResultType : short { Info = 0, Warning = 1, /*Error=2,*/ Exception = 3 }

   public partial class Results : ValueObject
   {

      internal Results(enResultType type, string[] messages)
      {
         Type = type;
         Messages = messages;
      }

      internal Results(Exception ex)
      {
         Type = enResultType.Exception;
         Details = ex.ToString();
      }

      public enResultType Type { get; }
      public string Details { get; }
      public string[] Messages { get; }

      protected override IEnumerable<object> GetAtomicValues()
      {
         yield return Type;
         yield return Details;
         yield return string.Join("|", Messages ?? new string[] { });
      }

   }

}
