using System.Threading.Tasks;

namespace FriendlyCashFlow.Identity
{
   partial class IdentityService
   {

      internal Task<string[]> ValidatePasswordAsync(string password)
      {
         using (var interactor = new ValidatePasswordInteractor(_MongoDatabase, _Settings))
         {
            return interactor.ExecuteAsync(password);
         }
      }

   }
}
