import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { AccountEntity } from '../model/accounts.model';
import { AccountsService } from '../accounts.service';

@Component({
   selector: 'accounts-list',
   templateUrl: './list.component.html',
   styleUrls: ['./list.component.scss']
})
export class ListComponent implements OnInit {

   constructor(private service: AccountsService) { }

   public ActiveAccounts: Observable<AccountEntity[]>;
   public InactiveAccounts: Observable<AccountEntity[]>;

   ngOnInit(): void {
      this.ActiveAccounts = this.service.ObserveAccounts(true);
      this.InactiveAccounts = this.service.ObserveAccounts(false);
      this.service.RefreshAccounts();
   }

}
