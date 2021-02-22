import { Component, OnInit } from '@angular/core';
import { IMonthSelectorService } from './month-selector.interface';

@Component({
   selector: 'shared-month-selector',
   templateUrl: './month-selector.component.html',
   styleUrls: ['./month-selector.component.scss']
})
export class MonthSelectorComponent implements OnInit {

   constructor(private service: IMonthSelectorService) { }

   ngOnInit(): void {
   }

   public get CurrentMonth(): Date { return this.service.CurrentMonth.Date; }

   public OnPreviousMonthClick() { this.service.CurrentMonth = this.service.PreviousMonth; }
   public OnNextMonthClick() { this.service.CurrentMonth = this.service.NextMonth; }

}
