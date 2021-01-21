using System;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace Elesse.Patterns
{
   partial class PatternRepository
   {
      DateTime LimitDate = DateTime.UtcNow.AddYears(-10);

      public async Task<IPatternEntity[]> ListPatternsAsync()
      {
         var list = await _Collection
            .Find(entity => entity.RowsDate >= LimitDate)
            .ToListAsync();
         return list.ToArray();
      }

   }
}
