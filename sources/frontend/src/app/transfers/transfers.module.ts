import { NgModule } from '@angular/core';
import { MaterialModule } from '../material/material.exports';
import { SharedModule } from '../shared/shared.exports';
import { TransfersRouting } from './transfers.routing';

@NgModule({
   declarations: [],
   imports: [
      MaterialModule, SharedModule,
      TransfersRouting
   ]
})
export class TransfersModule { }
