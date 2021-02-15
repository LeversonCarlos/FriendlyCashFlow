import { NgModule } from '@angular/core';
import { MaterialModule } from '../material/material.exports';
import { SharedModule } from '../shared/shared.exports';
import { TransactionsRouting } from './transactions.routing';
import { ListComponent } from './list/list.component';
import { DaysComponent } from './list/days/days.component';
import { DayHeaderComponent } from './list/day-header/day-header.component';

@NgModule({
   declarations: [ListComponent, DaysComponent, DayHeaderComponent],
   imports: [
      MaterialModule, SharedModule,
      TransactionsRouting
   ]
})
export class TransactionsModule { }
