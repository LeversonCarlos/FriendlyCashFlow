import { Component, OnDestroy, OnInit } from '@angular/core';
import { AnalyticsService } from '../../analytics.service';
import { Subscription } from 'rxjs';

@Component({
   selector: 'fs-yearly-budget-totals',
   templateUrl: './totals.component.html',
   styleUrls: ['./totals.component.scss']
})
export class YearlyBudgetTotalsComponent implements OnInit, OnDestroy {

   constructor(
      private service: AnalyticsService,
   ) { }

   public TotalBudget: number | null = null;
   public TotalMonth: number | null = null;
   public TotalUndefined: number | null = null;

   ngOnInit(): void {
      this.OnDataRefreshedSubscription =
         this.service.OnDataRefreshed.subscribe(x => this.OnDataRefreshed(x));
   }

   private OnDataRefreshedSubscription: Subscription
   private OnDataRefreshed(val: boolean) {
      this.TotalBudget = this.service.YearlyBudget
         ?.filter(x => x.BudgetValue)
         ?.map(x => x.BudgetValue)
         ?.reduce((p, c) => p + c, 0);
      this.TotalMonth = this.service.YearlyBudget
         ?.filter(x => x.BudgetValue > 0)
         ?.map(x => x.MonthValue)
         ?.reduce((p, c) => p + c, 0);
      this.TotalUndefined = this.service.YearlyBudget
         ?.filter(x => x.BudgetValue == undefined || x.BudgetValue == null || x.BudgetValue == 0)
         ?.map(x => x.MonthValue)
         ?.reduce((p, c) => p + c, 0);
   }

   ngOnDestroy(): void {
      this.OnDataRefreshedSubscription?.unsubscribe()
      this.OnDataRefreshedSubscription = null;
   }

}
