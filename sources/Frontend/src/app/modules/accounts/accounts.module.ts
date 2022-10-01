import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ComponentsModule } from '@components/components.module';

import { AccountsRouting } from './accounts.routing';
import { AccountsState, } from '@commands/accounts';
import { NavigateCommandProvider, InitializeCommandProvider, ShowAccountsCommandProvider } from '@commands/accounts';

import { ContainerComponent } from './views/container.component';
import { IndexComponent } from './views/index/index.component';

@NgModule({
   declarations: [
      ContainerComponent,
      IndexComponent
   ],
   imports: [
      CommonModule, ComponentsModule,
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
