using System;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace Elesse.Patterns
{
   partial class PatternRepository
   {

      public Task<IPatternEntity> LoadPatternAsync(EntityID patternID) =>
         throw new System.NotImplementedException();

      public Task<IPatternEntity> LoadPatternAsync(enPatternType type, EntityID categoryID, string text) =>
         throw new System.NotImplementedException();

   }
}
