import { NgModule } from '@angular/core';
import { MaterialModule } from '../material/material.exports';
import { SharedModule } from '../shared/shared.exports';

import { EntriesRouting } from './entries.routing';
import { DetailsRouteViewComponent } from './details/route-view/route-view.component';
import { PatternViewComponent } from './details/pattern-view/pattern-view.component';
import { CategoryViewComponent } from './details/category-view/category-view.component';
import { AccountViewComponent } from './details/account-view/account-view.component';
import { PaidViewComponent } from './details/paid-view/paid-view.component';
import { CancelComponent } from './details/commands/cancel/cancel.component';
import { ConfirmComponent } from './details/commands/confirm/confirm.component';
import { DueDateComponent } from './details/inputs/due-date/due-date.component';
import { ValueComponent } from './details/inputs/value/value.component';
import { PayDateComponent } from './details/inputs/pay-date/pay-date.component';

@NgModule({
   declarations: [DetailsRouteViewComponent,
      PatternViewComponent, CategoryViewComponent, AccountViewComponent,
      PaidViewComponent,
      CancelComponent, ConfirmComponent,
      DueDateComponent, ValueComponent, PayDateComponent
   ],
   imports: [
      MaterialModule, SharedModule,
      EntriesRouting
   ],
   providers: []
})
export class EntriesModule { }
