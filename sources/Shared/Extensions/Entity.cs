namespace Lewio.Shared;

public static partial class EntityExtensions
{

   public static bool IsValid(this EntityID? id)
   {
      if (id == null || !id.HasValue)
         return false;
      return id.Value.IsValid();
   }

   public static bool IsValid(this EntityID id)
   {
      if (id == null)
         return false;
      if (string.IsNullOrEmpty((string)id))
         return false;
      return true;
   }

   public static void EnsureValid(this EntityID id)
   {

      if (!id.IsValid())
         throw new Exception("Invalid entity ID");

   }

}
