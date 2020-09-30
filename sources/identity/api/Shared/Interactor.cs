using MongoDB.Driver;
using System;
using System.Threading.Tasks;

namespace FriendlyCashFlow.Shared
{
   public abstract class Interactor<DATA, PARAM, RESULT> : IDisposable
   {

      public Interactor(IMongoDatabase mongoDatabase, string collectionName)
      {
         _CollectionName = collectionName;
         _MongoDatabase = mongoDatabase;
      }

      readonly string _CollectionName;
      readonly IMongoDatabase _MongoDatabase;

      protected IMongoCollection<DATA> Collection =>
         _MongoDatabase.GetCollection<DATA>(_CollectionName);

      public virtual Task<RESULT> ExecuteAsync(PARAM param) =>
         Task.FromResult(default(RESULT));

      public virtual void Dispose() { }

   }
}
