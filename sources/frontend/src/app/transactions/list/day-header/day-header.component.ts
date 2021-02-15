import { Component, Input, OnInit } from '@angular/core';
import { TransactionDay } from '../../model/transactions.model';

@Component({
   selector: 'transactions-day-header',
   templateUrl: './day-header.component.html',
   styleUrls: ['./day-header.component.scss']
})
export class DayHeaderComponent implements OnInit {

   constructor() { }

   ngOnInit(): void {
   }

   @Input() Day: TransactionDay;

}
