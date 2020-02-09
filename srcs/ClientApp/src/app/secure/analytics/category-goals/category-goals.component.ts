import { Component, OnInit, OnDestroy } from '@angular/core';
import { AnalyticsService } from '../analytics.service';
import { CategoryGoalsVM } from '../analytics.viewmodels';
import { CategoryGoalsChart } from './category-goals.chart';
import { Subscription } from 'rxjs';

@Component({
   selector: 'fs-category-goals',
   templateUrl: './category-goals.component.html',
   styleUrls: ['./category-goals.component.scss']
})
export class CategoryGoalsComponent implements OnInit, OnDestroy {

   constructor(private service: AnalyticsService, private chart: CategoryGoalsChart) { }

   public ngOnInit() {
      this.OnDataRefreshedSubscription =
         this.service.OnDataRefreshed.subscribe(x => this.OnDataRefreshed(x));
   }

   private OnDataRefreshedSubscription: Subscription
   private OnDataRefreshed(val: boolean) {
      this.chart.show(this.service.CategoryGoals)
   }

   public ngOnDestroy() {
      if (this.OnDataRefreshedSubscription) {
         this.OnDataRefreshedSubscription.unsubscribe()
         this.OnDataRefreshedSubscription = null;
      }
   }

}
