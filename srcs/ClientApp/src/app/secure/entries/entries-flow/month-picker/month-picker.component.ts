import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { EntriesService } from '../../entries.service';

@Component({
   selector: 'fs-month-picker',
   templateUrl: './month-picker.component.html',
   styleUrls: ['./month-picker.component.scss']
})
export class MonthPickerComponent implements OnInit {

   constructor(private service: EntriesService, private route: ActivatedRoute) { }

   public ngOnInit() {
      try {
         let year: number = Number(this.route.snapshot.params.year);
         let month: number = Number(this.route.snapshot.params.month);

         if (
            (!year || isNaN(year) || year < 1901 || year > 3000) ||
            (!month || isNaN(month) || month < 1 || month > 12)
         ) {
            const date = new Date();
            year = date.getFullYear();
            month = date.getMonth() + 1;
            this.service.showFlow(year, month);
            return;
         }

         this.CurrentMonth = new Date(`${year.toString().padStart(4, "20")}-${(month + 0).toString().padStart(2, "0")}-01 12:00:00`);
      }
      catch{ return; }
   }

   public get CurrentMonth(): Date {
      return this.service.CurrentMonth;
   }
   public set CurrentMonth(val: Date) {
      this.PreviousMonth = new Date(new Date(val).setMonth(val.getMonth() - 1));
      this.service.CurrentMonth = val;
      this.NextMonth = new Date(new Date(val).setMonth(val.getMonth() + 1));
      this.service.loadFlowList().then(() => this.service.showFlow(val.getFullYear(), val.getMonth() + 1))
   }

   public PreviousMonth: Date;
   public OnPreviousMonthClick() { this.CurrentMonth = this.PreviousMonth; }
   public NextMonth: Date;
   public OnNextMonthClick() { this.CurrentMonth = this.NextMonth; }

}
