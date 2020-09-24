using Xunit;

namespace FriendlyCashFlow.Identity.Tests
{
   public partial class IdentityServiceTests
   {

      [Fact]
      public async void GetCollectionAsync_WithNonExistingDatabase_MustCreateDatabaseAndReturnIt()
      {
         var collectionName = "users";
         var mongoCollection = MongoCollectionMocker<IUser>.Create().WithName(collectionName).Build();
         var mongoDatabase = MongoDatabaseMocker.Create().WithCollection(mongoCollection, true).Build();
         var identityService = new IdentityService(mongoDatabase);

         var result = await identityService.GetCollectionAsync();

         Assert.NotNull(result);
         Assert.Equal(collectionName, result.CollectionNamespace.CollectionName);
      }

      [Fact]
      public async void GetCollectionAsync_WithExistingDatabase_MustJustReturnIt()
      {
         var collectionName = "users";
         var mongoCollection = MongoCollectionMocker<IUser>.Create().WithName(collectionName).Build();
         var mongoDatabase = MongoDatabaseMocker.Create().WithCollection(mongoCollection, false).Build();
         var identityService = new IdentityService(mongoDatabase);

         var result = await identityService.GetCollectionAsync();

         Assert.NotNull(result);
         Assert.Equal(collectionName, result.CollectionNamespace.CollectionName);
      }

   }
}
