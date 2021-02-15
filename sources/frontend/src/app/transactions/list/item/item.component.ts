import { Component, Input, OnInit } from '@angular/core';
import { TransactionBase } from '../../model/transactions.model';

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

}
