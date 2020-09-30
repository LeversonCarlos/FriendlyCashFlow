using System.Threading.Tasks;

namespace FriendlyCashFlow.Identity
{
   partial class IdentityService
   {

      internal Task<string[]> ValidateUsernameAsync(string username)
      {
         using (var interactor = new Interactors.ValidateUsername(_MongoDatabase, _Settings))
         {
            return interactor.ExecuteAsync(username);
         }
      }

   }
}
