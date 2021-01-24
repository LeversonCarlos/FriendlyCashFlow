import { NgModule } from '@angular/core';
import { MaterialModule } from '../material/material.exports';
import { SharedModule } from '../shared/shared.exports';

import { EntriesRouting } from './entries.routing';
import { EntriesService } from './entries.service';
import { ListComponent } from './list/list.component';

@NgModule({
   declarations: [ListComponent],
   imports: [
      MaterialModule, SharedModule, ,
      EntriesRouting
   ],
   providers: [EntriesService]
})
export class EntriesModule { }
