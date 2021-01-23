using System;

namespace Elesse.Entries
{
   partial class EntryEntity
   {

      internal static EntryEntity Mock() =>
         EntryEntity.Create(Patterns.PatternEntity.Mock(), Shared.EntityID.NewID(), DateTime.Now, (decimal)12.34);

   }
}
