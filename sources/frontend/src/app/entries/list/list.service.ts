import { AccountEntries, DayEntries, EntryEntity } from '../entries.data';

export class ListService {

   public static GetEntriesAccounts(entries: EntryEntity[]): AccountEntries[] {
      if (entries == null || entries.length == 0)
         return [];

      const accountGroups = entries
         .reduce((acc, cur) => {
            const key = cur.AccountID;
            acc[key] = acc[key] || [];
            acc[key].push(cur);
            return acc;
         }, {});

      const accountIDs = Object
         .keys(accountGroups)
         .sort((p, n) => p < n ? -1 : 1)

      const accountResults = accountIDs
         .map(accountID => {
            return {
               accountID,
               Days: ListService.GetEntriesDays(accountGroups[accountID])
            };
         })
         .map(account => Object.assign(new AccountEntries, {
            AccountID: account.accountID,
            Days: account.Days,
            Balance: account.Days[account.Days.length - 1].Balance
         }));

      return accountResults;
   }

   public static GetEntriesDays(entries: EntryEntity[]): DayEntries[] {
      if (entries == null || entries.length == 0)
         return [];

      const dayGroups = entries
         .reduce((acc, cur) => {
            const key = (new Date(cur.SearchDate)).toISOString().substring(0, 10);
            acc[key] = acc[key] || [];
            acc[key].push(cur);
            return acc;
         }, {});

      const days = Object
         .keys(dayGroups)
         .sort((p, n) => p < n ? -1 : 1)

      const dayResult = days
         .map(day => {
            return {
               day,
               Entries: dayGroups[day]
                  .sort((p, n) => p.Sorting < n.Sorting ? -1 : 1)
            };
         })
         .map(day => Object.assign(new DayEntries, {
            Day: day.day,
            Entries: day.Entries,
            Balance: day.Entries[day.Entries.length - 1].Balance.Account
         }));

      return dayResult;
   }

}
