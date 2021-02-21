import { Component, Input, OnInit } from '@angular/core';
import { TransactionBase } from '../../model/transactions.model';

@Component({
   selector: 'transactions-remove',
   templateUrl: './remove.component.html',
   styleUrls: ['./remove.component.scss']
})
export class RemoveComponent implements OnInit {

   constructor() { }

   @Input() Transaction: TransactionBase

   ngOnInit(): void {
   }

   public OnRemoveClick() {
      console.log('OnRemoveClick', this.Transaction)
   }

}
