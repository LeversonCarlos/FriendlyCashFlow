import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BusyComponent } from './busy/busy.component';

@NgModule({
   declarations: [
      BusyComponent
   ],
   imports: [
      CommonModule
   ],
   exports: [
      BusyComponent
   ]
})
export class SharedModule { }
