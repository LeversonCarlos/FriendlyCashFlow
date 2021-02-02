import { Injectable } from '@angular/core';
import { AccountEntity, AccountType, enAccountType } from '../model/accounts.model';
import { Observable } from 'rxjs';
import { BusyService, LocalizationService, MessageService, StorageService } from '@elesse/shared';
import { HttpClient } from '@angular/common/http';
import { AccountsCache } from '../cache/cache.service';

@Injectable({
   providedIn: 'root'
})
export class AccountsData {

   constructor(private localization: LocalizationService, private message: MessageService, private busy: BusyService,
      private http: HttpClient) {
      this.Cache = new AccountsCache();
   }

   private Cache: AccountsCache;

   public async RefreshAccounts(): Promise<void> {
      try {
         this.busy.show();

         const url = `api/accounts/list`;
         const values = await this.http.get<AccountEntity[]>(url).toPromise();
         if (!values)
            return;

         this.Cache.SetAccounts(values);

      }
      catch { /* error absorber */ }
      finally { this.busy.hide(); }
   }

   public ObserveAccounts = (state: boolean = true): Observable<AccountEntity[]> =>
      this.Cache.GetObservable(state);

   public GetAccounts = (state: boolean = true): AccountEntity[] =>
      this.Cache.GetValue(state);

   public async GetAccountTypes(): Promise<AccountType[]> {
      const accountTypes: AccountType[] = [
         { Value: enAccountType.General, Text: await this.localization.GetTranslation(`accounts.enAccountType_General`) },
         { Value: enAccountType.Bank, Text: await this.localization.GetTranslation(`accounts.enAccountType_Bank`) },
         { Value: enAccountType.CreditCard, Text: await this.localization.GetTranslation(`accounts.enAccountType_CreditCard`) },
         { Value: enAccountType.Investment, Text: await this.localization.GetTranslation(`accounts.enAccountType_Investment`) },
         { Value: enAccountType.Service, Text: await this.localization.GetTranslation(`accounts.enAccountType_Service`) }
      ];
      return accountTypes;
   }

   public GetAccountIcon(type: enAccountType): string {
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

   public async LoadAccount(accountID: string): Promise<AccountEntity> {
      try {
         this.busy.show();

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
      finally { this.busy.hide(); }
   }

   public async SaveAccount(account: AccountEntity): Promise<boolean> {
      try {
         this.busy.show();

         if (!account)
            return false;

         if (account.AccountID == null)
            await this.http.post("api/accounts/insert", account).toPromise();
         else
            await this.http.put("api/accounts/update", account).toPromise();

         this.RefreshAccounts();

         return true;

      }
      catch { return false; /* error absorber */ }
      finally { this.busy.hide(); }
   }

   public async ChangeAccountState(account: AccountEntity, state: boolean) {
      try {
         this.busy.show();

         if (!account)
            return;

         const changeStateVM = {
            AccountID: account.AccountID,
            State: state
         };
         await this.http.put("api/accounts/change-state", changeStateVM).toPromise();

         await this.RefreshAccounts();

      }
      catch { /* error absorber */ }
      finally { this.busy.hide(); }
   }

   public async RemoveAccount(account: AccountEntity) {
      try {

         if (!account)
            return;

         const confirm = await this.message.Confirm("accounts.REMOVE_TEXT", "shared.REMOVE_CONFIRM_COMMAND", "shared.REMOVE_CANCEL_COMMAND");
         if (!confirm)
            return

         this.busy.show();

         await this.http.delete(`api/accounts/delete/${account.AccountID}`).toPromise();

         await this.RefreshAccounts();

      }
      catch { /* error absorber */ }
      finally { this.busy.hide(); }
   }

}
