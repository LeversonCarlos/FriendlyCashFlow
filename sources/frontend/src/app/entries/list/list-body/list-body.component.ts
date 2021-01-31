import { Component, Input, OnInit } from '@angular/core';
import { ResponsiveService } from '@elesse/shared';
import { AccountEntries, EntryEntity } from '../../model/entries.model';

@Component({
   selector: 'entries-list-body',
   templateUrl: './list-body.component.html',
   styleUrls: ['./list-body.component.scss']
})
export class ListBodyComponent implements OnInit {

   constructor(private responsive: ResponsiveService) { }

   @Input()
   public AccountEntries: AccountEntries;
   public get IsMobile(): boolean { return this.responsive && this.responsive.IsMobile; }

   ngOnInit(): void {
   }

   public OnPaidClick(entry: EntryEntity) {
      entry.Paid = !entry.Paid
   }

}
