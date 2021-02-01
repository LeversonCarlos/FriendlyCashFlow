import { NgModule } from '@angular/core';
import { MaterialModule } from '../material/material.exports';
import { SharedModule } from '../shared/shared.exports';

import { AccountsRouting } from './accounts.routing';
import { AccountsData } from './data/accounts.data';
import { ListComponent } from './list/list.component';
import { DetailsRouteViewComponent } from './details/route-view/route-view.component';
import { ListBodyComponent } from './list/list-body/list-body.component';

@NgModule({
   declarations: [ListComponent, DetailsRouteViewComponent, ListBodyComponent],
   imports: [
      SharedModule, MaterialModule,
      AccountsRouting
   ],
   providers: [
      AccountsData
   ]
})
export class AccountsModule { }
