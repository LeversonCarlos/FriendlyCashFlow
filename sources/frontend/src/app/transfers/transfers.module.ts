import { NgModule } from '@angular/core';
import { MaterialModule } from '../material/material.exports';
import { SharedModule } from '../shared/shared.exports';
import { TransfersRouting } from './transfers.routing';
import { DetailsComponent } from './details/details.component';

@NgModule({
   declarations: [DetailsComponent],
   imports: [
      MaterialModule, SharedModule,
      TransfersRouting
   ]
})
export class TransfersModule { }
