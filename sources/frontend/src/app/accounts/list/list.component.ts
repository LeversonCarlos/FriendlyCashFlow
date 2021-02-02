import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { AccountEntity } from '../model/accounts.model';
import { AccountsData } from '../data/accounts.data';

@Component({
   selector: 'accounts-list',
   templateUrl: './list.component.html',
   styleUrls: ['./list.component.scss']
})
export class ListComponent implements OnInit {

   constructor(private accountsData: AccountsData) { }

   public ActiveAccounts: Observable<AccountEntity[]>;
   public InactiveAccounts: Observable<AccountEntity[]>;

   ngOnInit(): void {
      this.ActiveAccounts = this.accountsData.ObserveAccounts(true);
      this.InactiveAccounts = this.accountsData.ObserveAccounts(false);
      this.accountsData.RefreshAccounts();
   }

}
