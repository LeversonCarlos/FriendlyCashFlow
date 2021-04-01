using System;

namespace Elesse.Shared
{
   public abstract class DomainEventBase : IDomainEvent
   {

      public DomainEventBase()
      {
         this.ID = Guid.NewGuid();
         this.OccurredOn = DateTime.UtcNow;
      }

      public Guid ID { get; }
      public DateTime OccurredOn { get; }

   }
}
