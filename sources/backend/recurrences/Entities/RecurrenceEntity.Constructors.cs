using System;

namespace Elesse.Recurrences
{

   partial class RecurrenceEntity
   {

      public static RecurrenceEntity Create(IRecurrenceProperties properties) =>
         new RecurrenceEntity
         {
            RecurrenceID = Shared.EntityID.NewID(),
            Properties = properties
         };

   }

}
