import { NgModule } from '@angular/core';
import { MaterialModule } from '../material/material.exports';
import { SharedModule } from '../shared/shared.exports';

import { CategoriesRouting } from './categories.routing';
import { CategoriesService } from './categories.service';
import { ListComponent } from './list/list.component';
import { ListBodyComponent } from './list/list-body/list-body.component';

@NgModule({
   declarations: [ListComponent, ListBodyComponent],
   imports: [
      MaterialModule, SharedModule,
      CategoriesRouting
   ],
   providers: [CategoriesService]
})
export class CategoriesModule { }
