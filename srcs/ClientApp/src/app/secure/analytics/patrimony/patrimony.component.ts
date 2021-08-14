import { Component, OnDestroy, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { AnalyticsService } from '../analytics.service';
import { PatrimonyDistributionItem, PatrimonyResumeItem } from '../analytics.viewmodels';
import { PatrimonyDistributionPieChart } from './patrimony-distribution-pie.chart';

@Component({
   selector: 'fs-patrimony',
   templateUrl: './patrimony.component.html',
   styleUrls: ['./patrimony.component.scss']
})
export class PatrimonyComponent implements OnInit, OnDestroy {

   constructor(private service: AnalyticsService, private chart: PatrimonyDistributionPieChart) { }

   ngOnInit(): void {
      this.OnDataRefreshedSubscription =
         this.service.OnDataRefreshed.subscribe(x => this.OnDataRefreshed(x));
   }

   public get Resume(): PatrimonyResumeItem[] { return this.service?.Patrimony?.PatrimonyResume }
   public get Distribution(): PatrimonyDistributionItem[] { return this.service?.Patrimony?.PatrimonyDistribution }

   private OnDataRefreshedSubscription: Subscription
   private OnDataRefreshed(val: boolean) {
      this.chart.show(this.service.Patrimony.PatrimonyDistribution)
   }

   public ngOnDestroy() {
      if (this.OnDataRefreshedSubscription) {
         this.OnDataRefreshedSubscription.unsubscribe()
         this.OnDataRefreshedSubscription = null;
      }
   }

}
