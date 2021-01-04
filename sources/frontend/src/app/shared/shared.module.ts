import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { BusyComponent } from './busy/busy.component';
import { AnonymousContainerComponent } from './containers/anonymous-container/anonymous-container.component';

@NgModule({
   declarations: [
      BusyComponent,
      AnonymousContainerComponent
   ],
   imports: [
      CommonModule, FormsModule, ReactiveFormsModule
   ],
   exports: [
      CommonModule, FormsModule, ReactiveFormsModule,
      BusyComponent
   ]
})
export class SharedModule { }
