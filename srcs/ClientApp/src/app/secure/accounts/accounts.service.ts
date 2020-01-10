import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { BusyService } from 'src/app/shared/busy/busy.service';
import { Router } from '@angular/router';
import { EnumVM } from 'src/app/shared/common/common.models';
import { TranslationService } from 'src/app/shared/translation/translation.service';

export enum enAccountType { General = 0, Bank = 1, CreditCard = 2, Investment = 3, Service = 4 };
export class Account {
   AccountID: number;
   Text: string;
   Type: enAccountType;
   ClosingDay?: number;
   DueDay?: number;
   DueDate?: Date;
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

   constructor(private busy: BusyService, private translation: TranslationService,
      private http: HttpClient, private router: Router) { }

   // NAVIGATES
   public showList() { this.router.navigate(['/accounts']); }
   public showDetails(id: number) { this.router.navigate(['/account', id]); }
   public showNew() { this.router.navigate(['/account', 'new']); }

   // ACCOUNT TYPES
   public async getAccountTypes(): Promise<EnumVM<enAccountType>[]> {
      try {
         this.busy.show();
         const dataList = await this.http.get<EnumVM<enAccountType>[]>("api/accounts/types")
            .pipe(map(items => items.map(item => Object.assign(new EnumVM<enAccountType>(), item))))
            .toPromise();
         return dataList;
      }
      catch (ex) { return null; }
      finally { this.busy.hide(); }
   }

   // ACCOUNTS
   public async getAccounts(searchText: string = ''): Promise<Account[]> {
      try {
         this.busy.show();
         let url = `api/accounts/search`;
         if (searchText) { url = `${url}/${encodeURIComponent(searchText)}`; }
         const dataList = await this.http.get<Account[]>(url)
            .pipe(map(items => items.map(item => Object.assign(new Account, item))))
            .toPromise();
         return dataList;
      }
      catch (ex) { return null; }
      finally { this.busy.hide(); }
   }

   // ACCOUNT
   public async getAccount(accountID: number): Promise<Account> {
      try {
         this.busy.show();
         const dataList = await this.http.get<Account>(`api/accounts/${accountID}`)
            .pipe(map(item => Object.assign(new Account, item)))
            .toPromise();
         return dataList;
      }
      catch (ex) { return null; }
      finally { this.busy.hide(); }
   }

   // SAVE
   public async saveAccount(value: Account): Promise<boolean> {
      try {
         this.busy.show();
         let result: Account = null;
         if (!value.AccountID || value.AccountID == 0) {
            result = await this.http.post<Account>(`api/accounts`, value).toPromise();
         }
         else {
            result = await this.http.put<Account>(`api/accounts/${value.AccountID}`, value).toPromise();
         }
         return result != null;
      }
      catch (ex) { return null; }
      finally { this.busy.hide(); }
   }

   // REMOVE
   public async removeAccount(value: Account): Promise<boolean> {
      try {
         this.busy.show();
         const result = await this.http.delete<boolean>(`api/accounts/${value.AccountID}`).toPromise();
         return result;
      }
      catch (ex) { return null; }
      finally { this.busy.hide(); }
   }

}
