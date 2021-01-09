import { Injectable } from '@angular/core';
import { AccountEntity } from './accounts.data';

@Injectable({
   providedIn: 'root'
})
export class AccountsService {

   constructor() { }

   private _Data: { [id: number]: AccountEntity[]; } = {};
   private _DataKey = (state: boolean): number =>
      state == true ? 1 : 0;
   public GetData = (state: boolean = true): AccountEntity[] =>
      this._Data[this._DataKey(state)] ?? [];

}
