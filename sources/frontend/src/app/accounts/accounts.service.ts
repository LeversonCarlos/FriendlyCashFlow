import { Injectable } from '@angular/core';
import { AccountEntity, AccountType, enAccountType } from './accounts.data';
import { Observable } from 'rxjs';
import { LocalizationService, MessageService, StorageService } from '@elesse/shared';
import { HttpClient } from '@angular/common/http';

@Injectable({
   providedIn: 'root'
})
export class AccountsService {

   constructor(private localization: LocalizationService, private messsage: MessageService,
      private http: HttpClient) {
      this.Cache = new StorageService<boolean, AccountEntity[]>("AccountsService");
      this.Cache.InitializeValues(false, true);
   }

   private Cache: StorageService<boolean, AccountEntity[]>;

   public async RefreshCache(): Promise<void> {
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
            this.Cache.SetValue(key, value);
         });

      }
      catch { /* error absorber */ }
   }

   public ObserveAccounts = (state: boolean = true): Observable<AccountEntity[]> =>
      this.Cache.GetObservable(state);

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

   public async SaveAccount(account: AccountEntity): Promise<boolean> {
      try {

         if (!account)
            return false;

         if (account.AccountID == null)
            await this.http.post("api/accounts/insert", account).toPromise();
         else
            await this.http.put("api/accounts/update", account).toPromise();

         return true;

      }
      catch { return false;/* error absorber */ }
   }

   public async ChangeAccountState(account: AccountEntity, state: boolean) {
      try {

         if (!account)
            return;

         const changeStateVM = {
            AccountID: account.AccountID,
            State: state
         };
         await this.http.put("api/accounts/change-state", changeStateVM).toPromise();

         await this.RefreshCache();

      }
      catch { /* error absorber */ }
   }

   public async RemoveAccount(account: AccountEntity) {
      try {

         if (!account)
            return;

         const confirm = await this.messsage.Confirm("accounts.REMOVE_TEXT", "shared.REMOVE_CONFIRM_COMMAND", "shared.REMOVE_CANCEL_COMMAND");
         if (!confirm)
            return

         await this.http.delete(`api/accounts/delete/${account.AccountID}`).toPromise();

         await this.RefreshCache();

      }
      catch { /* error absorber */ }
   }

}
