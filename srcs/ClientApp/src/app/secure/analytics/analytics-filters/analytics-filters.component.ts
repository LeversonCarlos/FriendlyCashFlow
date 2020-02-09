import { Component, OnInit } from '@angular/core';
import { AnalyticsService } from '../analytics.service';

@Component({
   selector: 'fs-analytics-filters',
   templateUrl: './analytics-filters.component.html',
   styleUrls: ['./analytics-filters.component.scss']
})
export class AnalyticsFiltersComponent implements OnInit {

   constructor(private service: AnalyticsService) { }

   ngOnInit() {
   }

   /* CURRENT MONTH */
   public get CurrentMonth(): Date {
      return this.service.FilterData.Month;
   }
   public set CurrentMonth(val: Date) {
      this.service.setFilter(val)
   }

}
