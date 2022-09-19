import { Injectable } from '@angular/core';
import { AccountModel } from '@models/accounts';
import { BehaviorSubject } from 'rxjs';

@Injectable()
export class AccountsState {

   constructor() { }

   private _SearchTerms: string = "";
   public get SearchTerms(): string {
      return this._SearchTerms;
   }
   public set SearchTerms(value: string) {
      this._SearchTerms = value;
      this.SearchTermsSubject.next(value);
   }
   public SearchTermsSubject: BehaviorSubject<string> = new BehaviorSubject("");


   public Accounts: AccountModel[] = [];
   public Account: AccountModel | null = null;


}
