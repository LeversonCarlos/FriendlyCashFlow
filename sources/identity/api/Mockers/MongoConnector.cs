using MongoDB.Driver;

namespace FriendlyCashFlow.Identity.Tests
{

   internal class MongoConnector
   {

      readonly IMongoClient _MongoClient;
      public MongoConnector() => _MongoClient = new MongoClient("mongodb+srv://m220student:m220password@mflix.wxo2z.gcp.mongodb.net/<dbname>?retryWrites=true&w=majority");
      public static MongoConnector Create() => new MongoConnector();

      public IMongoClient BuildClient() => _MongoClient;
      public IMongoDatabase BuildDatabase() => _MongoClient.GetDatabase("identity");
   }

}