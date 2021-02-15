import { Component, OnInit } from '@angular/core';
import { combineLatest, Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { AccountsData, enAccountType } from '@elesse/accounts';
import { AccountEntries } from '../model/entries.model';
import { EntriesData } from '../data/entries.data';
import { ListService } from './list.service';

@Component({
   selector: 'entries-list',
   templateUrl: './list.component.html',
   styleUrls: ['./list.component.scss']
})
export class ListComponent implements OnInit {

   constructor(private entriesData: EntriesData, private accountsData: AccountsData) { }

   public AccountsEntries: Observable<AccountEntries[]>;

   ngOnInit() {
      this.AccountsEntries = combineLatest([this.accountsData.ObserveAccounts(), this.entriesData.ObserveEntries()])
         .pipe(
            map(([accounts, entries]) => ListService.GetEntriesAccounts(accounts, entries))
         );
   }

   public GetAccountIcon(type: enAccountType): string { return AccountsData.GetAccountIcon(type); }

}
