import { Component, Input, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { AccountEntity, enAccountType } from '../../model/accounts.model';
import { AccountsData } from '../../data/accounts.data';

@Component({
   selector: 'accounts-list-body',
   templateUrl: './list-body.component.html',
   styleUrls: ['./list-body.component.scss']
})
export class ListBodyComponent implements OnInit {

   constructor(private accountsData: AccountsData) { }

   @Input()
   public Accounts: Observable<AccountEntity[]>

   public GetAccountIcon(type: enAccountType): string { return AccountsData.GetAccountIcon(type); }

   ngOnInit(): void {
   }

   public OnEnableAccount(account: AccountEntity) {
      this.accountsData.ChangeAccountState(account, true);
   }

   public OnDisableAccount(account: AccountEntity) {
      this.accountsData.ChangeAccountState(account, false);
   }

   public OnRemoveAccount(account: AccountEntity) {
      this.accountsData.RemoveAccount(account);
   }

}
