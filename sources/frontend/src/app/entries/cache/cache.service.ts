import { Injectable } from '@angular/core';
import { StorageService } from '@elesse/shared';
import { BalanceItemEntity, EntryEntity } from '../model/entries.model';

@Injectable({
   providedIn: 'root'
})
export class EntriesCache extends StorageService<string, EntryEntity[]> {

   constructor() {
      super("EntriesCache");
   }

   public SetEntries(key: string, value: EntryEntity[]) {

      // SORTER FUNCTION
      const sorter = (a: EntryEntity, b: EntryEntity): number => {
         let result = 0;
         if (a.Sorting > b.Sorting) result += 1;
         if (a.Sorting < b.Sorting) result -= 1;
         return result;
      }

      // SORT AND PARSE OBJECTS
      const entries = value
         .map(entry => EntryEntity.Parse(entry))
         .sort(sorter);

      // INITIALIZE BALANCE
      const totalBalance = new BalanceItemEntity();
      const accountsBalance: Record<string, BalanceItemEntity> = {};

      // LOOP THROUGH ENTRIES
      for (let index = 0; index < entries.length; index++) {
         const entry = entries[index];

         // CURRENT ENTRY VALUES
         const expectedValue = entry.EntryValue;
         const realizedValue = entry.Paid ? entry.EntryValue : 0;

         // ACCUMULATE TOTAL BALANCE
         totalBalance.ExpectedBalance += expectedValue;
         totalBalance.RealizedBalance += realizedValue;
         entry.Balance.Total.ExpectedBalance = totalBalance.ExpectedBalance;
         entry.Balance.Total.RealizedBalance = totalBalance.RealizedBalance;

         // ADD CURRENT ACCOUNT TO DICTIONARY IF HASNT YET
         if (accountsBalance[entry.AccountID] == null)
            accountsBalance[entry.AccountID] = new BalanceItemEntity();

         // ACCUMULATE CURRENT ACCOUNT BALANCE
         const accountBalance = accountsBalance[entry.AccountID];
         accountBalance.ExpectedBalance += expectedValue;
         accountBalance.RealizedBalance += realizedValue;
         entry.Balance.Account.ExpectedBalance = accountBalance.ExpectedBalance;
         entry.Balance.Account.RealizedBalance = accountBalance.RealizedBalance;
      }

      this.SetValue(key, entries);
   }

}
