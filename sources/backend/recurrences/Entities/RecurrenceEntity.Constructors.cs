namespace Elesse.Recurrences
{

   partial class RecurrenceEntity
   {

      public static RecurrenceEntity Create(IRecurrenceProperties properties) =>
         Restore(Shared.EntityID.NewID(), properties);

      internal static RecurrenceEntity Restore(Shared.EntityID recurrenceID, IRecurrenceProperties properties) =>
         new RecurrenceEntity
         {
            RecurrenceID = recurrenceID,
            Properties = properties
         };

   }

}
