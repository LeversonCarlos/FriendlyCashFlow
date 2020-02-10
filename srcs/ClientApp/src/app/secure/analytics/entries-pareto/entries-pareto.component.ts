import { Component, OnInit } from '@angular/core';
import { AnalyticsService } from '../analytics.service';
import { Subscription } from 'rxjs';
import { EntriesParetoVM } from '../analytics.viewmodels';
import { EntriesParetoChart } from './entries-pareto.chart';

@Component({
   selector: 'fs-entries-pareto',
   templateUrl: './entries-pareto.component.html',
   styleUrls: ['./entries-pareto.component.scss']
})
export class EntriesParetoComponent implements OnInit {

   constructor(private service: AnalyticsService, private chart: EntriesParetoChart) { }

   public ngOnInit() {
      this.OnDataRefreshedSubscription =
         this.service.OnDataRefreshed.subscribe(x => this.OnDataRefreshed(x));
   }

   private OnDataRefreshedSubscription: Subscription
   private OnDataRefreshed(val: boolean) {
      this.chart.show(this.service.EntriesPareto)
   }

   public ngOnDestroy() {
      if (this.OnDataRefreshedSubscription) {
         this.OnDataRefreshedSubscription.unsubscribe()
         this.OnDataRefreshedSubscription = null;
      }
   }

}
