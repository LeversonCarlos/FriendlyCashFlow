import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from '../shared/shared.module';

import { AccountsService } from './accounts/accounts.service';
import { AccountsComponent } from './accounts/accounts.component';
import { AccountDetailsComponent } from './accounts/account-details/account-details.component';

import { CategoriesService } from './categories/categories.service';
import { CategoriesComponent } from './categories/categories.component';

@NgModule({
   declarations: [AccountsComponent, AccountDetailsComponent, CategoriesComponent],
   imports: [
      CommonModule, SharedModule
   ],
   providers: [AccountsService, CategoriesService]
})
export class SecureModule { }
