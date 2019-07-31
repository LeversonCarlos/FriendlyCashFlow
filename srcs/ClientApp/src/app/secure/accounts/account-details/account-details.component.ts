import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { AccountsService } from '../accounts.service';

@Component({
   selector: 'fs-account-details',
   templateUrl: './account-details.component.html',
   styleUrls: ['./account-details.component.scss']
})
export class AccountDetailsComponent implements OnInit {

   constructor(private service: AccountsService, private route: ActivatedRoute) { }

   public async ngOnInit() {
      const accountID: number = Number(this.route.snapshot.params.id)
      if (!accountID || accountID == 0) { console.error('todo'); return; }

      const account = await this.service.getAccount(accountID);
      if (!account) { console.error('todo'); return; }
      console.log(account);
   }

}
