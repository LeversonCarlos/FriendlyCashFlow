import { Component, OnInit } from '@angular/core';
import { AccountsService, Account } from './accounts.service';
import { Router } from '@angular/router';

@Component({
   selector: 'fs-accounts',
   templateUrl: './accounts.component.html',
   styleUrls: ['./accounts.component.scss']
})
export class AccountsComponent implements OnInit {

   constructor(private service: AccountsService, private router: Router) { }

   public Data: Account[];
   public get ActiveData(): Account[] {
      return this.Data && this.Data.filter(item => item.Active);
   }
   public get ArquivedData(): Account[] {
      return this.Data && this.Data.filter(item => !item.Active);
   }

   public async ngOnInit() {
      this.Data = await this.service.getAccounts();
   }

   public OnItemSelect(item: Account) {
      this.router.navigate(['/account', item.AccountID]);
   }

}
