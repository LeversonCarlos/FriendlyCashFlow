import { Component, OnDestroy, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { AnalyticsService } from '../analytics.service';

@Component({
   selector: 'fs-patrimony',
   templateUrl: './patrimony.component.html',
   styleUrls: ['./patrimony.component.scss']
})
export class PatrimonyComponent implements OnInit, OnDestroy {

   constructor(private service: AnalyticsService) { }

   ngOnInit(): void {
      this.OnDataRefreshedSubscription =
         this.service.OnDataRefreshed.subscribe(x => this.OnDataRefreshed(x));
   }

   private OnDataRefreshedSubscription: Subscription
   private OnDataRefreshed(val: boolean) {
      // this.chart.show(this.service.Patrimony)
      console.log(this.service.Patrimony);
   }

   public ngOnDestroy() {
      if (this.OnDataRefreshedSubscription) {
         this.OnDataRefreshedSubscription.unsubscribe()
         this.OnDataRefreshedSubscription = null;
      }
   }

}
