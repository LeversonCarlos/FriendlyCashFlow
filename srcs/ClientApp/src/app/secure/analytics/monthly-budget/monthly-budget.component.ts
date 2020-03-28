import { Component, OnInit, OnDestroy } from '@angular/core';
import { AnalyticsService } from '../analytics.service';
import { Subscription } from 'rxjs';
import { MonthlyBudgetChart } from './monthly-budget.chart';

@Component({
   selector: 'fs-monthly-budget',
   templateUrl: './monthly-budget.component.html',
   styleUrls: ['./monthly-budget.component.scss']
})
export class MonthlyBudgetComponent implements OnInit, OnDestroy {

   constructor(private service: AnalyticsService, private chart: MonthlyBudgetChart) { }

   public ngOnInit() {
      this.OnDataRefreshedSubscription =
         this.service.OnDataRefreshed.subscribe(x => this.OnDataRefreshed(x));
   }

   private OnDataRefreshedSubscription: Subscription
   private OnDataRefreshed(val: boolean) {
      this.chart.show(this.service.MonthlyBudget)
   }

   public ngOnDestroy() {
      if (this.OnDataRefreshedSubscription) {
         this.OnDataRefreshedSubscription.unsubscribe()
         this.OnDataRefreshedSubscription = null;
      }
   }

}
