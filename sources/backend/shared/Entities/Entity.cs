using System;
using System.Collections.Generic;

namespace Elesse.Shared
{
   public abstract class Entity<T> where T : EntityID
   {

      T _ID;
      public T ID
      {
         get => _ID;
         protected set
         {
            if (value == null)
               throw new ArgumentException("Invalid null entity ID");
            _ID = value;
         }
      }

      List<IDomainEvent> _DomainEvents;
      public IReadOnlyCollection<IDomainEvent> DomainEvents => _DomainEvents?.AsReadOnly();

      public void ClearDomainEvents() =>
         _DomainEvents?.Clear();

      protected void AddDomainEvent(IDomainEvent domainEvent)
      {
         _DomainEvents ??= new List<IDomainEvent>();
         _DomainEvents.Add(domainEvent);
      }

      protected static void EnsureBusinessRule(IBusinessRule rule)
      {
         if (rule.IsBroken())
            throw new BusinessRuleException(rule);
      }

   }
}
