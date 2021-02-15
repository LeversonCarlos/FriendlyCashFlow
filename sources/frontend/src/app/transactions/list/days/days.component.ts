import { Component, Input, OnInit } from '@angular/core';
import { ResponsiveService } from '@elesse/shared';
import { TransactionDay } from '../../model/transactions.model';

@Component({
   selector: 'transactions-days',
   templateUrl: './days.component.html',
   styleUrls: ['./days.component.scss']
})
export class DaysComponent implements OnInit {

   constructor(private responsive: ResponsiveService) { }

   ngOnInit(): void {
   }

   @Input() Days: TransactionDay[]
   public get IsMobile(): boolean { return this.responsive && this.responsive.IsMobile; }

}
