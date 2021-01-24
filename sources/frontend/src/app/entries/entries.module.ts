import { NgModule } from '@angular/core';
import { MaterialModule } from '../material/material.exports';
import { SharedModule } from '../shared/shared.exports';

import { EntriesRouting } from './entries.routing';

@NgModule({
   declarations: [],
   imports: [
      MaterialModule, SharedModule, ,
      EntriesRouting
   ]
})
export class EntriesModule { }
