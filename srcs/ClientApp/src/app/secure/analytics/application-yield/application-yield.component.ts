import { Component, OnDestroy, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { AnalyticsService } from '../analytics.service';

@Component({
   selector: 'fs-application-yield',
   templateUrl: './application-yield.component.html',
   styleUrls: ['./application-yield.component.scss']
})
export class ApplicationYieldComponent implements OnInit, OnDestroy {

   constructor(private service: AnalyticsService) { }

   public ngOnInit(): void {
      this.OnDataRefreshedSubscription =
         this.service.OnDataRefreshed.subscribe(x => this.OnDataRefreshed(x));
   }

   private OnDataRefreshedSubscription: Subscription
   private OnDataRefreshed(val: boolean) {
      // this.chart.show(this.service.ApplicationYield)
      console.log('ApplicationYield', this.service.ApplicationYield);
   }

   public ngOnDestroy(): void {
      throw new Error('Method not implemented.');
   }

}
