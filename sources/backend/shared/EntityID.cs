namespace Elesse.Shared
{

   public class EntityID : EntityID<Guid>
   {
      public EntityID() : base(Guid.NewGuid()) { }
      public EntityID(Guid guid) : base(guid) { }

      public static explicit operator string(EntityID id) => id.Value.ToString();
   }

   public class EntityID<T> : ValueObject
   {

      public T Value { get; }

      protected EntityID(T value) => Value = value;

      protected override IEnumerable<object> GetAtomicValues()
      {
         yield return Value;
      }

      // public static implicit operator T(EntityID<T> id) => id.Value;
      public static explicit operator EntityID<T>(T value) => new EntityID<T>(value);
      public static explicit operator T(EntityID<T> id) => id.Value;

      /*
         ESSES OPERATORS PERMITIRA:
         Guid guid1 = Guid.NewGuid();
         EntityID<Guid> id = (EntityID<Guid>)guid1;
         Guid guid = (Guid)id;
      */

   }

}
