import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from '../shared/shared.module';

import { AccountsService } from './accounts/accounts.service';
import { AccountsComponent } from './accounts/accounts.component';
import { AccountDetailsComponent } from './accounts/account-details/account-details.component';

import { CategoriesService } from './categories/categories.service';
import { CategoriesComponent } from './categories/categories.component';
import { CategoryDetailsComponent } from './categories/category-details/category-details.component';

import { EntriesService } from './entries/entries.service';
import { EntriesFlowComponent } from './entries/entries-flow/entries-flow.component';
import { MonthPickerComponent } from './pickers/month-picker/month-picker.component';
import { AddButtonComponent } from './entries/add-button/add-button.component';
import { EntryDetailsComponent } from './entries/entry-details/entry-details.component';
import { PatternsService } from './patterns/patterns.service';
import { RecurrencyService } from './recurrency/recurrency.service';
import { TransferDetailsComponent } from './entries/transfer-details/transfer-details.component';
import { TransfersService } from './transfers/transfers.service';
import { AccountPickerComponent } from './pickers/account-picker/account-picker.component';
import { SearchPanelComponent } from './entries/search-panel/search-panel.component';

@NgModule({
   declarations: [
      AccountsComponent, AccountDetailsComponent,
      CategoriesComponent, CategoryDetailsComponent,
      MonthPickerComponent, AccountPickerComponent,
      EntriesFlowComponent, EntryDetailsComponent, TransferDetailsComponent,
      AddButtonComponent, SearchPanelComponent],
   imports: [
      CommonModule, SharedModule
   ],
   providers: [AccountsService, CategoriesService, EntriesService, PatternsService, RecurrencyService, TransfersService]
})
export class SecureModule { }
