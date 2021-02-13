import { Injectable } from '@angular/core';
import { StorageService } from '@elesse/shared';
import { BehaviorSubject, Observable, Subscription } from 'rxjs';
import { BalanceItemEntity, EntryEntity } from '../model/entries.model';

@Injectable({
   providedIn: 'root'
})
export class EntriesCache extends StorageService<string, EntryEntity[]> {

   constructor() {
      super("EntriesCache");
      this.Subject = new BehaviorSubject<EntryEntity[]>(null);
      this.Observe = this.Subject.asObservable();
      this.InitializeValue
   }

   public InitializeValue(key: string) {
      super.InitializeValue(key);
      if (this.Subs) {
         if (this.SubsKey == key)
            return;
         this.Subs.unsubscribe();
      }
      this.Subs = this.GetObservable(key).subscribe(values => this.Subject.next(values));
      this.SubsKey = key;
   }

   private SubsKey: string;
   private Subs: Subscription;
   private Subject: BehaviorSubject<EntryEntity[]>;
   public Observe: Observable<EntryEntity[]>;

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
