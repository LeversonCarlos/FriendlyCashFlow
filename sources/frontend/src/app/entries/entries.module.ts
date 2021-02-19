import { NgModule } from '@angular/core';
import { MaterialModule } from '../material/material.exports';
import { SharedModule } from '../shared/shared.exports';

import { EntriesRouting } from './entries.routing';
import { DetailsRouteViewComponent } from './details/route-view/route-view.component';
import { PatternViewComponent } from './details/pattern-view/pattern-view.component';
import { CategoryViewComponent } from './details/category-view/category-view.component';
import { AccountViewComponent } from './details/account-view/account-view.component';
import { DueDateViewComponent } from './details/due-date-view/due-date-view.component';
import { EntryValueViewComponent } from './details/entry-value-view/entry-value-view.component';
import { PayDateViewComponent } from './details/pay-date-view/pay-date-view.component';
import { PaidViewComponent } from './details/paid-view/paid-view.component';
import { CancelComponent } from './details/commands/cancel/cancel.component';

@NgModule({
   declarations: [DetailsRouteViewComponent,
      PatternViewComponent, CategoryViewComponent, AccountViewComponent,
      DueDateViewComponent, EntryValueViewComponent, PayDateViewComponent, PaidViewComponent,
      CancelComponent
   ],
   imports: [
      MaterialModule, SharedModule,
      EntriesRouting
   ],
   providers: []
})
export class EntriesModule { }
