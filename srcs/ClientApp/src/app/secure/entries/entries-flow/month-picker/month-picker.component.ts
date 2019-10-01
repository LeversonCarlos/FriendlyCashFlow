import { Component, OnInit, EventEmitter, Output } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

@Component({
   selector: 'fs-month-picker',
   templateUrl: './month-picker.component.html',
   styleUrls: ['./month-picker.component.scss']
})
export class MonthPickerComponent implements OnInit {

   constructor(private route: ActivatedRoute) { }

   public ngOnInit() {
      try {
         const year: number = Number(this.route.snapshot.params.year);
         const month: number = Number(this.route.snapshot.params.month);

         if (!year || isNaN(year) || year < 1901 || year > 3000) { this.CurrentMonth = new Date(); return; }
         if (!month || isNaN(month) || month < 1 || month > 12) { this.CurrentMonth = new Date(); return; }

         this.CurrentMonth = new Date(`${year.toString().padStart(4, "20")}-${(month + 0).toString().padStart(2, "0")}-01 12:00:00`);
      }
      catch{ this.CurrentMonth = new Date(); return; }
   }

   private currentMonth: Date;
   public get CurrentMonth(): Date {
      return this.currentMonth;
   }
   public set CurrentMonth(val: Date) {
      this.PreviousMonth = new Date(new Date(val).setMonth(val.getMonth() - 1));
      this.currentMonth = val;
      this.NextMonth = new Date(new Date(val).setMonth(val.getMonth() + 1));
      this.Changed.emit(this.currentMonth);
   }

   public PreviousMonth: Date;
   public OnPreviousMonthClick() { this.CurrentMonth = this.PreviousMonth; }
   public NextMonth: Date;
   public OnNextMonthClick() { this.CurrentMonth = this.NextMonth; }

   @Output()
   public Changed: EventEmitter<Date> = new EventEmitter<Date>();

}
