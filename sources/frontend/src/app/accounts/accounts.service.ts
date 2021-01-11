import { Injectable } from '@angular/core';
import { AccountEntity, enAccountType } from './accounts.data';
import { Observable, Subject } from 'rxjs';
import { StorageService } from '@elesse/shared';
import { HttpClient } from '@angular/common/http';

@Injectable({
   providedIn: 'root'
})
export class AccountsService {

   constructor(private http: HttpClient) {
      this._Data = new StorageService<boolean, AccountEntity[]>("AccountsService");
      this._Data.InitializeValues(false, true);
   }

   private _Data: StorageService<boolean, AccountEntity[]>;
   public GetData = (state: boolean = true): Observable<AccountEntity[]> => this._Data.GetObservable(state);

   public async RefreshData(): Promise<void> {
      try {
         const values = await this.http.get<AccountEntity[]>(`api/accounts/list`).toPromise();
         if (!values) return;

         const sorter = (a: AccountEntity, b: AccountEntity): number => {
            let result = 0;
            if (a.Type > b.Type) result += 10;
            if (a.Type < b.Type) result -= 10;
            if (a.Text > b.Text) result += 1;
            if (a.Text < b.Text) result -= 1;
            return result;
         }

         const keys = [false, true];
         keys.forEach(key => {
            const value = values
               .filter(x => x.Active == key)
               .sort(sorter)
            this._Data.SetValue(key, value);
         });

      }
      catch { /* error absorber */ }
   }

   public async LoadAccount(accountID: string): Promise<AccountEntity> {
      try {

         if (!accountID)
            return null;

         if (accountID == 'new')
            return Object.assign(new AccountEntity, { Type: enAccountType.General });

         let value = await this.http.get<AccountEntity>(`api/accounts/load/${accountID}`).toPromise();
         if (!value)
            return null;

         value = Object.assign(new AccountEntity, value);
         return value;

      }
      catch { /* error absorber */ }
   }

   public getAccountIcon(type: enAccountType): string {
      switch (type) {
         case enAccountType.Bank:
            return 'account_balance';
         case enAccountType.CreditCard:
            return 'credit_card';
         case enAccountType.Investment:
            return 'local_atm';
         case enAccountType.Service:
            return 'card_giftcard';
         default:
            return 'account_balance_wallet';
      }
   }

}
