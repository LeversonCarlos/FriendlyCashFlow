import { NgModule } from '@angular/core';
import { MaterialModule } from '../material/material.exports';
import { SharedModule } from '../shared/shared.exports';

import { AccountsRouting } from './accounts.routing';
import { AccountsService } from './accounts.service';
import { ListComponent } from './list/list.component';
import { DetailsRouteViewComponent } from './details/route-view/route-view.component';

@NgModule({
   declarations: [ListComponent, DetailsRouteViewComponent],
   imports: [
      SharedModule, MaterialModule,
      AccountsRouting
   ],
   providers: [
      AccountsService
   ]
})
export class AccountsModule { }
