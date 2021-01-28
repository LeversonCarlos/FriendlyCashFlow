import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { AccountEntries } from '../entries.data';
import { EntriesService } from '../entries.service';
import { ListService } from './list.service';

@Component({
   selector: 'entries-list',
   templateUrl: './list.component.html',
   styleUrls: ['./list.component.scss']
})
export class ListComponent implements OnInit {

   constructor(private service: EntriesService) { }

   public AccountsEntries: Observable<AccountEntries[]>;
   public HasData: Observable<number>;

   ngOnInit() {
      this.HasData = this.service.ObserveEntries()
         .pipe(
            map(entries => entries.length)
         );
      this.AccountsEntries = this.service.ObserveEntries()
         .pipe(
            map(ListService.GetEntriesAccounts)
         );
      this.service.RefreshCache();
   }

}
