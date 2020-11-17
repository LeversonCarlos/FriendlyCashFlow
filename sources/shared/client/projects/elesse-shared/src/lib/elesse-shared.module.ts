import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { BusyComponent } from './busy/busy.component';

@NgModule({
   declarations: [BusyComponent],
   imports: [
      CommonModule, FormsModule, ReactiveFormsModule, HttpClientModule,
   ],
   exports: [
      CommonModule, FormsModule, ReactiveFormsModule, HttpClientModule,
      BusyComponent
   ]
})
export class ElesseSharedModule { }
