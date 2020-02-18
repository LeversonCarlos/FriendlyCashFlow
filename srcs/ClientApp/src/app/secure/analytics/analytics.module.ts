import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from 'src/app/shared/shared.module';
import { CommonSecureModule } from '../common/common.module';

import { AnalyticsRouting } from './analytics.routing';
import { AnalyticsService } from './analytics.service';
import { AnalyticsFiltersComponent } from './analytics-filters/analytics-filters.component';
import { CategoryGoalsComponent } from './category-goals/category-goals.component';
import { EntriesParetoComponent } from './entries-pareto/entries-pareto.component';
import { MonthlyTargetComponent } from './monthly-target/monthly-target.component';
import { AnalyticsComponent } from './analytics.component';


@NgModule({
   declarations: [AnalyticsComponent, AnalyticsFiltersComponent,
      CategoryGoalsComponent, EntriesParetoComponent, MonthlyTargetComponent],
   imports: [
      CommonModule, SharedModule, CommonSecureModule,
      AnalyticsRouting
   ],
   providers: [AnalyticsService]
})
export class AnalyticsModule { }
