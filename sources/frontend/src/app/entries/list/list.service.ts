import { AccountEntries, EntryEntity } from '../entries.data';

export class ListService {

   public static GetEntriesAccounts(entries: EntryEntity[]): AccountEntries[] {
      if (entries == null || entries.length == 0)
         return [];
   }

}
