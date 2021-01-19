import { NgModule } from '@angular/core';
import { MaterialModule } from '../material/material.exports';
import { SharedModule } from '../shared/shared.exports';

import { CategoriesRouting } from './categories.routing';

@NgModule({
   declarations: [],
   imports: [
      MaterialModule, SharedModule,
      CategoriesRouting
   ]
})
export class CategoriesModule { }
