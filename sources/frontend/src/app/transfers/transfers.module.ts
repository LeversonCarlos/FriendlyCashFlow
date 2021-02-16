import { NgModule } from '@angular/core';
import { MaterialModule } from '../material/material.exports';
import { SharedModule } from '../shared/shared.exports';
import { TransfersRouting } from './transfers.routing';
import { DetailsComponent } from './details/details.component';
import { CancelComponent } from './details/commands/cancel/cancel.component';
import { ConfirmComponent } from './details/commands/confirm/confirm.component';
import { ValueComponent } from './details/inputs/value/value.component';
import { DateComponent } from './details/inputs/date/date.component';

@NgModule({
   declarations: [DetailsComponent,
      CancelComponent, ConfirmComponent,
      ValueComponent, DateComponent
   ],
   imports: [
      MaterialModule, SharedModule,
      TransfersRouting
   ]
})
export class TransfersModule { }
