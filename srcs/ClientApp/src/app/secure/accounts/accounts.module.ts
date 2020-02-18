import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from 'src/app/shared/shared.module';

import { AccountsRouting } from './accounts.routing';
import { AccountsService } from './accounts.service';
import { AccountsComponent } from './accounts.component';
import { AccountsListComponent } from './accounts-list/accounts-list.component';
import { AccountDetailsComponent } from './account-details/account-details.component';

@NgModule({
   declarations: [AccountsListComponent, AccountDetailsComponent, AccountsComponent],
   imports: [
      CommonModule, SharedModule,
      AccountsRouting
   ],
   providers: [AccountsService]
})
export class AccountsModule { }
