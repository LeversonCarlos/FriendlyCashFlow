import { Injectable } from '@angular/core';
import { StorageService } from '@elesse/shared';
import { AccountEntity } from '../model/accounts.model';

@Injectable({
   providedIn: 'root'
})
export class AccountsCache extends StorageService<boolean, AccountEntity[]> {

   constructor() {
      super("AccountsCache");
      this.InitializeValues(...this.AccountStates);
   }

   private get AccountStates(): boolean[] {
      return [
         true,
         false
      ];
   }

   public SetAccounts(accounts: AccountEntity[]) {

      // SORTER FUNCTION
      const sorter = (a: AccountEntity, b: AccountEntity): number => {
         let result = 0;
         if (a.Type > b.Type) result += 10;
         if (a.Type < b.Type) result -= 10;
         if (a.Text > b.Text) result += 1;
         if (a.Text < b.Text) result -= 1;
      return result;
      }

      // LOOP THROUGH ACCOUNT STATES
      for (const accountState of this.AccountStates) {

         // FILTER, SORT AND PARSE OBJECTS
         const value = accounts
            .filter(account => account.Active == accountState)
            .map(account => AccountEntity.Parse(account))
            .sort(sorter);

         // SET VALUE
         this.SetValue(accountState, value);

      }
   }

}
