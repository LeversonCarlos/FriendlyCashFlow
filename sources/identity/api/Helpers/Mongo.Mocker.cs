using MongoDB.Driver;

namespace FriendlyCashFlow.Identity.Tests
{
   internal class MongoMocker
   {

      readonly IMongoClient _MongoClient;
      public MongoMocker() => _MongoClient = new MongoClient("mongodb+srv://m220student:m220password@mflix.wxo2z.gcp.mongodb.net/<dbname>?retryWrites=true&w=majority");
      public static MongoMocker Create() => new MongoMocker();

      public IMongoClient Build() => _MongoClient;
   }
}