import { NgModule } from '@angular/core';
import { MaterialModule } from '../material/material.exports';
import { SharedModule } from '../shared/shared.exports';

import { EntriesRouting } from './entries.routing';
import { DetailsRouteViewComponent } from './details/route-view/route-view.component';
import { PatternViewComponent } from './details/pattern-view/pattern-view.component';
import { CancelComponent } from './details/commands/cancel/cancel.component';
import { ConfirmComponent } from './details/commands/confirm/confirm.component';
import { DueDateComponent } from './details/inputs/due-date/due-date.component';
import { ValueComponent } from './details/inputs/value/value.component';
import { PayDateComponent } from './details/inputs/pay-date/pay-date.component';
import { PaidComponent } from './details/inputs/paid/paid.component';
import { AccountComponent } from './details/inputs/account/account.component';
import { CategoryComponent } from './details/inputs/category/category.component';

@NgModule({
   declarations: [DetailsRouteViewComponent,
      PatternViewComponent,
      CancelComponent, ConfirmComponent,
      DueDateComponent, ValueComponent, PayDateComponent, PaidComponent,
      AccountComponent, CategoryComponent
   ],
   imports: [
      MaterialModule, SharedModule,
      EntriesRouting
   ],
   providers: []
})
export class EntriesModule { }
