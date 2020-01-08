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
         let account: number = Number(this.route.snapshot.params.account);

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

         this.service.CurrentData.CurrentAccount = account;
         this.SetCurrentMonth(new Date(`${year.toString().padStart(4, "20")}-${(month + 0).toString().padStart(2, "0")}-01 12:00:00`));
      }
      catch{ return; }
   }

   public get CurrentMonth(): Date {
      return this.service.CurrentData.CurrentMonth;
   }
   public async SetCurrentMonth(val: Date) {
      this.PreviousMonth = new Date(new Date(val).setMonth(val.getMonth() - 1));
      this.service.CurrentData.setFlow(val, this.service.CurrentData.CurrentAccount);
      this.NextMonth = new Date(new Date(val).setMonth(val.getMonth() + 1));

      const currentYear = this.service.CurrentData.CurrentMonth.getFullYear()
      const currentMonth = this.service.CurrentData.CurrentMonth.getMonth() + 1
      const currentAccount = this.service.CurrentData.CurrentAccount || 0
      await this.service.loadFlowList(currentYear, currentMonth, currentAccount);

      this.service.showCurrentList()
   }

   public PreviousMonth: Date;
   public OnPreviousMonthClick() { this.SetCurrentMonth(this.PreviousMonth); }
   public NextMonth: Date;
   public OnNextMonthClick() { this.SetCurrentMonth(this.NextMonth); }

}
