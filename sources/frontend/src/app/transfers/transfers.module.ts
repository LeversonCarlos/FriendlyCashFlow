import { NgModule } from '@angular/core';
import { MaterialModule } from '../material/material.exports';
import { SharedModule } from '../shared/shared.exports';
import { TransfersRouting } from './transfers.routing';
import { DetailsComponent } from './details/details.component';
import { CancelComponent } from './details/cancel/cancel.component';

@NgModule({
   declarations: [DetailsComponent,
      CancelComponent],
   imports: [
      MaterialModule, SharedModule,
      TransfersRouting
   ]
})
export class TransfersModule { }
