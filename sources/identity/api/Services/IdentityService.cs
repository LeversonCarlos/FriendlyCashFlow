using System;
using MongoDB.Driver;

namespace FriendlyCashFlow.Identity
{

   internal partial class IdentityService : IIdentityService
   {

      readonly IMongoCollection<IUser> _UserCollection;

      public IdentityService(IMongoClient mongoClient)
      {
         _UserCollection = mongoClient.GetDatabase("identity").GetCollection<IUser>("users");
      }

   }

   public partial interface IIdentityService
   {
   }

}
