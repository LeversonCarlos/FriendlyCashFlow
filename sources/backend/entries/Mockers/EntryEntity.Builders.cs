using System;

namespace Elesse.Entries
{
   partial class EntryEntity
   {

      internal static EntryEntity Mock() =>
         EntryEntity.Create(Patterns.PatternEntity.Builder().Build(), Shared.EntityID.NewID(), DateTime.Now, (decimal)12.34);

   }
}
