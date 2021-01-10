import { Injectable } from '@angular/core';
import { AccountEntity } from './accounts.data';
import { Observable, Subject } from 'rxjs';
import { StorageService } from '@elesse/shared';

@Injectable({
   providedIn: 'root'
})
export class AccountsService {

   constructor() {
      this._Data = new StorageService<boolean, AccountEntity[]>("AccountsService");
      this._Data.InitializeValues(false, true);
   }

   private _Data: StorageService<boolean, AccountEntity[]>;
   public GetData = (state: boolean = true): Observable<AccountEntity[]> => this._Data.GetValue(state);

   public SetData(state: boolean, value: AccountEntity[]): void {
      this._Data.SetValue(state, value);
   }

}
