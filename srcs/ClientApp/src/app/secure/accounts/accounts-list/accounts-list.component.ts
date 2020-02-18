import { Component, OnInit } from '@angular/core';
import { AccountsService } from '../accounts.service';
import { Account } from '../accounts.viewmodels'

@Component({
   selector: 'fs-accounts-list',
   templateUrl: './accounts-list.component.html',
   styleUrls: ['./accounts-list.component.scss']
})
export class AccountsListComponent implements OnInit {

   constructor(private service: AccountsService) { }
   public Data: Account[];

   public async ngOnInit() {
      this.Data = await this.service.getAccounts();
   }

   public get ActiveData(): Account[] {
      return this.Data && this.Data.filter(item => item.Active);
   }
   public get ArquivedData(): Account[] {
      return this.Data && this.Data.filter(item => !item.Active);
   }

   public OnItemClick(item: Account) {
      this.service.showDetails(item.AccountID);
   }

   public OnNewClick() {
      this.service.showNew();
   }

}
