import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { SharedModule } from '../shared/shared.module';
import { AccountsComponent } from './accounts/accounts.component';

@NgModule({
  declarations: [AccountsComponent],
  imports: [
    CommonModule, SharedModule
  ]
})
export class SecureModule { }
