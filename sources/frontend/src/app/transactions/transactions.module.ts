import { NgModule } from '@angular/core';
import { MaterialModule } from '../material/material.exports';
import { SharedModule } from '../shared/shared.exports';
import { TransactionsRouting } from './transactions.routing';
import { ListComponent } from './list/list.component';
import { DaysComponent } from './list/days/days.component';

@NgModule({
   declarations: [ListComponent, DaysComponent],
   imports: [
      MaterialModule, SharedModule,
      TransactionsRouting
   ]
})
export class TransactionsModule { }
