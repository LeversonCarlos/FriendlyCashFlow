import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AccountsRouting } from './accounts.routing';
import { AccountsState, } from '@commands/accounts';
import { NavigateCommandProvider, InitializeCommandProvider, ShowAccountsCommandProvider } from '@commands/accounts';

@NgModule({
   declarations: [],
   imports: [
      CommonModule,
      AccountsRouting
   ],
   providers: [
      AccountsState,
      NavigateCommandProvider, InitializeCommandProvider, ShowAccountsCommandProvider
   ]
})
export class AccountsModule { }
