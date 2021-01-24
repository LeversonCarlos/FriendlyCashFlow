import { NgModule } from '@angular/core';
import { MaterialModule } from '../material/material.exports';
import { SharedModule } from '../shared/shared.exports';

import { EntriesRouting } from './entries.routing';
import { ListComponent } from './list/list.component';

@NgModule({
   declarations: [ListComponent],
   imports: [
      MaterialModule, SharedModule, ,
      EntriesRouting
   ]
})
export class EntriesModule { }
