import { Component, Input, OnInit } from '@angular/core';
import { TransactionDay } from '../../model/transactions.model';

@Component({
   selector: 'transactions-day-footer',
   templateUrl: './day-footer.component.html',
   styleUrls: ['./day-footer.component.scss']
})
export class DayFooterComponent implements OnInit {

   constructor() { }

   ngOnInit(): void {
   }

   @Input() Day: TransactionDay;

}
