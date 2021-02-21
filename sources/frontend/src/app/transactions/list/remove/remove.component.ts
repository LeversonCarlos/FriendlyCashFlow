import { Component, Input, OnInit } from '@angular/core';
import { EntriesData } from '@elesse/entries';
import { TransactionBase, TransactionEntry } from '../../model/transactions.model';

@Component({
   selector: 'transactions-remove',
   templateUrl: './remove.component.html',
   styleUrls: ['./remove.component.scss']
})
export class RemoveComponent implements OnInit {

   constructor(private entriesData: EntriesData) { }

   @Input() Transaction: TransactionBase

   ngOnInit(): void {
   }

   public OnRemoveClick() {
      if (!this.Transaction)
         return;
      if (this.Transaction instanceof TransactionEntry)
         this.entriesData.RemoveEntry(this.Transaction.Entry);
   }

}
