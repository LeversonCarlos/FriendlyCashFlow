using FriendlyCashFlow.Shared;
using MongoDB.Driver;
using System;
using System.Threading.Tasks;

namespace FriendlyCashFlow.Identity.Interactors
{
   internal class ValidateUsername : Interactor<IUser, string, string[]>
   {

      public ValidateUsername(IMongoDatabase mongoDatabase) :
         base(mongoDatabase, IdentityService.CollectionName)
      { }

      internal const string WARNING_IDENTITY_INVALID_USERNAME = "WARNING_IDENTITY_INVALID_USERNAME";

      public override async Task<string[]> ExecuteAsync(string username)
      {
         try
         {
            var mailAddress = new System.Net.Mail.MailAddress(username);
            await Task.CompletedTask;

            return new string[] { };
         }
         catch (Exception) { return new string[] { WARNING_IDENTITY_INVALID_USERNAME }; }
      }

   }
}
