import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from 'src/app/shared/shared.module';

import { DashboardRouting } from './dashboard.routing';
import { DashboardService } from './dashboard.service';
import { DashboardComponent } from './dashboard.component';
import { ResumeComponent } from './resume/resume.component';
import { EntriesComponent } from './entries/entries.component';
import { BalanceComponent } from './balance/balance.component';
import { CommonSecureModule } from '../common/common.module';

@NgModule({
   declarations: [DashboardComponent, ResumeComponent, EntriesComponent, BalanceComponent],
   imports: [
      CommonModule, SharedModule, CommonSecureModule,
      DashboardRouting
   ],
   providers: [DashboardService]
})
export class DashboardModule { }
