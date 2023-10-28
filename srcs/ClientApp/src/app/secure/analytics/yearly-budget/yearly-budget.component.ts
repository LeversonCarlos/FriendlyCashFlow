import { Component, OnDestroy, OnInit } from '@angular/core';
import { AnalyticsService } from '../analytics.service';
import { Subscription } from 'rxjs';
import { YearlyBudgetChart } from './yearly-budget.chart';

@Component({
   selector: 'fs-yearly-budget',
   templateUrl: './yearly-budget.component.html',
   styleUrls: ['./yearly-budget.component.scss']
})
export class YearlyBudgetComponent implements OnInit, OnDestroy {

   constructor(
      private service: AnalyticsService,
      private chart: YearlyBudgetChart,
   ) { }

   ngOnInit(): void {
      this.OnDataRefreshedSubscription =
         this.service.OnDataRefreshed.subscribe(x => this.OnDataRefreshed(x));
   }

   private OnDataRefreshedSubscription: Subscription
   private OnDataRefreshed(val: boolean) {
      this.chart.show(this.service.YearlyBudget)
   }

   ngOnDestroy(): void {
      this.OnDataRefreshedSubscription?.unsubscribe()
      this.OnDataRefreshedSubscription = null;
   }

}
