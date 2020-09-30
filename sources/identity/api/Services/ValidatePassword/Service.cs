using System.Threading.Tasks;

namespace FriendlyCashFlow.Identity
{
   partial class IdentityService
   {

      internal Task<string[]> ValidatePasswordAsync(string password)
      {
         using (var interactor = new Interactors.ValidatePassword(_MongoDatabase))
         {
            return interactor.ExecuteAsync(password);
         }
      }

   }
}
