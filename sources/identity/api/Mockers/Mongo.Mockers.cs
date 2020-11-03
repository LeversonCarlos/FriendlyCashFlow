using MongoDB.Driver;
using Moq;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace Elesse.Identity.Tests
{

   internal class MongoCollectionMocker<T>
   {

      readonly Mock<IMongoCollection<T>> Mock;
      public MongoCollectionMocker() => Mock = new Mock<IMongoCollection<T>>();
      public static MongoCollectionMocker<T> Create(string collectionName = "users") => (new MongoCollectionMocker<T>()).WithName(collectionName);

      public MongoCollectionMocker<T> WithName(string collectionName)
      {
         Mock.SetupGet(m => m.CollectionNamespace).Returns(new CollectionNamespace("databaseMockerName", collectionName));
         return this;
      }

      public MongoCollectionMocker<T> WithCount(params int[] countResults)
      {
         var seq = new MockSequence();
         foreach (var count in countResults)
            Mock.InSequence(seq).Setup(m => m.CountDocumentsAsync(It.IsAny<FilterDefinition<T>>(), It.IsAny<CountOptions>(), It.IsAny<CancellationToken>())).ReturnsAsync(count);
         return this;
      }

      public MongoCollectionMocker<T> WithFind(params T[] results)
      {
         Mock.Setup(m => m.FindAsync<T>(It.IsAny<FilterDefinition<T>>(), null, It.IsAny<CancellationToken>())).ReturnsAsync(GetCursor(results));
         return this;
      }

      public MongoCollectionMocker<T> WithFindAndDelete(T results)
      {
         Mock.Setup(m => m.FindOneAndDeleteAsync<T>(It.IsAny<FilterDefinition<T>>(), null, It.IsAny<CancellationToken>())).ReturnsAsync(results);
         return this;
      }

      IAsyncCursor<D> GetCursor<D>(params D[] results)
      {
         if (results == null)
            return null;

         var mockCursor = new Mock<IAsyncCursor<D>>();
         mockCursor.Setup(x => x.Current).Returns(results);

         var moveNext = mockCursor.SetupSequence(x => x.MoveNext(It.IsAny<CancellationToken>()));
         var moveNextAsync = mockCursor.SetupSequence(x => x.MoveNextAsync(It.IsAny<CancellationToken>()));
         for (int i = 0; i < results.Length; i++)
         {
            moveNext = moveNext.Returns(true);
            moveNextAsync = moveNextAsync.ReturnsAsync(true);
         }
         moveNext = moveNext.Returns(false);
         moveNextAsync = moveNextAsync.ReturnsAsync(false);

         return mockCursor.Object;
      }


      public IMongoCollection<T> Build() => Mock.Object;

   }

   internal class MongoDatabaseMocker
   {

      readonly Mock<IMongoDatabase> Mock;
      public MongoDatabaseMocker() => Mock = new Mock<IMongoDatabase>();
      public static MongoDatabaseMocker Create() => new MongoDatabaseMocker();

      public MongoDatabaseMocker WithCollection<T>(IMongoCollection<T> collection, string collectionName)
      {
         Mock.Setup(m => m.GetCollection<T>(collectionName, null)).Returns(collection);
         return this;
      }

      public IMongoDatabase Build() => Mock.Object;
   }
}