using System;

namespace Elesse.Identity
{
   public interface ITokenEntity
   {
      string TokenID { get; }
      string UserID { get; }
      DateTime ExpirationDate { get; }
   }
}