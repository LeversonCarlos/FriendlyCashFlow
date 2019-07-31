import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { SharedModule } from '../shared/shared.module';
import { AccountsComponent } from './accounts/accounts.component';
import { AccountsService } from './accounts/accounts.service';
import { AccountDetailsComponent } from './accounts/account-details/account-details.component';

@NgModule({
   declarations: [AccountsComponent, AccountDetailsComponent],
   imports: [
      CommonModule, SharedModule
   ],
   providers: [AccountsService]
})
export class SecureModule { }
