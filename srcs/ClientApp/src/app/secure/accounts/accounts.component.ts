import { Component, OnInit } from '@angular/core';
import { AccountsService, Account } from './accounts.service';

@Component({
   selector: 'fs-accounts',
   templateUrl: './accounts.component.html',
   styleUrls: ['./accounts.component.scss']
})
export class AccountsComponent implements OnInit {

   constructor(private service: AccountsService) { }

   public async ngOnInit() {
      this.Data = await this.service.getAccounts();
   }

   public Data: Account[];
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
