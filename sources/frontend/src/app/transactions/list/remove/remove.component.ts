import { Component, Input, OnInit } from '@angular/core';
import { EntriesData } from '@elesse/entries';
import { TransfersData } from '@elesse/transfers';
import { TransactionBase, TransactionEntry, TransactionTransfer } from '../../model/transactions.model';

@Component({
   selector: 'transactions-remove',
   templateUrl: './remove.component.html',
   styleUrls: ['./remove.component.scss']
})
export class RemoveComponent implements OnInit {

   constructor(private entriesData: EntriesData, private transfersData: TransfersData) { }

   @Input() Transaction: TransactionBase

   ngOnInit(): void {
   }

   public OnRemoveClick() {
      if (!this.Transaction)
         return;
      if (this.Transaction instanceof TransactionEntry) {
         this.entriesData.RemoveEntry(this.Transaction.Entry);
      }
      if (this.Transaction instanceof TransactionTransfer) {
         this.transfersData.RemoveTransfer(this.Transaction.Transfer);
      }
   }

}
