import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { AccountEntity } from '../accounts.data';
import { AccountsService } from '../accounts.service';

@Component({
   selector: 'accounts-list',
   templateUrl: './list.component.html',
   styleUrls: ['./list.component.scss']
})
export class ListComponent implements OnInit {

   constructor(private service: AccountsService) { }

   public get ActiveAccounts(): Observable<AccountEntity[]> { return this.service.ObserveAccounts(true); }
   public get InactiveAccounts(): Observable<AccountEntity[]> { return this.service.ObserveAccounts(false); }

   ngOnInit(): void {
      this.service.RefreshCache();
   }

}
