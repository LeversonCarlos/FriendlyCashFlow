using FriendlyCashFlow.Shared;
using MongoDB.Driver;
using System;
using System.Threading.Tasks;

namespace FriendlyCashFlow.Identity
{
   internal class ValidateUsernameInteractor : Interactor<IUser, IdentitySettings, string, string[]>
   {

      public ValidateUsernameInteractor(IMongoDatabase mongoDatabase, IdentitySettings settings) :
         base(mongoDatabase, settings, IdentityService.CollectionName)
      { }

      internal struct WARNING
      {
         internal const string INVALID_USERNAME = "WARNING_IDENTITY_INVALID_USERNAME";
      }

      public override async Task<string[]> ExecuteAsync(string username)
      {
         try
         {
            var mailAddress = new System.Net.Mail.MailAddress(username);
            await Task.CompletedTask;

            return new string[] { };
         }
         catch (Exception) { return new string[] { WARNING.INVALID_USERNAME }; }
      }

   }
}
