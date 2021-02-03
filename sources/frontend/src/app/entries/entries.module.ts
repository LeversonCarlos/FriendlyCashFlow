import { NgModule } from '@angular/core';
import { MaterialModule } from '../material/material.exports';
import { SharedModule } from '../shared/shared.exports';

import { EntriesRouting } from './entries.routing';
import { EntriesData } from './data/entries.data';
import { ListComponent } from './list/list.component';
import { ListBodyComponent } from './list/list-body/list-body.component';
import { DetailsRouteViewComponent } from './details/route-view/route-view.component';
import { PatternViewComponent } from './details/pattern-view/pattern-view.component';
import { CategoryViewComponent } from './details/category-view/category-view.component';
import { AccountViewComponent } from './details/account-view/account-view.component';

@NgModule({
   declarations: [ListComponent, ListBodyComponent, DetailsRouteViewComponent,
      PatternViewComponent, CategoryViewComponent, AccountViewComponent],
   imports: [
      MaterialModule, SharedModule,
      EntriesRouting
   ],
   providers: [EntriesData]
})
export class EntriesModule { }
