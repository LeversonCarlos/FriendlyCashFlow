using System;
using MongoDB.Driver;

namespace FriendlyCashFlow.Identity
{

   internal partial class IdentityService : IIdentityService
   {

      readonly IMongoClient _MongoClient;
      public IdentityService(IMongoClient mongoClient)
      {
         _MongoClient = mongoClient;
      }

   }

   public partial interface IIdentityService
   {
   }

}
