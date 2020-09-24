using MongoDB.Driver;
using System.Threading.Tasks;

namespace FriendlyCashFlow.Identity.Helpers
{
   internal static class Extentions
   {

      /*
      internal static bool CheckConnection(this IMongoDatabase mongoDatabase)
      {
         return mongoDatabase.RunCommandAsync((Command<MongoDB.Bson.BsonDocument>)"{ping:1}").Wait(1000);
      }
      */

      internal static async Task<IMongoCollection<T>> GetCollectionAsync<T>(this IMongoDatabase mongoDatabase, string collectionName)
      {

         // CHECK OF COLLECTION EXISTS
         var options = new ListCollectionNamesOptions { Filter = new MongoDB.Bson.BsonDocument("name", collectionName) };
         var collectionNames = await mongoDatabase.ListCollectionNamesAsync(options);

         // CREATE THE COLLECTION
         if (!await collectionNames.AnyAsync())
            await mongoDatabase.CreateCollectionAsync(collectionName);

         // RETRIEVE AND RETURN THE COLLECTION
         return mongoDatabase.GetCollection<T>(collectionName);

      }

   }
}
