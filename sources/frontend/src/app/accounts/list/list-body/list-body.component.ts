import { Component, Input, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { AccountEntity, enAccountType } from '../../accounts.data';
import { AccountsService } from '../../accounts.service';

@Component({
   selector: 'accounts-list-body',
   templateUrl: './list-body.component.html',
   styleUrls: ['./list-body.component.scss']
})
export class ListBodyComponent implements OnInit {

   constructor(private service: AccountsService) { }

   @Input()
   public Accounts: Observable<AccountEntity[]>

   public GetAccountIcon(type: enAccountType): string { return this.service.GetAccountIcon(type); }

   ngOnInit(): void {
   }

   public OnEnableAccount(account: AccountEntity) {
      this.service.ChangeAccountState(account, true);
   }

   public OnDisableAccount(account: AccountEntity) {
      this.service.ChangeAccountState(account, false);
   }

   public OnRemoveAccount(account: AccountEntity) {
      this.service.RemoveAccount(account);
   }

}
