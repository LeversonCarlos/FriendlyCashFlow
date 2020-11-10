using System.Security.Claims;

namespace Elesse.Identity
{
   public interface IUser
   {
      string UserID { get; }
      string UserName { get; }
      ClaimsIdentity Identity { get; }
   }
}