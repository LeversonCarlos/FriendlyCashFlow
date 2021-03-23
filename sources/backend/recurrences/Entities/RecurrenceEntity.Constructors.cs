namespace Elesse.Recurrences
{

   partial class RecurrenceEntity
   {

      private RecurrenceEntity(Shared.EntityID recurrenceID, IRecurrenceProperties properties)
      {
         RecurrenceID = recurrenceID;
         SetProperties(properties);
      }

      public static RecurrenceEntity Create(IRecurrenceProperties properties) =>
         Restore(Shared.EntityID.NewID(), properties);

      internal static RecurrenceEntity Restore(Shared.EntityID recurrenceID, IRecurrenceProperties properties) =>
         new RecurrenceEntity(recurrenceID, properties);

   }

}
