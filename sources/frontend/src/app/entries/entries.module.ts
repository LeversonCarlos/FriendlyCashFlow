import { NgModule } from '@angular/core';
import { MaterialModule } from '../material/material.exports';
import { SharedModule } from '../shared/shared.exports';

import { EntriesRouting } from './entries.routing';
import { EntriesService } from './entries.service';
import { ListComponent } from './list/list.component';
import { ListBodyComponent } from './list/list-body/list-body.component';
import { RouteViewComponent } from './details/route-view/route-view.component';

@NgModule({
   declarations: [ListComponent, ListBodyComponent, RouteViewComponent],
   imports: [
      MaterialModule, SharedModule,
      EntriesRouting
   ],
   providers: [EntriesService]
})
export class EntriesModule { }
