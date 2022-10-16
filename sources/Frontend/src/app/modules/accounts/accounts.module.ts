import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ComponentsModule } from '@components/components.module';

import { AccountsRouting } from './accounts.routing';
import { SearchRepository } from '@repositories/accounts';
import { AccountsState, InitializeCommand, NavigateCommand, ShowAccountsCommand, } from '@commands/accounts';

import { ContainerComponent } from './views/container.component';
import { IndexComponent } from './views/index/index.component';
import { IndexListComponent } from './views/index/list/list.component';

@NgModule({
   declarations: [
      ContainerComponent,
      IndexComponent,
      IndexListComponent
   ],
   imports: [
      CommonModule, ComponentsModule,
      AccountsRouting
   ],
   providers: [
      AccountsState,
      SearchRepository,
      NavigateCommand, InitializeCommand, ShowAccountsCommand,
   ],
   bootstrap: [
      ContainerComponent
   ]
})
export class AccountsModule { }
