import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TransactionsRouting } from './transactions.routing';
import { ListComponent } from './list/list.component';

@NgModule({
   declarations: [ListComponent],
   imports: [
      CommonModule,
      TransactionsRouting
   ]
})
export class TransactionsModule { }
