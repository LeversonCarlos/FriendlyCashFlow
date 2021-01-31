import { Component, OnInit } from '@angular/core';
import { combineLatest, Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { AccountsService, enAccountType } from '@elesse/accounts';
import { AccountEntries } from '../model/entries.model';
import { EntriesService } from '../entries.service';
import { ListService } from './list.service';

@Component({
   selector: 'entries-list',
   templateUrl: './list.component.html',
   styleUrls: ['./list.component.scss']
})
export class ListComponent implements OnInit {

   constructor(private service: EntriesService, private accountsService: AccountsService) { }

   public AccountsEntries: Observable<AccountEntries[]>;
   public HasData: Observable<number>;

   ngOnInit() {
      this.HasData = this.service.ObserveEntries()
         .pipe(
            map(entries => entries.length)
         );
      this.AccountsEntries = combineLatest([this.accountsService.ObserveAccounts(), this.service.ObserveEntries()])
         .pipe(
            map(([accounts, entries]) => ListService.GetEntriesAccounts(accounts, entries))
         );
      this.service.RefreshCache();
   }

   public GetAccountIcon(type: enAccountType): string { return this.accountsService.GetAccountIcon(type); }

}
