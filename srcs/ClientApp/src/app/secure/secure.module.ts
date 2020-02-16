import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from '../shared/shared.module';

import { AccountsService } from './accounts/accounts.service';
import { AccountsComponent } from './accounts/accounts.component';
import { AccountDetailsComponent } from './accounts/account-details/account-details.component';

import { CategoriesService } from './categories/categories.service';
import { CategoriesComponent } from './categories/categories.component';
import { CategoryDetailsComponent } from './categories/category-details/category-details.component';

import { CommonResumeComponent } from './common/resume/resume.component';
import { MonthPickerComponent } from './pickers/month-picker/month-picker.component';
import { AccountPickerComponent } from './pickers/account-picker/account-picker.component';

import { EntriesService } from './entries/entries.service';
import { PatternsService } from './patterns/patterns.service';
import { RecurrencyService } from './recurrency/recurrency.service';
import { TransfersService } from './transfers/transfers.service';
import { EntriesFlowComponent } from './entries/entries-flow/entries-flow.component';
import { EntryDetailsComponent } from './entries/entry-details/entry-details.component';
import { TransferDetailsComponent } from './entries/transfer-details/transfer-details.component';
import { EntriesResumeComponent } from './entries/entries-resume/entries-resume.component';
import { AddButtonComponent } from './entries/add-button/add-button.component';
import { SearchPanelComponent } from './entries/search-panel/search-panel.component';

import { DashboardService } from './dashboard/dashboard.service';
import { DashboardComponent } from './dashboard/dashboard.component';
import { BalanceComponent } from './dashboard/balance/balance.component';
import { ResumeComponent } from './dashboard/resume/resume.component';
import { EntriesComponent } from './dashboard/entries/entries.component';
import { SettingsComponent } from './settings/settings.component';
import { PasswordChangeComponent } from './settings/password-change/password-change.component';
import { AnalyticsComponent } from './analytics/analytics.component';
import { CategoryGoalsComponent } from './analytics/category-goals/category-goals.component';
import { AnalyticsFiltersComponent } from './analytics/analytics-filters/analytics-filters.component';
import { EntriesParetoComponent } from './analytics/entries-pareto/entries-pareto.component';
import { MonthlyTargetComponent } from './analytics/monthly-target/monthly-target.component';


@NgModule({
   declarations: [
      AccountsComponent, AccountDetailsComponent,
      CategoriesComponent, CategoryDetailsComponent,
      CommonResumeComponent, MonthPickerComponent, AccountPickerComponent,
      EntriesFlowComponent, EntryDetailsComponent, TransferDetailsComponent, EntriesResumeComponent,
      AddButtonComponent, SearchPanelComponent,
      DashboardComponent, BalanceComponent, ResumeComponent, EntriesComponent,
      SettingsComponent, PasswordChangeComponent,
      AnalyticsComponent, AnalyticsFiltersComponent, CategoryGoalsComponent, EntriesParetoComponent, MonthlyTargetComponent],
   imports: [
      CommonModule, SharedModule
   ],
   providers: [AccountsService, CategoriesService, EntriesService, TransfersService,
      PatternsService, RecurrencyService, DashboardService]
})
export class SecureModule { }
