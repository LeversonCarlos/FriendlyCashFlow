using System;
// using MediatR;

namespace Elesse.Shared
{
   public interface IDomainEvent // : INotification
   {
      Guid ID { get; }
      DateTime OccurredOn { get; }
   }
}
