import { Component, OnInit } from '@angular/core';
import { AccountsService, Account } from './accounts.service';

@Component({
   selector: 'fs-accounts',
   templateUrl: './accounts.component.html',
   styleUrls: ['./accounts.component.scss']
})
export class AccountsComponent implements OnInit {

   constructor(private service: AccountsService) { }
   public Data: Account[];

   public async ngOnInit() {
      this.Data = await this.service.getAccounts();
   }

}
