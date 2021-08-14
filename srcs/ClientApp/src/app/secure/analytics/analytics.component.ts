import { Component, Input, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { AnalyticsService } from './analytics.service';
import { MediaMatcher } from '@angular/cdk/layout';

@Component({
   selector: 'fs-analytics',
   templateUrl: './analytics.component.html',
   styleUrls: ['./analytics.component.scss']
})
export class AnalyticsComponent implements OnInit {

   constructor(private service: AnalyticsService,
      private route: ActivatedRoute, private media: MediaMatcher) {
      this.mobileQuery = this.media.matchMedia('(max-width: 600px)');
   }
   public mobileQuery: MediaQueryList
   @Input() IsPrinting: boolean

   public ngOnInit() {
      try {

         let year: number = Number(this.route.snapshot.params.year);
         let month: number = Number(this.route.snapshot.params.month);
         this.IsPrinting = Number(this.route.snapshot.params.printing) == 1;

         // DEFAULT YEAR/MONTH
         if (
            (!year || isNaN(year) || year < 1901 || year > 3000) ||
            (!month || isNaN(month) || month < 1 || month > 12)
         ) {
            const date = new Date();
            year = date.getFullYear();
            month = date.getMonth() + 1;
            this.service.showIndex(year, month, this.IsPrinting);
            return;
         }

         const currentMonth = new Date(`${year.toString().padStart(4, "20")}-${(month + 0).toString().padStart(2, "0")}-01 12:00:00`);
         this.service.setFilter(currentMonth, this.IsPrinting);

      }
      catch (ex) { console.error(ex); }
   }

   public OnPrintVersionChanged(isPrinting: boolean) {
      this.service.setFilter(this.service.FilterData.Month, isPrinting);
      /*
      // this isnt working as the charts isnt redraw
      const date = this.service.FilterData.Month;
      const year = date.getFullYear();
      const month = date.getMonth() + 1;
      let url = `/analytics/${year}/${month}`
      if (isPrinting)
         url += "/1"
      this.location.replaceState(url);
      window.dispatchEvent(new Event('resize'));
      Highcharts.charts.forEach(chart => chart.reflow());
      */
   }

}
