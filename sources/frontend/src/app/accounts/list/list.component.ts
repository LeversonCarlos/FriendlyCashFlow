import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { AccountEntity, enAccountType } from '../accounts.data';
import { AccountsService } from '../accounts.service';

@Component({
   selector: 'accounts-list',
   templateUrl: './list.component.html',
   styleUrls: ['./list.component.scss']
})
export class ListComponent implements OnInit {

   constructor(private service: AccountsService) { }

   public get ActiveAccounts(): Observable<AccountEntity[]> { return this.service.GetData(true); }
   public get InactiveAccounts(): Observable<AccountEntity[]> { return this.service.GetData(false); }
   public getAccountIcon(type: enAccountType): string { return this.service.getAccountIcon(type); }

   ngOnInit(): void {
      this.service.RefreshData();
   }

   public AddClick(): void {
      console.log('add clicked')
   }

}
