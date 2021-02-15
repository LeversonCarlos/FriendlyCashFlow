import { Component, Input, OnInit } from '@angular/core';
import { TransactionBase, TransactionEntry } from '../../model/transactions.model';

@Component({
   selector: 'transactions-item',
   templateUrl: './item.component.html',
   styleUrls: ['./item.component.scss']
})
export class ItemComponent implements OnInit {

   constructor() { }

   ngOnInit(): void {
   }

   @Input() Transaction: TransactionBase

   public OnPaidClick() {
      this.Transaction.Paid = !this.Transaction.Paid
      console.log('OnPaidClick', 'TODO !')
   }

   public OnRemoveClick() {
      console.log('OnRemoveClick', 'TODO !')
   }

   public get DetailsRouterLink(): string {
      if (this.Transaction instanceof TransactionEntry)
         return `/entries/details/${this.Transaction.Entry.EntryID}`;
      else
         return null;
   }

}