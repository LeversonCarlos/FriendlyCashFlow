import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AccountsRouting } from './accounts.routing';
import { AccountsState, } from '@commands/accounts';
import { NavigateCommandProvider, InitializeCommandProvider, ShowAccountsCommandProvider } from '@commands/accounts';

import { ContainerComponent } from './views/container.component';

@NgModule({
   declarations: [
      ContainerComponent
   ],
   imports: [
      CommonModule,
      AccountsRouting
   ],
   providers: [
      AccountsState,
      NavigateCommandProvider, InitializeCommandProvider, ShowAccountsCommandProvider
   ],
   bootstrap: [
      ContainerComponent
   ]
})
export class AccountsModule { }
