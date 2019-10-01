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

@NgModule({
   declarations: [
      AccountsComponent, AccountDetailsComponent,
      CategoriesComponent, CategoryDetailsComponent, EntriesFlowComponent],
   imports: [
      CommonModule, SharedModule
   ],
   providers: [AccountsService, CategoriesService, EntriesService]
})
export class SecureModule { }
