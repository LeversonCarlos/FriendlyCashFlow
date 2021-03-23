using System.Threading.Tasks;

namespace Elesse.Entries
{
   internal interface IEntryRepository
   {

      Task InsertAsync(IEntryEntity value);
      Task UpdateAsync(IEntryEntity value);
      Task DeleteAsync(Shared.EntityID entryID);

      Task<IEntryEntity[]> ListAsync(int year, int month);
      Task<IEntryEntity> LoadAsync(Shared.EntityID entryID);
      Task<IEntryEntity[]> LoadRecurrencesAsync(Shared.EntityID recurrencyID);

   }
}
