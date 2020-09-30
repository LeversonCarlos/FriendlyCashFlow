using MongoDB.Driver;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace FriendlyCashFlow.Shared
{
   public abstract class Interactor<CollectionType, SettingsType, PARAM, RESULT> : IDisposable
   {

      public Interactor(IMongoDatabase mongoDatabase, SettingsType settings, string collectionName)
      {
         _CollectionName = collectionName;
         MongoDatabase = mongoDatabase;
         Settings = settings;
      }

      readonly string _CollectionName;
      readonly protected IMongoDatabase MongoDatabase;

      protected SettingsType Settings;

      protected IMongoCollection<CollectionType> Collection =>
         MongoDatabase.GetCollection<CollectionType>(_CollectionName);

      [ExcludeFromCodeCoverage]
      public virtual Task<RESULT> ExecuteAsync(PARAM param) =>
         Task.FromResult(default(RESULT));

      public virtual void Dispose() { }

   }
}
