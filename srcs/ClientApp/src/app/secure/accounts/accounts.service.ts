import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { BusyService } from 'src/app/shared/busy/busy.service';
import { Router } from '@angular/router';
import { Account, AccountType } from './accounts.viewmodels';

@Injectable({
   providedIn: 'root'
})
export class AccountsService {

   constructor(private busy: BusyService,
      private http: HttpClient, private router: Router) { }

   // NAVIGATES
   public showList() { this.router.navigate(['/accounts']); }
   public showDetails(id: number) { this.router.navigate(['/accounts', id], { skipLocationChange: true }); }
   public showNew() { this.router.navigate(['/accounts', 'new'], { skipLocationChange: true }); }

   // ACCOUNT TYPES
   public async getAccountTypes(): Promise<AccountType[]> {
      try {
         this.busy.show();
         const dataList = await this.http.get<AccountType[]>("api/accounts/types")
            .pipe(map(items => items.map(item => Object.assign(new AccountType(), item))))
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
