import { Injectable } from '@angular/core';
import { AccountEntity } from './accounts.data';
import { Observable, Subject } from 'rxjs';

@Injectable({
   providedIn: 'root'
})
export class AccountsService {

   constructor() { }

   private _Data: { [id: number]: Subject<AccountEntity[]>; } = {
      0: new Subject<AccountEntity[]>(),
      1: new Subject<AccountEntity[]>()
   };
   private _DataKey = (state: boolean): number =>
      state == true ? 1 : 0;

   public GetData(state: boolean = true): Observable<AccountEntity[]> {
      return this._Data[this._DataKey(state)];
   }
   public SetData(state: boolean, value: AccountEntity[]): void {
      this._Data[this._DataKey(state)].next(value);
   }

}
