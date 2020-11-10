using System;

namespace Elesse.Identity
{
   public interface IRefreshToken
   {
      string TokenID { get; }
      string UserID { get; }
      DateTime ExpirationDate { get; }
   }
}