import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { BusyComponent } from './busy/busy.component';

@NgModule({
   declarations: [BusyComponent],
   imports: [
      CommonModule, FormsModule, ReactiveFormsModule,
   ],
   exports: [
      CommonModule, FormsModule, ReactiveFormsModule,
      BusyComponent
   ]
})
export class ElesseSharedModule { }
