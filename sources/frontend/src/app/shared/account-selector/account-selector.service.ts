import { Injectable } from '@angular/core';
import { IAccountSelectorService } from './account-selector.interface'

@Injectable({
   providedIn: 'root'
})
export class AccountSelectorService implements IAccountSelectorService {

   private _TabIndex: number = 0;
   public get TabIndex(): number { return this._TabIndex; }

   private _AccountID: string = '';
   public get AccountID(): string { return this._AccountID; }

   public SetTab(tabIndex: number, accountID: string) {
      this._TabIndex = tabIndex;
      this._AccountID = accountID;
   }

}
