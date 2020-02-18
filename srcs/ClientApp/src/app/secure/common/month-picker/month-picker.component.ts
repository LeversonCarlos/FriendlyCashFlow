import { Component, OnInit, Output, Input, EventEmitter } from '@angular/core';

@Component({
   selector: 'fs-month-picker',
   templateUrl: './month-picker.component.html',
   styleUrls: ['./month-picker.component.scss']
})
export class MonthPickerComponent implements OnInit {

   constructor() { }

   /* INIT */
   public ngOnInit() {
   }

   /* CURRENT MONTH */
   private currentMonth: Date
   public get CurrentMonth(): Date {
      return this.currentMonth;
   }
   @Input() public set CurrentMonth(val: Date) {
      if (!val) { return }
      this.PreviousMonth = new Date(new Date(val).setMonth(val.getMonth() - 1));
      this.currentMonth = val;
      this.NextMonth = new Date(new Date(val).setMonth(val.getMonth() + 1));
   }
   @Output() public CurrentMonthChanged: EventEmitter<Date> = new EventEmitter<Date>()

   /* PREVIOUS MONTH */
   public PreviousMonth: Date;
   public OnPreviousMonthClick() { this.CurrentMonth = this.PreviousMonth; this.CurrentMonthChanged.emit(this.CurrentMonth); }

   /* NEXT MONTH */
   public NextMonth: Date;
   public OnNextMonthClick() { this.CurrentMonth = this.NextMonth; this.CurrentMonthChanged.emit(this.CurrentMonth); }

}
