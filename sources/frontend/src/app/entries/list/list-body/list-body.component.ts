import { Component, Input, OnInit } from '@angular/core';
import { AccountEntries, EntryEntity } from '../../entries.data';

@Component({
   selector: 'entries-list-body',
   templateUrl: './list-body.component.html',
   styleUrls: ['./list-body.component.scss']
})
export class ListBodyComponent implements OnInit {

   constructor() { }

   @Input()
   public AccountEntries: AccountEntries;

   ngOnInit(): void {
   }

   public OnPaidClick(entry: EntryEntity) {
      entry.Paid = !entry.Paid
   }

}
