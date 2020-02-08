import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { AnalyticsService } from './analytics.service';

@Component({
   selector: 'fs-analytics',
   templateUrl: './analytics.component.html',
   styleUrls: ['./analytics.component.scss']
})
export class AnalyticsComponent implements OnInit {

   constructor(private service: AnalyticsService, private route: ActivatedRoute) { }

   public ngOnInit() {
      try {

         let year: number = Number(this.route.snapshot.params.year);
         let month: number = Number(this.route.snapshot.params.month);

         // DEFAULT YEAR/MONTH
         if (
            (!year || isNaN(year) || year < 1901 || year > 3000) ||
            (!month || isNaN(month) || month < 1 || month > 12)
         ) {
            const date = new Date();
            year = date.getFullYear();
            month = date.getMonth() + 1;
            this.service.showIndex(year, month);
            return;
         }

         const currentMonth = new Date(`${year.toString().padStart(4, "20")}-${(month + 0).toString().padStart(2, "0")}-01 12:00:00`);
         this.service.setFilter(currentMonth);

      }
      catch (ex) { console.error(ex); }
   }


}
