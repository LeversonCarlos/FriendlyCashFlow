import { NgModule } from '@angular/core';
import { MaterialModule } from '../material/material.exports';
import { SharedModule } from '../shared/shared.exports';

import { EntriesRouting } from './entries.routing';
import { DetailsComponent } from './details/details.component';
import { DueDateComponent } from './details/inputs/due-date/due-date.component';
import { ValueComponent } from './details/inputs/value/value.component';
import { PayDateComponent } from './details/inputs/pay-date/pay-date.component';
import { PaidComponent } from './details/inputs/paid/paid.component';
import { AccountComponent } from './details/inputs/account/account.component';
import { CategoryComponent } from './details/inputs/category/category.component';
import { PatternComponent } from './details/inputs/pattern/pattern.component';
import { CancelComponent } from './details/commands/cancel/cancel.component';
import { ConfirmComponent } from './details/commands/confirm/confirm.component';

@NgModule({
   declarations: [DetailsComponent,
      DueDateComponent, ValueComponent, PayDateComponent, PaidComponent,
      AccountComponent, CategoryComponent, PatternComponent,
      CancelComponent, ConfirmComponent
   ],
   imports: [
      MaterialModule, SharedModule,
      EntriesRouting
   ],
   providers: []
})
export class EntriesModule { }
