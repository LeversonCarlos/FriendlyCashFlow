import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { BusyService } from 'src/app/shared/busy/busy.service';

export enum enAccountType { General = 0, Bank = 1, CreditCard = 2, Investment = 3, Service = 4 };
export class Account {
   AccountID: number;
   Text: string;
   Type: enAccountType;
   DueDay?: number;
   Active: boolean;
   get Icon(): string {
      switch (this.Type) {
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

@Injectable({
   providedIn: 'root'
})
export class AccountsService {

   constructor(private busy: BusyService, private http: HttpClient) { }

   public async getAccounts(): Promise<Account[]> {
      try {
         this.busy.show();
         const dataList = await this.http.get<Account[]>("api/accounts")
            .pipe(map(items => items.map(item => Object.assign(new Account, item))))
            .toPromise();
         return dataList;
      }
      catch (ex) { console.error(ex); return null; }

   }

   public async getAccount(accountID: number): Promise<Account> {
      try {
         const dataList = await this.http.get<Account>(`api/accounts/${accountID}`)
            .pipe(map(item => Object.assign(new Account, item)))
            .toPromise();
         return dataList;
      }
      catch (ex) { console.error(ex); return null; }

   }

}
