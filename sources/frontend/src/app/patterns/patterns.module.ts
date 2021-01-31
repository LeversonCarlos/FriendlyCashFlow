import { NgModule } from '@angular/core';
import { MaterialModule } from '../material/material.exports';
import { SharedModule } from '../shared/shared.exports';
import { PatternsData } from './data/data.service';

@NgModule({
   declarations: [],
   imports: [
      MaterialModule, SharedModule
   ],
   providers: [PatternsData]
})
export class PatternsModule { }
