namespace Lewio.Shared;

public struct EntityID
{

   public string Value { get; private set; }

   public static implicit operator string(EntityID id) => id.Value;
   public static implicit operator EntityID(string id) => EntityID.Create(id);

   public static EntityID New() =>
      EntityID.Create(Guid.NewGuid().ToString());
   public static EntityID Create(string value) =>
      new EntityID
      {
         Value = value
      };

}

public static class EntityIDExtensions
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
      if (string.IsNullOrEmpty(id.ToString()))
         return false;
      return true;
   }

}
