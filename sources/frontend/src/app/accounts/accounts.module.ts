import { NgModule } from '@angular/core';
import { MaterialModule } from '../material/material.exports';
import { SharedModule } from '../shared/shared.exports';

import { AccountsRouting } from './accounts.routing';


@NgModule({
   declarations: [],
   imports: [
      SharedModule, MaterialModule,
      AccountsRouting
   ]
})
export class AccountsModule { }
