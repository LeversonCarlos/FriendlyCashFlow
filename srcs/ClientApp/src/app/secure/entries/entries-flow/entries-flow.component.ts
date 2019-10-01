import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { EntriesService } from '../entries.service';

@Component({
   selector: 'fs-entries-flow',
   templateUrl: './entries-flow.component.html',
   styleUrls: ['./entries-flow.component.scss']
})
export class EntriesFlowComponent implements OnInit {

   constructor(private service: EntriesService, private route: ActivatedRoute) { }

   public get CurrentMonth(): Date {
      try {
         const year: number = Number(this.route.snapshot.params.year);
         const month: number = Number(this.route.snapshot.params.month);
         if (!year || isNaN(year) || year < 1901 || year > 3000) { return null; }
         if (!month || isNaN(month) || month < 1 || month > 12) { return null; }
         return new Date(`${year.toString().padStart(4, "20")}-${(month + 0).toString().padStart(2, "0")}-01 12:00:00`);
      }
      catch{ return null; }
   };

   public get PreviousMonth(): Date {
      if (!this.CurrentMonth) { return null }
      else { return new Date(this.CurrentMonth.setMonth(this.CurrentMonth.getMonth() - 1)); }
   }
   public OnPreviousMonthClick() {
      this.service.showFlow(this.PreviousMonth.getFullYear(), this.PreviousMonth.getMonth() + 1);
   }

   public get NextMonth(): Date {
      if (!this.CurrentMonth) { return null }
      else { return new Date(this.CurrentMonth.setMonth(this.CurrentMonth.getMonth() + 1)); }
   }
   public OnNextMonthClick() {
      this.service.showFlow(this.NextMonth.getFullYear(), this.NextMonth.getMonth() + 1);
   }

   public ngOnInit() {
      if (this.CurrentMonth == null) {
         const currentMonth = new Date();
         this.service.showFlow(currentMonth.getFullYear(), currentMonth.getMonth() + 1);
         return;
      }
   }

}
