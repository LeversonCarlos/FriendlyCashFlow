import { NgModule } from '@angular/core';
import { MaterialModule } from '../material/material.exports';
import { SharedModule } from '../shared/shared.exports';

import { AccountsRouting } from './accounts.routing';
import { AccountsService } from './accounts.service';
import { ListComponent } from './list/list.component';

@NgModule({
   declarations: [ListComponent],
   imports: [
      SharedModule, MaterialModule,
      AccountsRouting
   ],
   providers: [
      AccountsService
   ]
})
export class AccountsModule { }
