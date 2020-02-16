import { Component, OnInit, OnDestroy } from '@angular/core';
import { AnalyticsService } from '../analytics.service';
import { Subscription } from 'rxjs';

@Component({
   selector: 'fs-monthly-target',
   templateUrl: './monthly-target.component.html',
   styleUrls: ['./monthly-target.component.scss']
})
export class MonthlyTargetComponent implements OnInit, OnDestroy {

   constructor(private service: AnalyticsService) { }

   public ngOnInit() {
      this.OnDataRefreshedSubscription =
         this.service.OnDataRefreshed.subscribe(x => this.OnDataRefreshed(x));
   }

   private OnDataRefreshedSubscription: Subscription
   private OnDataRefreshed(val: boolean) {
      // this.chart.show(this.service.CategoryGoals)
      console.log(this.service.MonthlyTarget)
   }

   public ngOnDestroy() {
      if (this.OnDataRefreshedSubscription) {
         this.OnDataRefreshedSubscription.unsubscribe()
         this.OnDataRefreshedSubscription = null;
      }
   }

}
